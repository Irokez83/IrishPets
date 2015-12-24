using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace IrishPets.Models
{
    public abstract class EF_BaseRepository<TEnt, TPk> : IRepositoryEx<TEnt, TPk> where TEnt : class
    {
        public IrishPetsDb Db { get; set; } = IrishPetsDb.Create();
        public DbSet<TEnt> m_DbSet { get; set; }

        public async Task<IEnumerable<TEnt>> GetAll() => await m_DbSet.ToListAsync();
        public async Task<TEnt> GetById(TPk id) => (TEnt) await m_DbSet.FindAsync(id);
        
        public async Task<int> Update(TEnt _item, EntityState _state = EntityState.Added)
        {
            this.Db.Entry(_item).State = _state;

            return await this.SaveChangesAsync();
        }

        public async Task Remove(TPk _id)
        {
            TEnt __item = await this.GetById(_id);

            m_DbSet.Remove(__item);

            await this.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync() => await this.Db.SaveChangesAsync();
    } // --- EF_BaseRepository : class ---
}