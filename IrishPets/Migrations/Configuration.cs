namespace IrishPets.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<IrishPets.Models.IrishPetsDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Models.IrishPetsDb _context)
        {
            //ConfigurationEx.CreateNew_AdvAda(_context);
            //ConfigurationEx.CreateNewCounties(_context);
            //ConfigurationEx.CreateNew_PetService(_context);
            //ConfigurationEx.CreateNewUsersAndRole(_context);
            //ConfigurationEx.CreateNewPetKind(_context);
            //ConfigurationEx.CreateNewPetAdvert(_context);
        }
    }
}
