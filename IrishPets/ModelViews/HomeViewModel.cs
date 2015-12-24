using System.Collections.Generic;

namespace IrishPets.Models
{
    public class HomeViewModel
    {
        public ICollection<AdvAda> AdvAdas { get; set; }
        public ICollection<PetAdvert> AdvPets { get; set; }
    }
}