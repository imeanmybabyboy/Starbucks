namespace ASP_Starbucks.Data.Entities
{
    public class Subcategory
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public Category Category { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = [];
    }
}
