namespace CoreBot.Domain.Models;

public record Role
{
    public long AccountId { get; set; }
    public long Id { get; set; }
    public string Name { get; set; }
    public int Race { get; set; }

    public static Role ToRole(GRoleData roleData)
    {        
        return new Role
        {
            AccountId = roleData.GRoleBase.UserId,
            Id = roleData.GRoleBase.Id,
            Name = roleData.GRoleBase.Name,
            Race = roleData.GRoleBase.Race
        };
    }
}