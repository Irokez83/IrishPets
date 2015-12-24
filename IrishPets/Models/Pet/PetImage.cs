using System.ComponentModel.DataAnnotations.Schema;

namespace IrishPets.Models
{
    public class PetImage : ModelBaseId
    {
        public PetImage() : base()
        {
            this.IsAvatar = true;
        }

        public byte[] Image { get; set; }
        public bool IsAvatar { get; set; }
        
        public string ContentType { get; set; }

        public int ContentLength { get; set; }

        #region Pet

        public int PetId { get; set; }

        [ForeignKey("PetId"), InverseProperty("Images")]
        public virtual Pet Pet { get; set; }

        #endregion Pet

        [NotMapped]
        public string ReturnUrl { get; set; }
    }
}