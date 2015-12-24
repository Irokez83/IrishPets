using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models
{
    /// <summary> Sorting </summary>
    public enum AdvAdaSort : byte
    {
        [Display(Name = "Leave as it is")]
        Unsort = 0,
        [Display(Name = "Sort by Date Created (Newest to Oldest)")]
        DateCreate = 2
    }
}