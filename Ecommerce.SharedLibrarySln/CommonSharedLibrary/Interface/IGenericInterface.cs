using System.Linq.Expressions;
using CommonSharedLibrary.Responses;

namespace CommonSharedLibrary.Interface
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAync(T entity);
        Task<IEnumerable<T>> GetAllAsync(T entity);
        Task<T> FindByIdAsync(int id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }  
}

