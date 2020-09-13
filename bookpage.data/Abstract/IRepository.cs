using System.Collections.Generic;

namespace bookpage.data.Abstract
{
    public interface IRepository<T>
    {
        T GetById(int id);
        List<T> GetAll();
        void Create(T Entity);
        void Update(T Entity);
        void Delete(T Entity);
         
    }
}