using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models
{
    //Sorting
    public enum PetSort : byte
    {
        [Display(Name = "Leave as it is")]
        Unsort = 0,
        [Display(Name = "Sort by Price (Max to Min)")]
        Price = 1,
        [Display(Name = "Sort by Date Created (Newest to Oldest)")]
        DateCreate = 2,
        [Display(Name = "Sort by Breed (Alphabetically)")]
        Breed = 3,
        [Display(Name = "Sort by County (Alphabetically)")]
        County = 4,
        [Display(Name = "Sort by Owner's Email (Alphabetically)")]
        Email = 5
    }
}