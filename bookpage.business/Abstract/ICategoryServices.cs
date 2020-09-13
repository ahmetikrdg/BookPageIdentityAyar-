using System.Collections.Generic;
using bookpage.entity;

namespace bookpage.business.Abstract
{
    public interface ICategoryServices
    {
        Category GetById(int id);
        List<Category> GetAll();
        void Create(Category Entity);
        void Update(Category Entity);
        void Delete(Category Entity);
        Category GetByIdWithProducts(int CategoryId);
        void DeleteFromCategory(int productId,int categoryId);


    }
}