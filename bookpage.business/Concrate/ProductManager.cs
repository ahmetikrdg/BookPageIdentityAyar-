using System.Collections.Generic;
using bookpage.business.Abstract;
using bookpage.data.Abstract;
using bookpage.entity;

namespace bookpage.business.Concrate
{
    public class ProductManager : IProductServices
    {
        private IProductRepository _productRepository;
        public ProductManager(IProductRepository productrepository)
        {
            _productRepository=productrepository;
        }
        public void Create(Product Entity)
        {
            _productRepository.Create(Entity);
        }

        public void Delete(Product Entity)
        {
            _productRepository.Delete(Entity);
        }

        public List<Product> GetAll()
        {
           return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
          return _productRepository.GetById(id);

        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
          return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetProductDetails(string Url)
        {
            return _productRepository.GetProductDetails(Url);
        }

         public Product GetProductpageDetails(int id)
         {
            return _productRepository.GetProductpageDetails(id); //şimdi burdaki metoduda webui'da kullanıcaz
         }

        public List<Product> GetProductsByCategory(string name,int page,int pageSize)//burada sayı veremezsin controllerden gelir
        {
            return _productRepository.GetProductsByCategory(name,page,pageSize);
        }

        public List<Product> GetSearchResult(string search)
        {
            return _productRepository.GetSearchResult(search);
        }

        public void Update(Product Entity)
        {
            _productRepository.Update(Entity);
        }

        public void Update(Product entity, int[] categoryIds)
        {
            _productRepository.Update(entity,categoryIds);
        }
    }
}