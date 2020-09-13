using System.Collections.Generic;

namespace bookpage.entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Author { get; set; }
        public double? Pages { get; set; }//? koymasaydım 0 olarak alırdı ve işlevsel olmazdı
        public string Description { get; set; }
        // public string Door { get; set; }
        // public string Moduls1 { get; set; }
        // public string Moduls2 { get; set; }
        // public string Moduls3 { get; set; }
        // public string Moduls4 { get; set; }
        public string ImageUrl { get; set; }
        public bool  IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}