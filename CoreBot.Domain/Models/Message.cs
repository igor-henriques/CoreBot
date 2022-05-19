namespace CoreBot.Domain.Model;

public record Message
{
    public int? IdFrom { get; init; }
    public int? IdTo { get; init; }
    public string RoleNameFrom { get; init; }
    public string RoleNameTo { get; init; }
    public string Content { get; init; }
}
