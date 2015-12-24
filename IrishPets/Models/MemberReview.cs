namespace IrishPets.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MemberReview : ModelBaseId_Ex
    {
        public MemberReview() : base()
        {
            //this.Type = ReviewType.Unknow;
        }

        [DataType(DataType.MultilineText)]
        public string Note { get; set; }
        //public ReviewType Type { get; set; }

        #region Member

        [StringLength(50, ErrorMessage = "Error: field [MemberId] has more 50 chars")]
        public string MemberId { get; set; }

        [ForeignKey("MemberId"), InverseProperty("MemberReviews")]
        public virtual Member Member { get; set; }

        #endregion Member
    }
}
