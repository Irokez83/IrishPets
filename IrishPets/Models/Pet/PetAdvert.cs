using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IrishPets.Models
{
    /// <summary> Ad message </summary>
    public class PetAdvert : ModelBaseId_Ex
    {
        public PetAdvert() : base() { this.Init(); }
        public PetAdvert(Pet _pet, AdvertType _advertType = AdvertType.Notification_Advert) : base()
        {
            if(null != _pet)
            {
                this.Pet = _pet;
                this.PetId = _pet.Id;
            }

            this.Init(_advertType);
        }

        private void Init(AdvertType _advertType = AdvertType.Notification_Advert)
        {
            this.Type = _advertType;
            this.TypeOfSale = TypeOfSale.Private;
            this.DateShowStart = DateTime.Now;
            this.DateShowEnd = this.DateShowStart.AddDays(30);
            this.IsShow = true;
            this.Enabled = true;
        }

        /// <summary> Ad notes </summary>
        [Required, DataType(DataType.MultilineText)]
        public string Note { get; set; }

        public string NoteShort {
            get
            {
                int __cnt = this.Note.Length;

                if (200 < __cnt)
                {
                    return this.Note.Substring(0, 200) + " ...";
                }

                return this.Note;
            }
        }


        /// <summary> Ad type </summary>
        public virtual AdvertType Type { get; set; }

        [Display(Name=" Type of sale: ")]
        public virtual TypeOfSale TypeOfSale { get; set; }

        /// <summary> First price </summary>
        [Display(Name=" First price: ")]
        public double FirstPrice { get; set; }

        #region Ad show period

        /// <summary> Flag whether ad is shown on website </summary>
        [Display(Name = " Is show ")]
        public bool IsShow { get; set; }

        /// <summary> Date and time of ad post time  </summary>
        [Required(ErrorMessage = "Please state Date and time of ad post time"), Display(Name = " Advert posted: "), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateShowStart { get; set; }

        /// <summary> Date and time of ad expiry </summary>
        [Required(ErrorMessage = "Please state Date and time of ad expiry"), Display(Name = " Ends: "), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateShowEnd { get; set; }

        #endregion Ad show period

        #region Pet

        public int PetId { get; set; }

        [ForeignKey("PetId"), InverseProperty("Adverts")]
        public virtual Pet Pet { get; set; }

        #endregion Pet

        ///// <summary> Finance movement on this ad </summary>
        //public virtual ICollection<Transaction> Transactions { get; set; }

        public string StringForSearch => $"{Id}{Name}{Note}{PetId}{Pet?.StringForSearch}";


        public override string ToString() => $"{Id}, {Name}, {Note}";
    }
}