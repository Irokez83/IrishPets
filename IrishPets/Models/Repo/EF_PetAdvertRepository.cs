namespace IrishPets.Models
{
    public class EF_PetAdvertRepository : EF_BaseWPetsRepository<PetAdvert, int>
    {
        public EF_PetAdvertRepository() : base()
        {
            Db_Set = this.Db.PetAdverts;
        }
    } // --- EF_PetAdvertRepository : class ---
}
