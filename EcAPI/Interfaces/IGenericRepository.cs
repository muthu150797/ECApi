using System.Collections.Generic;
using System.Threading.Tasks;
//using API.Specifications;
using EcAPI.Entities;
using EcAPI.Entity;

namespace EcAPI.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
		Task<T> GetEntityWithSpec(ISpecification<T> spec);
		Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
		Task<int> CountAsync(ISpecification<T> spec);
		void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}