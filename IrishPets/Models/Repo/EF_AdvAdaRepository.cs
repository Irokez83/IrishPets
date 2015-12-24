namespace IrishPets.Models
{
    public class EF_AdvAdaRepository : EF_BaseRepository<AdvAda, int>
    {
        public EF_AdvAdaRepository() : base()
        {
            m_DbSet = this.Db.AdvAdas;
        }
    } // --- EF_AdvAdaRepository : class ---
}