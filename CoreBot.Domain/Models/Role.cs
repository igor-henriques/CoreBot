namespace CoreBot.Domain.Models;

public record Role
{
    public long AccountId { get; set; }
    public long Id { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }   
}