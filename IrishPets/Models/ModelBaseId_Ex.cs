using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IrishPets.Models
{
    public abstract class ModelBaseId_Ex : ModelBase
    {
        public ModelBaseId_Ex() : base() { }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please fill in the Title"), StringLength(64, ErrorMessage = "Error!! field [Title] has more than 64 chars")]
        public string Name { get; set; }

        public override string ToString() => $"{Id} {Name}";
    }
}