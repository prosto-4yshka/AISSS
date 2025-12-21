using System.Collections.Generic;

using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : class, IDomainObject
    {
        // Базовые CRUD
        void Add(T item);
        void Delete(int id);
        void Update(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();

        // Пагинация
        IEnumerable<T> GetPage(int pageNumber, int pageSize);
        int GetTotalCount();
    }
}