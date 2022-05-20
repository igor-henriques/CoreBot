namespace CoreBot.Domain.Models.LogModels;

public record ItemDropped : IBaseLogModel
{
    public Role Dropped { get; set; }
    public int ItemId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (Dropped is not null)
            sb.AppendLine($"Dropado por: {this?.Dropped?.Name}({this?.Dropped?.Id})");

        sb.AppendLine($"Item dropado: {ItemId}");

        sb.AppendLine($"Quantidade: {Amount}");

        sb.AppendLine($"Data: {Date.ToString("G")}");

        return sb.ToString();
    }
}
