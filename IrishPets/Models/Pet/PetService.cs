using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace IrishPets.Models
{
    /// <summary> Pet clinics, shelters </summary>
    public class PetService : ModelBaseId_Ex
    {
        public PetService() : base() { }

        public string Note { get; set; }

        [Phone]
        public virtual string PhoneNumber { get; set; }

        [EmailAddress]
        public virtual string Email { get; set; }
        
        [StringLength(10, ErrorMessage = "[PetService.Postcode] exceeds 10 characters")]
        public string Postcode { get; set; }

        [StringLength(25, ErrorMessage = "[PetService.HouseNameNo] exceeds 25 characters")]
        public string HouseNameNo { get; set; }

        [StringLength(50, ErrorMessage = "[PetService.Street] exceeds 50 characters")]
        public string Street { get; set; }

        [StringLength(25, ErrorMessage = "[PetService.Town] exceeds 25 characters")]
        public string Town { get; set; }

        #region County

        public int? CountyId { get; set; }

        [ForeignKey("CountyId"), InverseProperty("PetServices")]
        public virtual County County { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> Counties { get; set; }

        #endregion County

        string AddressConCat(string _ddd1, string _ddd2)
            => !string.IsNullOrEmpty(_ddd1) && !string.IsNullOrEmpty(_ddd2)
                ? $"{_ddd1}, {_ddd2}" 
                : string.IsNullOrEmpty(_ddd1) ? _ddd2 : _ddd1
                ;
        
        public string Address
        {
            get
            {
                var __adr = string.Empty;

                __adr = this.AddressConCat(__adr, this.HouseNameNo);
                __adr = this.AddressConCat(__adr, this.Street);
                __adr = this.AddressConCat(__adr, this.Town);
                __adr = this.AddressConCat(__adr, this.Postcode);
                __adr = this.AddressConCat(__adr, this.County?.Name);

                return __adr;
            }
        }

        public static PetService Dummy_NewItem(int _id = 1) => new PetService
        {
            Id = _id,
            Name = $"Test Data of {typeof(PetService).Name} -=<{_id}>=- It has to be removed",
            Note = $"Test Data of {typeof(PetService).Name} -=<{_id}>=- It has to be removed",

            Email = "Tester@IrishPet.ie",
            PhoneNumber = $"+353 ({_id}) 499 4780",
            HouseNameNo = _id.ToString(),
            Postcode = $"D{_id}",
            Street = "Test Street",
            Town = "Dummy Town",
            CountyId = 9,
            DateUpdated = DateTime.Now,

            Enabled = true
        };

        public string StringForSearch => $"{Id}{Name}{Note}{PhoneNumber}{Email}{this.Address}";

        public override string ToString() => $"{Id}, {Name}, {PhoneNumber}";
    }
}