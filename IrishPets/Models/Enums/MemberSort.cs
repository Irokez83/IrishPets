using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models
{
    public enum MemberSort : byte
    {
        [Display(Name = "Leave as is")]
        Unsort = 0,
        [Display(Name = "Sort by Username (Alphabetically)")]
        Username = 1,
        [Display(Name = "Sort by Owner's Email (Alphabetically)")]
        Email = 2,
        [Display(Name = "Sort by Date of Last Login (Newest to Oldest)")]
        DateOfLastLogin = 3,
        [Display(Name = "Sort by Date of Birth (Newest to Oldest)")]
        DateOfBirth = 4,
        [Display(Name = "Sort by Email confirmed (Newest to Oldest)")]
        EmailConfirmed = 5,
        County = 6
    }
}