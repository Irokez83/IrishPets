namespace IrishPets.Models
{
    public class EF_PetRepository : EF_BaseWPetsRepository<Pet, int>
    {
        public EF_PetRepository() : base()
        {
            Db_Set = this.Db.Pets;
        }
    } // --- EF_PetRepository : class ---
}
