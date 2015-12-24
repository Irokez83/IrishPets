using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models.Enums
{
    public enum PetServiceSort : byte
    {
        [Display(Name = "Leave as it is")]
        Unsort = 0,
        [Display(Name = "Sort by Date Created (Newest to Oldest)")]
        DateCreate = 2,
        [Display(Name = "Sort by County (Alphabetically)")]
        County = 4
    }
}