namespace CoreBot.Domain.Models.LogModels;

public class Chat : IBaseLogModel
{
    public Role SentFrom { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (SentFrom is not null)
            sb.AppendLine($"Mensagem enviada por: {this?.SentFrom?.Name}");

        sb.AppendLine(Content);

        sb.AppendLine($"Data: {Date.ToString("G")}");

        return sb.ToString();
    }
}
