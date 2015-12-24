using System.Collections.Generic;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public class PetAdvsViewModel
    {
        public AdvertType AdvertType { get; set; }

        public ICollection<PetAdvert> PetAdverts { get; set; }

        public IEnumerable<SelectListItem> Kinds { get; set; }
        public int KindId { get; set; }

        public IEnumerable<SelectListItem> Counties { get; set; }
        public int CountyId { get; set; }
        public PetSort Sort { get; set; }
    }
}