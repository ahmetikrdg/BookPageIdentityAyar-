using System.Linq;
using bookpage.entity;
using Microsoft.EntityFrameworkCore;

namespace bookpage.data.Concrate.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context=new ShopContext();
            if(context.Database.GetPendingMigrations().Count()==0)
            {
                if(context.Categories.Count()==0)
                {
                    context.Categories.AddRange(Categori);
                }
            }

            if(context.Database.GetPendingMigrations().Count()==0)
            {
                if(context.Products.Count()==0)
                {
                    context.Products.AddRange(Product);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }

        private static Category[] Categori=
        {
            new Category(){Name="Roman",Url="roman"},
            new Category(){Name="Kişisel Gelişim",Url="kisisel-gelisim"},
            new Category(){Name="Bilim",Url="bilim"}
        };
        private static Product[] Product=
        {
            new Product(){Name="Sadece Aptallar 8 Saat Uyur",Url="sadece-aptallar-8-saat-uyur",Description="hagara hugara şagara şugara",Author="Erdal Demirkıran",Pages=180,ImageUrl="1.jpg.jpg",IsApproved=true},
            new Product(){Name="Ruhsal Zeka",Url="ruhsal-zeka",Description="hagara hugara şagara şugara",Author="Muhammed Bozdağ",Pages=230,ImageUrl="2.jpg.jpg",IsApproved=true},
            new Product(){Name="İrade Terbiyesi",Url="irade-terbiyesi",Description="hagara hugara şagara şugara",Author="Jules Payot",Pages=160,ImageUrl="3.jpg.jpg",IsApproved=true},
            new Product(){Name="Ben Dünyanın En Akıllı İnsanıyım",Url="ben-dünyanın-en-akıllı-insanıyım",Description="hagara hugara şagara şugara",Author="Erdal Demirkıran",Pages=290,ImageUrl="4.jpg.jpg",IsApproved=true},
            new Product(){Name="Mutlu Beyin",Url="mutlu-beyin",Description="hagara hugara şagara şugara",Author="Loretta Greziona",Pages=260,ImageUrl="5.jpg",IsApproved=true},
            new Product(){Name="Nefes",Url="nefes",Description="hagara hugara şagara şugara",Author="Michael Katz Krefeld",Pages=280,ImageUrl="6.jpg",IsApproved=true}, 
            new Product(){Name="Nefes",Url="nefes",Description="Burada kitabın açıklaması var.................",Author="Jean Christophe",Pages=310,ImageUrl="9.jpg.jpg",IsApproved=true}, 
            new Product(){Name="Sözlü Dövüş Sanatı",Url="tong-fu",Description="Burada kitabın açıklaması var.................",Author="Sam Horn",Pages=280,ImageUrl="7.jpg.jpg",IsApproved=true}, 
            new Product(){Name="İnsan Neyle Yaşar",Url="insan-neyle-yasar",Description="Burada kitabın açıklaması var.................",Author="L.N Tolstoy",Pages=140,ImageUrl="8.jpg.jpg",IsApproved=true}, 
        };

        private static ProductCategory[] ProductCategories=
        {//çoka çok ilişki kurmak için oluşturulan yapı.-->context üzerine ekliyorum yukarda.-->Daha sonra webui içindeki bookdb'yi sildim ve update database sorgusu yazdım.-->sonra webui run ettim ki veriler vtye gitsin.
            new ProductCategory(){products=Product[0],Categories=Categori[0]},//sadece aptallar kitabı Roman 
            new ProductCategory(){products=Product[0],Categories=Categori[2]},//aynı zamanda Bilim kategorisindede olsun
            new ProductCategory(){products=Product[1],Categories=Categori[0]},//Ruhsal zeka romana
            new ProductCategory(){products=Product[1],Categories=Categori[1]},//ve kişisel gelişime karşılık gelsin
            new ProductCategory(){products=Product[2],Categories=Categori[0]},//irade terbiyesi Romana
            new ProductCategory(){products=Product[2],Categories=Categori[1]},//ve Kişisele
            new ProductCategory(){products=Product[3],Categories=Categori[1]},//Ben dünyanın en Sadece Kişisele
            new ProductCategory(){products=Product[4],Categories=Categori[2]},//Mutlu beyin Bilime 
            new ProductCategory(){products=Product[4],Categories=Categori[1]},//ve Kişisel gelişime
            new ProductCategory(){products=Product[5],Categories=Categori[2]}//nefes sadece bilime

        };
    }
}