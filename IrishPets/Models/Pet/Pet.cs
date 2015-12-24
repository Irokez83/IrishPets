using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;

namespace IrishPets.Models
{
    /// <summary> Pet </summary>
    public class Pet : ModelBaseId_Ex
    {
        public Pet() : base() { }
        public Pet(Member _member) : base() { this.Member = _member; }

        /// <summary> Ad details </summary>
        [DataType(DataType.MultilineText), StringLength(1024, ErrorMessage = " Error: field [Note] has more 1024 chars")]
        public string Note { get; set; }

        /// <summary>  Date of birth  </summary>
        [Required(ErrorMessage = "Please state Date of Birth"), Display(Name = " Date of Birth "), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateOfBirth { get; set; }

        public virtual Gender? Gender { get; set; }

        #region Breed

        [Required(ErrorMessage = "Please select a Breed")]
        public int BreedId { get; set; }

        /// <summary> Dog or Cat </summary>
        [ForeignKey("BreedId"), InverseProperty("Pets")]
        public virtual PetBreed Breed { get; set; }

        #endregion Breed

        /// <summary> Pet colour </summary>
        [Display(Name= " Coat colour: "), StringLength(64, ErrorMessage = " Error: field [CoatColour] has more 64 chars")]
        public string CoatColour { get; set; }

        /// <summary> Weight </summary>
        [StringLength(25, ErrorMessage = " Error: field [Weight] has more 25 chars")]
        public string Weight { get; set; }

        /// <summary> Additional info for cats and dogs </summary>
        public Pet_ExProperty ExProperty { get; set; }

        /// <summary> Advertisement notes </summary>
        public virtual ICollection<PetAdvert> Adverts { get; set; }

        /// <summary> Comments </summary>
        public virtual ICollection<PetReview> Reviews { get; set; }

        /// <summary> Main picture </summary>
        public string PicAvatar { get; set; }
        public virtual ICollection<PetImage> Images { get; set; }
 
        public PetImage Image { get { return null == this.Images ? null : this.Images.FirstOrDefault(zzz => zzz.IsAvatar); } }
               
        #region Member

        [StringLength(50, ErrorMessage = " Error: field [MemberId] has more 50 chars")]
        public string MemberId { get; set; }

        [ForeignKey("MemberId"), InverseProperty("Pets")]
        public virtual Member Member { get; set; }

        #endregion Member

        public string StringForSearch => $"{Id}{Name}{Note}{Breed?.Name}{Member?.UserName}{Member?.FirstName}{Member?.Surname}{Member?.Address}"; // 

        public bool IsOwner(IPrincipal _user, bool _enableAdmin = true)
            => (_user.IsInRole("Admin") && _enableAdmin) || _user.Identity.Name == this.Member.UserName;

        public static Pet Dummy_NewItem(int _id = 1, Member _member = null)
        {
            var __item = new Pet
            {
                Id = _id,
                Name = $"Test Data of {typeof(Pet).Name} -=<{_id}>=-  It'll need to remove",
                Note = $"Test Data of {typeof(Pet).Name} -=<{_id}>=-  It'll need to remove",
                Breed = PetBreed.Dummy_NewItem(_id, PetKind.Dummy_NewItem(_id)),
                BreedId = _id,
                MemberId = "466f6c38-0de8-4877-9f25-b6738f205e36",
                DateUpdated = DateTime.Now,
                DateOfBirth = DateTime.Now.AddYears(-1),
                CoatColour = "White/Grey",
                Weight = "1,5 kg",
                Gender = Models.Gender.Male,
                Member = _member,
                Enabled = true
            };

            if (null != _member)
            {
                __item.MemberId = _member.Id;
                _member.Pets.Add(__item);
            }

            return __item;
        }
        public override string ToString() => $"{Id}, {Name}, {Note}";
    }
}