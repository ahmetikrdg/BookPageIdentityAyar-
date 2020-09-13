using System.Collections.Generic;
using bookpage.entity;

namespace bookpage.data.Abstract
{
    public interface IProductRepository:IRepository<Product>
    {
        Product GetProductpageDetails(int id);
        List<Product> GetProductsByCategory(string name, int page,int pageSize);
        Product GetProductDetails(string url);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string search);
        List<Product> GetPopularProducts();
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryIds);

    }
}