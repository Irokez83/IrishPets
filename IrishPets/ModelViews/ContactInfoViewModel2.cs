using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public class ContactInfoViewModel2
    {
        private Member m_Member;
        public ContactInfoViewModel2() : base() { }

        public ContactInfoViewModel2(Member _member) : base()
        {
            m_Member = _member;

            this.Id = _member.Id;

            this.FirstName = _member.FirstName;
            this.Surname = _member.Surname;
            this.Note = _member.Note;
            this.DateOfBirth = _member.DateOfBirth;
            this.Gender = _member.Gender;
            this.PhoneNum = _member.PhoneNumber;
            this.HouseNameNo = _member.HouseNameNo;
            this.Street = _member.Street;
            this.Town = _member.Town;
            this.County = _member.County;
            this.Postcode = _member.Postcode;
            this.CountyId = _member.CountyId;

            this.EmailConfirmed = _member.EmailConfirmed;

        }

        public string Id { get; set; }


        [Display(Name = "Date of Birth"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public virtual DateTime? DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public string Email
        {
            get { return null == m_Member ? string.Empty : m_Member.Email; }
        }

        [Display(Name = "First name")]
        public string FirstName { get; set; }
        public string Surname { get; set; }

        [Display(Name = "House Name / Number")]
        public string HouseNameNo { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNum { get; set; }

        [Display(Name = "Postcode")]
        public string Postcode { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }

        [Display(Name = "Emai Confirmed")]
        public bool EmailConfirmed { get; set; }


        #region County

        public int? CountyId { get; set; }
        public virtual County County { get; set; }
        public IEnumerable<SelectListItem> Counties { get; set; }
        
        #endregion County

        //Admin notes
        public string Note { get; set; }
        public DateTimeOffset? DateOfLastLogin { get { return m_Member.DateOfLastLogin; } }
        
        public void Update(Member _member)
        {
            _member.FirstName = this.FirstName;
            _member.Surname = this.Surname;
            _member.DateOfBirth = this.DateOfBirth;
            _member.Gender = this.Gender;
            _member.Note = this.Note;
            _member.PhoneNumber = this.PhoneNum;
            _member.HouseNameNo = this.HouseNameNo;
            _member.Street = this.Street;
            _member.Town = this.Town;
            _member.County = this.County;
            _member.CountyId = this.CountyId;
            _member.Postcode = this.Postcode;
            _member.DateOfLastLogin = _member.DateOfLastLogin;

            _member.EmailConfirmed = this.EmailConfirmed;
        }

        public Member CreateNew(string _userName = null, string _email = null)
        {
            return new Member
            {
                UserName = _userName,
                Email = _email ?? _userName,
                EmailConfirmed = this.EmailConfirmed,

                FirstName = this.FirstName,
                Surname = this.Surname,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Note = this.Note,
                PhoneNumber = this.PhoneNum,
                HouseNameNo = this.HouseNameNo,
                Street = this.Street,
                Town = this.Town,
                County = this.County,
                Postcode = this.Postcode,
                DateOfLastLogin = DateTime.Now
            };
        }
    }
}