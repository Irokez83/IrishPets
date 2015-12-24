using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models
{
    public enum AdvertType : byte
    {
        Error = 0,

        [Display(Name = "General advertising")]
        Notification_Advert = 1,

        [Display(Name = "Breeding")]
        Notification_Breeding = 2,

        [Display(Name = "Lost and Found")]
        Notification_LostAndFound = 3
    }
}