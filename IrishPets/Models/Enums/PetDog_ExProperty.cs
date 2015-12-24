using System;
using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models
{
    [Flags]
    public enum PetDog_ExProperty : long
    {
        Unknow = 0,

        [Display(Name = "4 weeks free Petplan insurance")]
        WeeksFreePetPlanInsurance = 1 << 1,

        [Display(Name = "4 weeks free other insurance")]
        WeeksFreeOtherInsurance = 1 << 2,

        [Display(Name = "Microchipped")]
        Microchipped = 1 << 3,

        [Display(Name = "Wormed")]
        Wormed = 1 << 4,

        [Display(Name = "Vaccinated")]
        Vaccinated = 1 << 5,

        [Display(Name = "Kennel Club registered")]
        KennelClubRegistered = 1 << 6,

        [Display(Name = "Vet checked")]
        VetChecked = 1 << 7,

        [Display(Name = "Health tests")]
        HealthTests = 1 << 8,

        [Display(Name = "DNA tested")]
        DNAtested = 1 << 9,

        [Display(Name = "Blood tested")]
        BloodTested = 1 << 10,

        [Display(Name = "Pedigree Certificate")]
        PedigreeCertificate = 1 << 11,

        [Display(Name = "Born on the premises")]
        BornOnThePremises = 1 << 12,

        [Display(Name = "Sold from the UK")]
        SoldFromTheUK = 1 << 13,

        [Display(Name = "Mum can be seen")]
        MumCanBeSeen = 1 << 14,

        [Display(Name = "Dad can be seen")]
        DadCanBeSeen = 1 << 15,

        [Display(Name = "Crate")]
        Crate = 1 << 16,

        [Display(Name = "Scent blanket")]
        ScentBlanket = 1 << 17,

        [Display(Name = "Feeding instructions")]
        FeedingInstructions = 1 << 18,

        [Display(Name = "Care information")]
        CareInformation = 1 << 19
    }
}