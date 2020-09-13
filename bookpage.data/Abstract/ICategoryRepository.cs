using bookpage.entity;

namespace bookpage.data.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {
         Category GetByIdWithProducts(int CategoryId);
         void DeleteFromCategory(int productId,int categoryId);
    }
}