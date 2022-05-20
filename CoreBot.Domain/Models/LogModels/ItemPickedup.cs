namespace CoreBot.Domain.Models.LogModels;

public record ItemPickedup : IBaseLogModel
{
    public Role DroppedBy { get; set; }
    public Role PickedupBy { get; set; }
    public DateTime Date { get; set; }
    public long ItemId { get; set; }    

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (this?.DroppedBy != null)
            sb.AppendLine($"Dropado por: {this?.DroppedBy?.Name}({this?.DroppedBy?.Id})");

        sb.AppendLine($"Item dropado: {ItemId}");

        if (this?.PickedupBy != null)
            sb.AppendLine($"Coletado por: {this?.DroppedBy?.Name}({this?.DroppedBy?.Id})");

        sb.AppendLine($"Data: {Date.ToString("G")}");

        return sb.ToString();
    }
}
