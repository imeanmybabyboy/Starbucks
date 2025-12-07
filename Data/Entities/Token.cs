namespace ASP_Starbucks.Data.Entities
{
    public class Token
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

        public User User { get; set; } = null!; // nav property
    }
}
