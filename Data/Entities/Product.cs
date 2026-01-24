namespace ASP_Starbucks.Data.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;


        public string Ingredients { get; set; } = null!;
        public Guid SizeId { get; set; }
        public Guid SubcategoryId { get; set; }


        public int Calories { get; set; }
        public int CaloriesFromFat { get; set; }
        
        
        public int TotalFatG { get; set; }
        public int TotalFatPercent { get; set; }
        
        
        public int SaturatedFatG { get; set; }
        public int SaturatedFatPercent { get; set; }


        public int TransFatG { get; set; }
        public int TransFatPercent { get; set; }


        public int CholesterolMg { get; set; }
        public int CholesterolPercent { get; set; }


        public int SodiumMg { get; set; }
        public int SodiumPercent { get; set; }


        public int TotalCarbs { get; set; }
        public int DietFiber { get; set; }
        public int Sugars { get; set; }

        public int Protein { get; set; }

        public string? Allergens { get; set; }


        public Subcategory Subcategory { get; set; } = null!;
        public Size Size { get; set; } = null!;
    }
}
