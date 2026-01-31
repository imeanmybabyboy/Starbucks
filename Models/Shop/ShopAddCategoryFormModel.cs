namespace ASP_Starbucks.Models.Shop
{
    public class ShopAddCategoryFormModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}
