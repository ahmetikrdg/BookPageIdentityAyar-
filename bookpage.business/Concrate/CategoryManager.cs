using System.Collections.Generic;
using bookpage.business.Abstract;
using bookpage.data.Abstract;
using bookpage.entity;

namespace bookpage.business.Concrate
{
    public class CategoryManager : ICategoryServices
    {
        private ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryrepository)
        {
            _categoryRepository=categoryrepository;
        }
        public void Create(Category Entity)
        {
            _categoryRepository.Create(Entity);
        }

        public void Delete(Category Entity)
        {
            _categoryRepository.Delete(Entity);
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _categoryRepository.DeleteFromCategory(productId,categoryId);
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Category GetByIdWithProducts(int CategoryId)
        {
            return _categoryRepository.GetByIdWithProducts(CategoryId);
        }

        public void Update(Category Entity)
        {
            _categoryRepository.Update(Entity);
        }
    }
}