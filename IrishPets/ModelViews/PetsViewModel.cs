using System.Collections.Generic;

namespace IrishPets.Models
{
    public class PetsViewModel
    {
        public int PetId { get; set; }
        public Pet Selected { get; set; }
        public ICollection<Pet> Pets { get; set; }
    }
}