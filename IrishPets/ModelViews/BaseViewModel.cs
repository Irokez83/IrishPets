using System;
using System.Data.Entity;

namespace IrishPets.Models
{
    public class BaseViewModel
    {
        public virtual DateTime DateShowStart { get; set; }
        public virtual DateTime DateShowEnd { get; set; }
        public bool Enabled { get; set; } = true;
        public EntityState State { get; set; } = EntityState.Unchanged;
        public string ReturnUrl { get; set; } = null;
    }
}