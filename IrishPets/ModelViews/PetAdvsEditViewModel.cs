using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public class PetAdvsEditViewModel
    {
        public PetAdvsEditViewModel() { }

        public PetAdvsEditViewModel(Member _member = null,
            Pet _pet = null,
            PetAdvert _advert = null,
            string _returnUrl = null,
            AdvertType _advertType = AdvertType.Notification_Advert)
        {
            this.Init(_member, _pet, _advert, _returnUrl, _advertType);
        }

        public void Init(Member _member = null,
            Pet _pet = null,
            PetAdvert _advert = null,
            string _returnUrl = null,
            AdvertType _advertType = AdvertType.Notification_Advert,
            EntityState _state = EntityState.Added)
        {
            this.AdvertType = _advertType;
            this.State = _state;
            this.Title = this.State == EntityState.Added ? "New " : "Edit ";
            this.ReturnUrl = _returnUrl;

            this.Member = _member;
            this.Pet = _pet;
            this.ChangeInfoViewModel = new ContactInfoViewModel2(this.Member);

            if (null == this.Pet && null != _member.Pets && 0 < _member.Pets.Count)
                this.Pet = _member.Pets.FirstOrDefault();

            if (null != this.Pet)
                this.PetId = this.Pet.Id;

            this.Advert = null == _advert ? new PetAdvert(this.Pet, _advertType) : _advert;

            _member.Pets.ToList()
                .ForEach(zzz =>
                {
                    this.Pets.Add(new SelectListItem { Text = $"({zzz.Breed.Kind.Name}) {zzz.Name}", Value = zzz.Id.ToString() });
                });
        }

        public void InitFromModel()
        {
            this.Init(this.Pet.Member, this.Pet, this.Advert, this.ReturnUrl, this.AdvertType, this.State);
        }

        public string Title { get; set; }

        public AdvertType AdvertType { get; set; }
        public EntityState State { get; set; }

        public ContactInfoViewModel2 ChangeInfoViewModel { get; set; }
        public Member Member { get; set; }
        public Pet Pet { get; set; }

        #region Pet ComboBox

        public int PetId { get; set; }

        public List<SelectListItem> Pets { get; set; } = new List<SelectListItem>();

        #endregion Pet ComboBox

        public PetAdvert Advert { get; set; }

        public string ReturnUrl { get; set; }
    }
}