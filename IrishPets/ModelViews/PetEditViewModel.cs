using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public class PetEditViewModel
    {
        public PetEditViewModel()
        {
            this.InitBreeds();
        }

        public PetEditViewModel(Pet _pet, PetKind _kind = null, PetBreed _breed = null, string _returnUrl = null)
        {
            this.Pet = _pet;
            this.Breed = _breed ?? _pet.Breed;
            this.Kind = _kind ?? this.Breed?.Kind;
            if (null != this.Breed)
            {
                this.BreedId = this.Breed.Id;
            }

            this.ReturnUrl = _returnUrl;

            this.InitBreeds();
            this.InitKind();
        }

        void InitBreeds() => this.Breeds = new List<SelectListItem>();

        public void InitKind()
        {
            if (null != this.Kind && 0 < this.Kind.Breeds.Count)
            {
                ((List<SelectListItem>)this.Breeds).Clear();

                this.Kind.Breeds
                    .ToList()
                    .ForEach(zzz => { ((List<SelectListItem>)this.Breeds).Add(new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() }); });
            }
        }

        public void SetPetKind(IEnumerable<SelectListItem> _kinds, PetKind _kind = null)
        {
            this.Kinds = _kinds;

            if (null == this.Kind.Breeds)
            {
                if (null != _kind)
                {
                    this.Kind = _kind;
                }

                this.InitKind();
            }
        }
        
        public string Title { get; set; }
        public EntityState State { get; set; }
        public AdvertType AdvertType { get; set; }

        [Required]
        public Pet Pet { get; set; }

        [Required]
        public PetKind Kind { get; set; }
        public IEnumerable<SelectListItem> Kinds { get; set; }

        [Required]
        public PetBreed Breed { get; set; }
        public int BreedId { get; set; }
        public IEnumerable<SelectListItem> Breeds { get; set; }

        public string ReturnUrl { get; set; }
    }
}