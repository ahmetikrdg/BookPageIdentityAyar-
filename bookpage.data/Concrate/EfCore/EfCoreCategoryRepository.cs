using System.Linq;
using bookpage.data.Abstract;
using bookpage.entity;
using Microsoft.EntityFrameworkCore;

namespace bookpage.data.Concrate.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, ShopContext>, ICategoryRepository
    {
        public void DeleteFromCategory(int productId, int categoryId)
        {
            using(var context=new ShopContext())
            {
                context.Database.ExecuteSqlRaw("delete from productcategory where ProductId=@p0 and CategoryId=@p1",productId,categoryId);
                //klasik sql sorgusu yazdım sen productcategory tablosuna git productıdsi ve category ıdsi eşit olan ürünü sil ProductCategories tablosundan.
            }
        }

        public Category GetByIdWithProducts(int CategoryId)
        {
             using(var context=new ShopContext())
            {
                return context.Categories//kategoriye gittim
                .Where(i=>i.CategoryId==CategoryId)//eşleşen kaydı buldum
                .Include(i=>i.ProductCategories)//include diyerek ilgili kaydın kategori bilgisini aldım
                .ThenInclude(i=>i.products).FirstOrDefault();//ve kategoriye uyan ürünleri getirdim
            }
        }
    }
}