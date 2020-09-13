using System.Collections.Generic;
using bookpage.entity;

namespace bookpage.webui.Models
{
    public class ProductDetailModel
    {//hem ürün hem category bilgisini bana taşıyacak olan bir yapı
        public Product product { get; set; }
        public List<Category> Category { get; set; }
    }
}