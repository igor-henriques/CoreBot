﻿namespace CoreRanking.License;

public class LicenseControl : BackgroundService
{
    private readonly CoreLicense _license;
    private readonly ILogger<LicenseControl> _logger;
    private readonly HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
    private readonly CancellationTokenSource hwidTimeout = new CancellationTokenSource();
    private readonly Timer timeoutTimer;

    public LicenseControl(ILogger<LicenseControl> logger, CoreLicense license)
    {
        this._logger = logger;
        this._license = license;

        this.timeoutTimer = new Timer((obj) => hwidTimeout.Cancel(), null, 10_000, 2_000);
    }

    protected override async Task ExecuteAsync(System.Threading.CancellationToken stoppingToken)
    {
        _logger.Write("MÓDULO DE LICENÇA INICIADO");

        this._license.Hwid = await UserHWID(hwidTimeout.Token);

        if (string.IsNullOrEmpty(this._license.Hwid))
        {
            _logger.Write("Não foi possível obter o registro de HWID.");
            Process.GetCurrentProcess().Kill();
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Check();
            await Task.Delay(1_800_000);
        }
    }
    public override Task StartAsync(System.Threading.CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }
    public override Task StopAsync(System.Threading.CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }

    private async Task Check()
    {
        try
        {
            using var httpClient = HttpClientFactory.Create();

            var response = (State)int.Parse(await httpClient.GetStringAsync($"http://license.ironside.dev/api/license/{_license.User}/{_license.Licensekey}/{_license.Hwid}/{Enum.GetName(typeof(Product), (int)_license.Product)}"));

            var logMessage = response switch
            {
                State.Erro => "Houve um erro na requisição da licença.",
                State.Esgotado => "Sua licença já está registrada em outra instância.",
                State.Inexiste => "Sua licença não existe.",
                State.Expirado => "Sua licença está fora da validade.",
                State.Inativo => "Sua licença não está ativa.",
                State.InvalidProduct => "Sua não pode ser utilizada neste produto.",
                State.Welcome => "Licença validada com sucesso.",
                State.Valido => "Licença validada com sucesso.",
                _ => "Houve um erro na requisição da licença."
            };

            logMessage += response != (State.Valido | State.Welcome) ? " Entre em contato com a administração. Discord: Ironside#3862" : default;

            _logger.Write(logMessage);

            if (response != (State.Valido | State.Welcome))
                Process.GetCurrentProcess().Kill();
        }
        catch (Exception e)
        {
            _logger.Write(e.ToString());

            Process.GetCurrentProcess().Kill();
        }
    }

    private async Task<string> UserHWID(CancellationToken cancellationToken)
    {
        string ipAddress = await client.GetStringAsync("https://ipv4.icanhazip.com/", cancellationToken);

        return ipAddress?.Replace("\n", default)?.Replace(".", default);
    }
}