using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace IrishPets.Models
{

    public class PetReview : ModelBase
    {
        string m_Note;

        public PetReview() : base()
        {
            this.Type = ReviewType.Unknow;
        }

        public PetReview(int _petId, string _memberId, string _note) : base()
        {
            this.Type = ReviewType.Unknow;
            this.PetId = _petId;
            this.MemberId = _memberId;
            this.Note = _note;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        public string Note
        {
            get
            {
                return m_Note;
            }
            set
            {
                m_Note = value; this.DateUpdated = DateTimeOffset.Now;
            }
        }

        public ReviewType Type { get; set; }

        #region Pet

        public int PetId { get; set; }

        [ForeignKey("PetId"), InverseProperty("Reviews")]
        public virtual Pet Pet { get; set; }

        #endregion Pet

        #region Member

        public string MemberId { get; set; }

        [ForeignKey("MemberId"), InverseProperty("PetReviews")]
        public virtual Member Member { get; set; }

        #endregion Member

        public bool IsOwner(IPrincipal _user)
        {
            return _user.Identity.GetUserId() == this.MemberId;
        }
        
        public override string ToString() => $"{Id} {Note}";
    }
}
