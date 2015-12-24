using System.Collections.Generic;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public class MemberViewModel
    {
        public ICollection<Member> Members { get; set; }
        
        public IEnumerable<SelectListItem> Counties { get; set; }
        public int CountyId { get; set; }

        public MemberSort Sort { get; set; }
    }
}