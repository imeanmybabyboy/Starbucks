namespace ASP_Starbucks.Data.Entities
{
    public class Size
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Volume { get; set; }

        public ICollection<Product> Products { get; set; } = [];
    }
}
