using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IrishPets.Models
{
    /// <summary> Account </summary>
    public class Member : IdentityUser
    {
        public Member() : base() { this.DateOfBirth = DateTime.Now; }

        [Display(Name = " Date of Birth: "), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        public virtual Gender Gender { get; set; }

        //Account description (note for admin)
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        [Display(Name = "First Name"), StringLength(25, ErrorMessage = "[Member.FirstName] exceeds 25 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Surname"), StringLength(25, ErrorMessage = "[Member.Surname] exceeds 25 characters")]
        public string Surname { get; set; }

        [StringLength(25, ErrorMessage = "[Member.HouseNameNo] exceeds 25 characters")]
        public string HouseNameNo { get; set; }

        [StringLength(25, ErrorMessage = "[Member.Street] exceeds 25 characters")]
        public string Street { get; set; }

        [StringLength(25, ErrorMessage = "[Member.Town] exceeds 25 characters")]
        public string Town { get; set; }

        [StringLength(10, ErrorMessage = "[Member.Postcode] exceeds 10 characters")]
        public string Postcode { get; set; }

        /// <summary> Comma adding to separate fields </summary>
        string AddressConCat(string _ddd1, string _ddd2)
            => !string.IsNullOrEmpty(_ddd1) && !string.IsNullOrEmpty(_ddd2)
                ? $"{_ddd1}, {_ddd2}" : string.IsNullOrEmpty(_ddd1) ? _ddd2 : _ddd1;

        public string Address
        {
            get
            {
                var __adr = string.Empty;
                __adr = this.AddressConCat(__adr, this.HouseNameNo);
                __adr = this.AddressConCat(__adr, this.Street);
                __adr = this.AddressConCat(__adr, this.Town);

                __adr = this.AddressConCat(__adr, this.Postcode);
                if (null != this.County)
                {
                    __adr = this.AddressConCat(__adr, this.County?.Name);
                }
                return __adr;
            }
        }

        #region County

        public int? CountyId { get; set; }

        [ForeignKey("CountyId"), InverseProperty("Members")]
        public virtual County County { get; set; }

        #endregion County

        /// <summary> User pets </summary>
        public virtual ICollection<Pet> Pets { get; set; }

        /// <summary> Reviews </summary>
        public virtual ICollection<MemberReview> MemberReviews { get; set; }

        /// <summary> Pet reviews </summary>
        public virtual ICollection<PetReview> PetReviews { get; set; }

        /// <summary> User avatar </summary>
        public byte[] PicAvatar { get; set; }

        /// <summary> Date of last system's entry </summary>
        [Display(Name ="Date of last login"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateOfLastLogin { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Member> _manager)
        {
            var __userIdentity = await _manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Claim objects help to keep additional user info without accessing DB
            __userIdentity.AddClaim(new Claim("DateOfLastLogon", this.DateOfLastLogin.ToString()));
            __userIdentity.AddClaim(new Claim("DateOfBirth", this.DateOfBirth.ToString()));
            __userIdentity.AddClaim(new Claim("Gender", this.Gender.ToString()));

            return __userIdentity;
        }

        public override string ToString() => $"{Id}, {UserName}, {Gender}";

        /// <summary> Check if phone number or e-mail is supplied </summary>
        public bool IsValidContactInfo => string.IsNullOrEmpty(this.Email) && string.IsNullOrEmpty(this.PhoneNumber);

        public static Member Dummy_NewItem(string _id = "466f6c38-0de8-4877-9f25-b6738f205e36")
        {
            var __item = new Member
            {
                Id = _id,
                FirstName = "Colm",
                Surname = "Garvey",
                Note = $"Test Data of {typeof(Member).Name} -=<{_id}>=-  It has to be removed",
                Gender = Gender.Male,
                DateOfBirth = DateTime.Now.AddYears(-30),
                DateOfLastLogin = DateTime.Now,
                UserName = "Admin001@IrishPets.ie",
                Email = "Admin001@IrishPets.ie",
                EmailConfirmed = true,
                PhoneNumber = "+353 (4) 5553322",
                Postcode = "D1 D32",
                HouseNameNo = "45",
                Town = "Dublin",
                CountyId = 9,
                Street = "Tara street",
                Pets = new List<Pet>()
            };

            __item.Pets.Add(Pet.Dummy_NewItem(DateTime.Now.Millisecond, __item));
            __item.Pets.Add(Pet.Dummy_NewItem(DateTime.Now.Millisecond, __item));
            __item.Pets.Add(Pet.Dummy_NewItem(DateTime.Now.Millisecond, __item));

            return __item;
        }
    }
}