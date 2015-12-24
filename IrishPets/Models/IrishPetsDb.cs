namespace IrishPets.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public partial class IrishPetsDb : IdentityDbContext<Member> 
    {
        public IrishPetsDb() : base("DefaultConnection", throwIfV1Schema: false) { }

        public static IrishPetsDb Create() => new IrishPetsDb();


        #region Entities

        #region Member & Ext

        public DbSet<County> Counties { get; set; }
        public DbSet<MemberReview> MemberReviews { get; set; }
        public DbSet<PetService> PetServices { get; set; }
        // public DbSet<Tariff> Tariffs { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AdvAda> AdvAdas { get; set; }
        
        #endregion Member & Ext

        #region Pets

        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetAdvert> PetAdverts { get; set; }
        public DbSet<PetReview> PetReviews { get; set; }
        public DbSet<PetKind> PetKinds { get; set; }
        public DbSet<PetBreed> PetBreeds { get; set; }
        public DbSet<PetImage> PetImages { get; set; }

        #endregion Pets

        #endregion Entities

        public override int SaveChanges()
        {
            int __cnt = 0;

            try
            {
                __cnt = base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException _ex)
            {
                var __sb = new System.Text.StringBuilder();

                foreach (var _failure in _ex.EntityValidationErrors)
                {
                    __sb.AppendFormat("{0} failed validation\n", _failure.Entry.Entity.GetType());
                    foreach (var _error in _failure.ValidationErrors)
                    {
                        __sb.AppendLine($"- {_error.PropertyName} : {_error.ErrorMessage}");
                        __sb.AppendLine();
                    }
                }

                throw new System.Data.Entity.Validation.DbEntityValidationException($"Entity Validation Failed - errors:\n{__sb.ToString()}", _ex);

            }

            return __cnt;
        }

        public override async Task<int> SaveChangesAsync()
        {
            int __cnt = 0;

            try
            {
                __cnt = await base.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException _ex)
            {
                var __sb = new System.Text.StringBuilder();

                foreach (var _failure in _ex.EntityValidationErrors)
                {
                    __sb.AppendLine($"{_failure.Entry.Entity.GetType()} failed validation\n");
                    foreach (var _error in _failure.ValidationErrors)
                    {
                        __sb.AppendLine($"- {_error.PropertyName} : {_error.ErrorMessage}");
                        __sb.AppendLine();
                    }
                }

                throw new System.Data.Entity.Validation.DbEntityValidationException(
                    "Entity Validation Failed - errors:\n" + __sb.ToString(), _ex
                );

            }

            return __cnt;
        }

        protected override void OnModelCreating(DbModelBuilder _builder)
        {
            base.OnModelCreating(_builder);

            this.Database.Log = s => Debug.Write(s);
        } 
    }
}