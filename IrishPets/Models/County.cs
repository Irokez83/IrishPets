using System.Collections.Generic;

namespace IrishPets.Models
{
    public class County : ModelBaseId
    {
        public County() : base() { }

        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        //[Required, StringLength(25, ErrorMessage = "Error!! field [Name] has more 25 chars")]
        //public string Name { get; set; }

        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<PetService> PetServices { get; set; }

        //public override string ToString() => $"{Id} {Name}";

        public static County Dummy_NewItem(int _id = 1) => new County
        {
            Id = _id,
            Name = $"Test Data of {typeof(County).Name} -=<{_id}>=- It has to be removed",
        };
    }
}