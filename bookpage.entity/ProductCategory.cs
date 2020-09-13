using System;

namespace bookpage.entity
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public Product products { get; set; }

        public int CategoryId { get; set; }
        public Category Categories { get; set; }

        
    }
}