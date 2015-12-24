using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IrishPets.Models
{
    /// <summary> Pet breed </summary>
    public class PetBreed : ModelBaseId
    {
        public PetBreed() : base() { }

        #region Pet kind

        public int KindId { get; set; }

        [ForeignKey("KindId"), InverseProperty("Breeds")]
        public virtual PetKind Kind { get; set; }

        #endregion Pet kind
        
        public virtual ICollection<Pet> Pets { get; set; }

        public static PetBreed Dummy_NewItem(int _id = 1, PetKind _kind = null)
        {
            return new PetBreed
            {
                Id = _id,
                Name = $"Test Data of {typeof(PetBreed).Name} -=<{_id}>=-  It has to be removed",
                Kind = _kind,
                KindId = _kind.Id
            };
        }
    }
}