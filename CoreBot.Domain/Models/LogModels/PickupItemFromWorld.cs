namespace CoreBot.Domain.Models.LogModels;

public class PickupItemFromWorldModel : IBaseLogModel
{
    public Role Dropped { get; set; }
    public Role Pickedup { get; set; }
    public DateTime Date { get; set; }
    public long ItemId { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (this?.Dropped != null)
            sb.AppendLine($"Dropado por: {this?.Dropped?.Name}({this?.Dropped?.Id})");

        sb.AppendLine($"Item dropado: {ItemId}");

        if (this?.Pickedup != null)
            sb.AppendLine($"Coletado por: {this?.Dropped?.Name}({this?.Dropped?.Id})");

        sb.AppendLine($"Data: {Date.ToString("G")}");

        return sb.ToString();
    }
}
