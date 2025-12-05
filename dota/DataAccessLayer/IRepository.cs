using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T item);
        void Delete(int id);
        void Update(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
