using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IrishPets.Models
{
    public abstract class ModelBaseId
    {
        public ModelBaseId() { }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(64, ErrorMessage = "Error!! field [Name] has more than 64 chars")]
        public string Name { get; set; }

        public override string ToString() => $"{Id} {Name}";
    }
}