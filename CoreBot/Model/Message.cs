namespace CoreBot.Model
{
    public record Message
    {
        public int RoleID { get; init; }
        public string RoleName { get; init; }
        public string Content { get; init; }
    }
}
