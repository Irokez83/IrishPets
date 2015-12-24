using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace IrishPets.Models
{
    public class AdvAdaViewModel : BaseViewModel
    {
        public AdvAdaViewModel() : base()
        {
            this.Init();
        }

        public AdvAdaViewModel(AdvAda _advAda = null) : base()
        {
            this.Init(_advAda);
        }

        public void Init(AdvAda _advAda = null)
        {
            if (null == _advAda)
            {   // New item
                this.Item = new AdvAda();
                this.State = EntityState.Added;
            }
            else
            {   // Edit item
                this.Item = _advAda;
                this.State = EntityState.Modified;
            }

            this.Item.DateUpdated = DateTimeOffset.Now;
        }

        public string Title { get; set; }

        public int Id { get { return this.Item.Id; } set { this.Item.Id = value; } }

        [Required, DataType(DataType.MultilineText)]
        public string Note { get { return this.Item.Note; } set { this.Item.Note = value; } }

        [Required(ErrorMessage = "Please state Date and time of ad post time"), Display(Name = " News posted: "), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public override DateTime DateShowStart { get { return this.Item.DateShowStart; } set { this.Item.DateShowStart = value; } }

        [Required(ErrorMessage = "Please state Date and time of ad expiry"), Display(Name = " Ends: "), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public override DateTime DateShowEnd { get { return this.Item.DateShowEnd; } set { this.Item.DateShowEnd = value; } }

        public AdvAda Item { get; set; }
    }
}