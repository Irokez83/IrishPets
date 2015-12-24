using System.Collections.Generic;

namespace IrishPets.Models
{
    /// <summary> Pet kind </summary>
    public class PetKind: ModelBaseId
    {
        /// <summary> Breeds </summary>
        public virtual ICollection<PetBreed> Breeds { get; set; }

        public static PetKind Dummy_NewItem(int _id = 1)
        {
            var __kind = new PetKind
            {
                Id = _id,
                Name = $"Test Data of {typeof(PetKind).Name} -=<{_id}>=-  It has to be removed",
                Breeds = new List<PetBreed>()
            };

            __kind.Breeds.Add(PetBreed.Dummy_NewItem(_id*100  , __kind));
            __kind.Breeds.Add(PetBreed.Dummy_NewItem(_id*100+1, __kind));
            __kind.Breeds.Add(PetBreed.Dummy_NewItem(_id*100+2, __kind));
            __kind.Breeds.Add(PetBreed.Dummy_NewItem(_id*100+3, __kind));

            return __kind;
        }
    }
}