namespace CoreBot.Utils;

public static class LogWriter
{
    public static void Write<T>(this ILogger<T> logger, string message)
    {
        try
        {
            using StreamWriter w = File.AppendText("./log.txt");

            Log(message, w);

            logger.LogInformation(message);
        }
        catch { }
    }

    private static void Log(string logMessage, TextWriter txtWriter)
    {
        txtWriter.Write("\r\nLog Entry : ");
        txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            DateTime.Now.ToLongDateString());
        txtWriter.WriteLine("  :");
        txtWriter.WriteLine("  :{0}", logMessage);
        txtWriter.WriteLine("-------------------------------");

        Console.WriteLine(logMessage);
    }
}
