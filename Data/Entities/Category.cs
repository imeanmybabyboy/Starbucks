namespace ASP_Starbucks.Data.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public ICollection<Subcategory> Subcategories { get; set; } = [];
    }
}
