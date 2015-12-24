using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public abstract class EF_BaseWPetsRepository<TEnt, TPk> 
        : IRepositoryPets<TEnt, TPk> where TEnt 
        : class
    {
        public EF_BaseWPetsRepository() : base() { }

        public Member Member { get; set; }
        //[Dependency]
        public IrishPetsDb Db { get; set; } = IrishPetsDb.Create();

        public virtual async Task<int> SaveChangesAsync() => await this.Db.SaveChangesAsync();


        #region =-  TEnt  -=

        public virtual DbSet<TEnt> Db_Set { get; set; }

        public virtual async Task<IEnumerable<TEnt>> GetAll() => await this.Db_Set.ToListAsync();

        public virtual async Task<TEnt> GetById(TPk id) => (TEnt) await this.Db_Set.FindAsync(id);

        public virtual async Task<int> Update(TEnt _item, EntityState _state = EntityState.Added)
        {
            this.Db.Entry(_item).State = _state;

            return await this.SaveChangesAsync();
        }

        public virtual async Task Remove(TPk _id)
        {
            var __item = await this.GetById(_id);

            if (null == __item)
            {
                return;
            }

            this.Db_Set.Remove(__item);

            int __cnt = await this.SaveChangesAsync();
        }

        #endregion =-  TEnt  -=


        #region =-  PetKinds  -=

        public async Task<IEnumerable<SelectListItem>> GetPetKinds()
            => await this.Db.PetKinds.AsNoTracking().Select(zzz => new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() }).ToListAsync();

        public async Task<PetKind> GetPetKind_First() => await this.Db.PetKinds.FirstOrDefaultAsync();

        public async Task<PetKind> GetPetKind_ById(int id) => await this.Db.PetKinds.FindAsync(id);

        #endregion =-  PetKinds  -=


        #region =-  PetBreeds  -=

        public async Task<PetBreed> GetPetBreed_First() => await this.Db.PetBreeds.FirstOrDefaultAsync();

        public async Task<IEnumerable<PetBreed>> GetPetBreeds_ByKindId(int id)
            => await this.Db.PetBreeds.Where(zzz => zzz.KindId == id).ToListAsync();

        #endregion =-  PetBreeds  -=

        public async Task<IEnumerable<SelectListItem>> GetCounties() => new SelectList(await this.Db.Counties.ToListAsync(), "Id", "Name");
    } // --- EF_BaseWCountiesRepository : class ---
}