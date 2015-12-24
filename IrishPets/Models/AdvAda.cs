using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IrishPets.Models
{
    /// <summary> News message of administration </summary>
    public class AdvAda : ModelBase
    {
        public AdvAda() : base()
        {
            this.Init();
        }

        void Init()
        {
            this.DateShowStart = DateTime.Now;
            this.DateShowEnd = this.DateShowStart.AddDays(30);
            this.Enabled = true;
            this.IsShow = true;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(300, ErrorMessage = "[AdvAda.Note] exceeds 300 characters")]
        public string Note { get; set; }

        #region News show period

        // Flag whether ad is shown on website
        [Display(Name = " Is show ")]
        public bool IsShow { get; set; }

        /// <summary> Date and time of ad post time </summary>
        [Required(ErrorMessage = "Please state Date and time of ad post time"), Display(Name = " Advert posted: "), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateShowStart { get; set; }

        /// <summary> Date and time of ad expiry </summary>
        [Required(ErrorMessage = "Please state Date and time of ad expiry"), Display(Name = " Ends: "), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateShowEnd { get; set; }

        #endregion News show period

        public static AdvAda Dummy_NewItem(int _id = 1) => new AdvAda
        {
            Id = _id,
            Note = $"Test Data of {typeof(AdvAda).Name} -=<{_id}>=-  It has to be removed.",

            DateShowStart = DateTime.Now,
            DateShowEnd = DateTime.Now.AddDays(90),
            DateUpdated = DateTime.Now,

            Enabled = false,
            IsShow = false
        };

        public override string ToString() => $"{Id}, {Note}";
    }
}