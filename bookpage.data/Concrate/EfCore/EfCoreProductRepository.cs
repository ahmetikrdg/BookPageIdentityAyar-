using System.Collections.Generic;
using System.Linq;
using bookpage.data.Abstract;
using bookpage.entity;
using Microsoft.EntityFrameworkCore;

namespace bookpage.data.Concrate.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public Product GetByIdWithCategories(int id)
        {
            using(var context=new ShopContext())
            {
                return context.Products//product üzerinden sorgulama yapıcaz
                .Where(i=>i.ProductId==id)//idye göre olacak sorgulama
                .Include(i=>i.ProductCategories)
                .ThenInclude(i=>i.Categories)//her aldığım prıdtcat bilgisi üzerinden category bilgisini yükliycezb
                .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {//ilgili kategori null değilse tüm ürün bilerini getirsin
             using(var context=new ShopContext())
            {
                var product= context.Products.AsQueryable();//asquarible biz sorguyu yazıyoruz ama vtye göndermeden önce üzerine ekstra link kriter belirlemek istiyorum demek
                if (!string.IsNullOrEmpty(category))//kategori boş değilse kategoriye göre filitrele
                {
                    product=product//ürün bilgilerinin
                    .Include(i=>i.ProductCategories)//productcategorislerini
                    .ThenInclude(i=>i.Categories)//sonra kategorilerini yüklüyoruz.Daha sonra şart ekleyeceğiz şart en son çünkü ilgili kayıtların referanslarına ulaşmam lazım
                    .Where(i=>i.ProductCategories.Any(a=>a.Categories.Url==category));//ilgili kaydın productcategorislerine gidiyoruz kategorilerine geçiyoruz ve gönderdiğimiz kategoriye ait bir ürün varsa any bana true döndürür oda o ürünü bana getir demek 
                }
                return product.Count();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using(var context=new ShopContext())
            {
                return context.Products.Where(i=>i.IsApproved&&i.IsHome).ToList();//gelen productların ısapp ve ishome alanlar true
            }
        }

        public List<Product> GetPopularProducts()
        {
            using(var context=new ShopContext())
            {
                return context.Products.ToList();
            }
        }

        public Product GetProductDetails(string url)
        {
           using(var context=new ShopContext())
               {
                   return context.Products
                   .Where(i=>i.Url==url)//id yerine url koydum ve url oalrak değiştirdim url urye eşitse aynıysa devam etsin
                   .Include(i=>i.ProductCategories)//producttan productcategoriese gittim .. inner join yapar bu ürünün 
                   .ThenInclude(i=>i.Categories)//ordanda categoriese. inclued ettiğinin kategorisine geçtim 
                   .FirstOrDefault();//kayıt varsa iligli idye uyan product varsa bunu getir ve getirirkende ekstra join işlemleri yapmış oluyorum.şimdi bunları service katmanındada kullanmam lazım          
               }
        }

          public Product GetProductpageDetails(int id)
           {//left join uygulayacağız.Ürünü alıcam ve ona ait kategoriyide alıcam.
               using(var context=new ShopContext())
               {
                   return context.Products
                   .Where(i=>i.ProductId==id)//product entitysinin tüm bilgilerini alıyorum bunun yanında productcategoriese geçmek istiyorum onun üzerindende ilgili category bilgisine geçmek istiyorum
                   .Include(i=>i.ProductCategories)//producttan productcategoriese gittim .. inner join yapar bu ürünün 
                   .ThenInclude(i=>i.Categories)//ordanda categoriese. inclued ettiğinin kategorisine geçtim 
                   .FirstOrDefault();//kayıt varsa iligli idye uyan product varsa bunu getir ve getirirkende ekstra join işlemleri yapmış oluyorum.şimdi bunları service katmanındada kullanmam lazım          
               }
           } //product categoiry tabosunuda getirdikten sonra categoriyi getir 

        public List<Product> GetProductsByCategory(string name,int page,int pageSize)
        {
            using(var context=new ShopContext())
            {
                var product= context
                .Products
                .Where(i=>i.IsApproved==true)//eğer ürün onaylıysa
                .AsQueryable();//asquarible biz sorguyu yazıyoruz ama vtye göndermeden önce üzerine ekstra link kriter belirlemek istiyorum demek
                if (!string.IsNullOrEmpty(name))
                {
                    product=product//ürün bilgilerinin
                    .Include(i=>i.ProductCategories)//productcategorislerini
                    .ThenInclude(i=>i.Categories)//sonra kategorilerini yüklüyoruz.Daha sonra şart ekleyeceğiz şart en son çünkü ilgili kayıtların referanslarına ulaşmam lazım
                    .Where(i=>i.ProductCategories.Any(a=>a.Categories.Url==name));//ilgili kaydın productcategorislerine gidiyoruz kategorilerine geçiyoruz ve gönderdiğimiz kategoriye ait bir ürün varsa any bana true döndürür oda o ürünü bana getir demek 
                }
                return product.Skip((page-1)*pageSize).Take(pageSize).ToList();
                //skip(5) yazarsan 5 ürünü öteler demek. take(5) ilede o beş ürünü alır. 
                //yani şu işlemde ilk beş ürünü değilde 2. 5 ürünü almış oluruz.skip10 desin 10 ürünü öteler 5 alır.
                //pagesize ile kaç ürün alacağımızı göndeririz.
                //page iledede kaç ürün öteleyeceğimizi yazacağız.
  //(page-1)*pageSize) neden bunu yaptık: örneğin kullanıcı herhangi sayfa göndermedi bize varsayılan değeri 1 gelir 1-1 = 0 ve sıfır olduğu için pageize önemi yok skip metodu ötelenmez ve takteki pagesizeda 3 varsa 3 ürün alır.           
 // 2 gelmişse mesala 2-1 1*3 =3 ürünü ötele 3 ürünü al          
            }
        }

        public List<Product> GetSearchResult(string search)
        {
            using(var context=new ShopContext())
            {
                var product= context
                    .Products
                    .Where(i=>i.IsApproved==true && i.Name.Contains(search)||(i.Description.Contains(search)))
                    .AsQueryable();
                return product.ToList();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using(var context=new ShopContext())
            {
                var product= context
                    .Products
                    .Include(i=>i.ProductCategories)
                    .FirstOrDefault(i=>i.ProductId==entity.ProductId);
                //bize product gelecek veiçerisinde productcategories bilgisi gelecek
                if(product!=null)
                {
                    product.Name=entity.Name;
                    product.Author=entity.Author;
                    product.Url=entity.Url;
                    product.Pages=entity.Pages;
                    product.Description=entity.Description;
                    product.ImageUrl=entity.ImageUrl;
                    product.IsApproved=entity.IsApproved;
                    product.IsHome=entity.IsHome;
                    
                    product.ProductCategories=categoryIds.Select(catid=>new ProductCategory()//her bir selectin zaten idsi var catid olacak catid göre product bilisi oluşturucam
                    {
                        ProductId=entity.ProductId,
                        CategoryId=catid
                    }).ToList();
                    context.SaveChanges();
                    
                }    
            }
        }
    } 
}
