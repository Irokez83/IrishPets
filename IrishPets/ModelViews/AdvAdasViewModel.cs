using System.Collections.Generic;

namespace IrishPets.Models
{
    public class AdvAdasViewModel
    {
        public AdvAdasViewModel() { }

        public AdvAdasViewModel(ICollection<AdvAda> _items)
        { 
            this.Items = _items;
        }

        public ICollection<AdvAda> Items { get; set; }
    }
}