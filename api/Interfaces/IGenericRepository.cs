using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> getAll();
        Task<T> getById(long id);
        Task<bool> Add(T entity);
        Task<bool> Delete(long id);
        Task<bool> Update(T entity, int id);

    }
}