﻿namespace CoreBot.Producers;

internal class ChatProducer : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<ChatProducer> _logger;
    private long lastSize;
    private readonly string path;

    public ChatProducer(IServiceProvider services, ILogger<ChatProducer> logger, ServerConnection serverConnection)
    {
        this._taskQueue = (IBackgroundTaskQueue)services.GetService(typeof(LogTaskQueue));
        this._logger = logger;
        this.path = Path.Combine(serverConnection.LogsPath, "world2.chat");
        this.lastSize = GetFileSize(this.path);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(ChatProducer)} inicializado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                long fileSize = GetFileSize(path);

                if (fileSize <= lastSize)
                    continue;

                var logs = await ReadTail(path, UpdateLastFileSize(fileSize));

                foreach (var log in logs)
                {
                    await _taskQueue.QueueBackgroundWorkItemAsync(() => log());
                }

                await Task.Delay(250);
            }
            catch (Exception ex)
            {
                _logger.Write(ex.ToString());
            }
        }
    }

    private async Task<IEnumerable<Func<ValueTask>>> ReadTail(string filename, long offset)
    {
        try
        {
            var logs = await Task.Run(() =>
            {
                byte[] bytes;

                using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fs.Seek(offset * -1, SeekOrigin.End);

                    bytes = new byte[offset];
                    fs.Read(bytes, 0, (int)offset);
                }

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                var logs = GB2312ToUtf8(bytes)
                       .Split(new string[] { "\n" }[0])
                       .Where(x => !string.IsNullOrEmpty(x.Trim()))
                       .Select(log => BuildTask(log))
                       .ToList();

                return logs;
            });

            return logs;
        }
        catch (Exception ex)
        {
            _logger.Write(ex.ToString());
        }

        return default;
    }

    private Func<ValueTask> BuildTask(string log)
    {
        var task = new Func<ValueTask>(async () =>
        {
            await ChatConsumer.Process(log);
        });

        return task;
    }

    private string GB2312ToUtf8(byte[] gb2312bytes)
    {
        Encoding fromEncoding = Encoding.GetEncoding("GB2312");
        Encoding toEncoding = Encoding.UTF8;
        return EncodingConvert(gb2312bytes, fromEncoding, toEncoding);
    }

    private string EncodingConvert(byte[] fromBytes, Encoding fromEncoding, Encoding toEncoding)
    {
        byte[] toBytes = Encoding.Convert(fromEncoding, toEncoding, fromBytes);

        string toString = toEncoding.GetString(toBytes);
        return toString;
    }

    private long UpdateLastFileSize(long fileSize)
    {
        long difference = fileSize - lastSize;
        lastSize = fileSize;

        return difference;
    }

    public long GetFileSize(string fileName)
    {
        return new FileInfo(fileName).Length;
    }
}