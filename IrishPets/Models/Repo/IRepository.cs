using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace IrishPets.Models
{
    public interface IRepository<TEnt, in TPk> where TEnt : class
    {
        Task<int> Update(TEnt _item, EntityState _state = EntityState.Added);
        Task Remove(TPk id);
        Task<TEnt> GetById(TPk id);
        Task<IEnumerable<TEnt>> GetAll();
        Task<int> SaveChangesAsync();
    } // --- IRepository : interface ---
}