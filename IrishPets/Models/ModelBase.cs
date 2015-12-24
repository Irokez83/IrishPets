using System;
using System.ComponentModel.DataAnnotations;

namespace IrishPets.Models
{
    public abstract class ModelBase
    {
        public ModelBase()
        {
            this.Enabled = true;
        }

        /// <summary> Date and time of creation </summary>
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTimeOffset? DateCreated { get; private set; } = DateTimeOffset.Now;

        /// <summary> Date and time of editing </summary>
        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTimeOffset? DateUpdated { get; set; } = DateTimeOffset.Now;

        /// <summary> Option to block </summary>
        [Display(Name = " Enable this entity ")]
        public bool Enabled { get; set; }
    }
}