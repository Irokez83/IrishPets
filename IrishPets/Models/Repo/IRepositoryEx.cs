using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace IrishPets.Models
{
    public interface IRepositoryEx<TEntity, TPk> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(TPk id);
        Task<int> Update(TEntity _item, EntityState _state = EntityState.Added);
        Task Remove(TPk id);
        Task<int> SaveChangesAsync();
    } // --- IRepositoryEx : interface ---
}