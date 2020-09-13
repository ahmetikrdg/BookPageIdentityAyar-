using System.Collections.Generic;
using bookpage.entity;

namespace bookpage.business.Abstract
{
    public interface IProductServices
    {
        Product GetById(int id);
        List<Product> GetAll();
        void Create(Product Entity);
        void Update(Product Entity);
        void Delete(Product Entity);
        Product GetProductpageDetails(int id);
        List<Product> GetProductsByCategory(string name,int page,int pageSize);
        Product GetProductDetails(string url);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string search);
        int GetCountByCategory(string category);
        Product GetByIdWithCategories(int id);
        void Update(Product entity, int[] categoryIds);
    }
}