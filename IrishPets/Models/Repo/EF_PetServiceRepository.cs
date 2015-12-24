namespace IrishPets.Models
{
    public class EF_PetServiceRepository : EF_BaseWCountiesRepository<PetService, int>
    {
        public EF_PetServiceRepository() : base()
        {
            m_DbSet = this.Db.PetServices;
        }
    } // --- EF_PetServiceRepository : class ---
}
