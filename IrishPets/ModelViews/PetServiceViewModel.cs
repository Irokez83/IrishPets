using System.Collections.Generic;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public class PetServiceViewModel
    {
        public PetServiceViewModel(IEnumerable<SelectListItem> _counties, ICollection<PetService> _services)
        {
            this.Counties = _counties;
            this.Services = _services;
        }

        public ICollection<PetService> Services { get; set; }
        public PetService Service { get; set; }

        public IEnumerable<SelectListItem> Counties { get; set; }
        public int CountyId { get; set; }
    }
}