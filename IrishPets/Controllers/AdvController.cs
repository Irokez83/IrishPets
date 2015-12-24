using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IrishPets.Controllers
{
    using Properties;
    using Models;

    public class AdvController : BaseController
    {
        private IRepositoryPets<Pet, int> m_RepositoryPets;
        private IRepositoryPets<PetAdvert, int> m_RepositoryPetAdverts;

        public AdvController(IRepositoryPets<Pet, int> _repoPets, IRepositoryPets<PetAdvert,int> _repoAdvert) : base()
        {
            m_RepositoryPets = _repoPets;
            m_RepositoryPetAdverts = _repoAdvert;
        }
        
        #region Contact info

        [Authorize]
        public async Task<ActionResult> WizContactInfoEdit()
        {
            var __user = await this.GetMemberAsync();
            if (null == __user)
            {
                return View(c_Error);
            }

            var __model = new PetAdvsEditViewModel(__user, _returnUrl: this.ReferUrl, _advertType: AdvertType.Notification_Breeding);
            __model.ChangeInfoViewModel.Counties = new SelectList(this.Db.Counties.AsNoTracking().ToList(), "Id", "Name");

            return View(__model);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> WizContactInfoEdit(ContactInfoViewModel _model, string ReturnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                return View(_model);
            }

            var __user = await this.GetMemberAsync();

            _model.Update(__user);

            var __result = await this.UserManager.UpdateAsync(__user);

            if (__result.Succeeded)
                return Redirect(this.GetReturnUrl(ReturnUrl));

            this.AddErrors(__result);

            return View(_model);
        }

        #endregion Contact info

        #region Pet Advert

        public async Task<ActionResult> Index(PetSort? Sort = null, string q = null, int? KindId = null, int? CountyId = null, byte u = 0, AdvertType t = AdvertType.Error)
        {
            if (t == AdvertType.Error)
            {
                return HttpNotFound();
            }

            return View(await this.GetAdvsModel(t, Sort, q, KindId, CountyId, u));
        }

        async Task<PetAdvsViewModel> GetAdvsModel(AdvertType _advertType = AdvertType.Notification_Advert
            , PetSort? _sortId = null, string _searchString = null, int? _kindId = null, int? _countyId = null, byte _isUser = 0)
        {
            var __now = DateTimeOffset.Now;

            var __adverts = await m_RepositoryPetAdverts.GetAll();
            
            if (string.IsNullOrEmpty(_searchString))
                __adverts = __adverts
                        .Where(zzz => zzz.DateShowStart <= __now && __now <= zzz.DateShowEnd && zzz.Enabled && zzz.Type == _advertType);
            else
                __adverts = __adverts
                        .Where(zzz => zzz.DateShowStart <= __now && __now <= zzz.DateShowEnd && zzz.Enabled && zzz.Type == _advertType
                                && (zzz.StringForSearch.Contains(_searchString)));

            if (1 == _isUser)
            {
                var __user = await this.GetMemberAsync();
                __adverts = __adverts.Where(zzz => zzz.Pet.MemberId == __user.Id);
            }

            if (null != _kindId)
            {
                __adverts = __adverts.Where(zzz => zzz.Pet.Breed.KindId == _kindId);
            }

            if (null != _countyId)
            {
                __adverts = __adverts.Where(zzz => zzz.Pet.Member.CountyId == _countyId);
            }

            var __advertsQOrder = __adverts.OrderBy(zzz => zzz.TypeOfSale);

            switch (_sortId)
            {
                case PetSort.Price:
                    __adverts = __advertsQOrder.ThenByDescending(zzz => zzz.FirstPrice);
                    break;
                case PetSort.DateCreate:
                    __adverts = __advertsQOrder.ThenByDescending(zzz => zzz.DateCreated);
                    break;
                case PetSort.Breed:
                    __adverts = __advertsQOrder.ThenByDescending(zzz => zzz.Pet.Breed.Name);
                    break;
                case PetSort.County:
                    __adverts = __advertsQOrder.ThenByDescending(zzz => zzz.Pet.Member.County.Name);
                    break;
                case PetSort.Email:
                    __adverts = __advertsQOrder.ThenByDescending(zzz => zzz.Pet.Member.Email);
                    break;
                default:
                    __adverts = __advertsQOrder.ThenByDescending(zzz => zzz.DateUpdated);
                    break;
            }

            var __model = new PetAdvsViewModel();
            __model.AdvertType = _advertType;
            __model.Counties = await m_RepositoryPets.GetCounties(); 
            __model.Kinds = await m_RepositoryPets.GetPetKinds(); 
            __model.PetAdverts = __adverts.ToList();
            return __model;
        }


        [Authorize]
        public async Task<ActionResult> WizEdit(int? id = null, int? p = null, AdvertType t = AdvertType.Error)
        {
            if (t == AdvertType.Error)
            {
                return HttpNotFound();
            }

            var __user = await this.GetMemberAsync();

            Pet __pet = null;
            PetAdvert __advert = null;
            EntityState __entityState;

            if (null == id)
            {
                __entityState = EntityState.Added;
            }
            else
            {
                __entityState = EntityState.Modified;

                __advert = await m_RepositoryPetAdverts.GetById((int)id);

                if (null == __advert || !__advert.Pet.IsOwner(User))
                {
                    return View(c_Error);
                }

                if(User.IsInRole(Resources.DefAdmin)) // If admin changes the item, get .member from .Pet.Member
                {
                    __user = __advert.Pet.Member;
                }
            }

            if (null != p)
            {
                __pet = __user.Pets.FirstOrDefault(zzz => zzz.Id == p);
            }

            // var __model = new PetAdvsEditViewModel(__user, __advert.Pet, __advert, this.ReferUrl, __advert.Type);
            var __model = new PetAdvsEditViewModel(__user, __pet, __advert, this.ReferUrl, t);
            __model.Title = __entityState == EntityState.Added ? "New ": "Edit ";
            __model.State = __entityState;
            return View(__model);
        }

        private async Task<PetAdvsEditViewModel> Init_PetAdvsEditViewModel(PetAdvsEditViewModel _model)
        {
            if (null == _model.Pet)
            {
                _model.Pet = await m_RepositoryPets.GetById((int)_model?.Advert?.PetId);
                _model.Advert.Pet = _model.Pet;
            }

            _model.InitFromModel();
            return _model;
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> WizEdit(PetAdvsEditViewModel _model, AdvertType t = AdvertType.Error, string ReturnUrl = null)
        {
            int __errors = 0;

            if (!ModelState.IsValid)
            {
                __errors++;
                return View(await this.Init_PetAdvsEditViewModel(_model));
            }

            if (0 > _model.Advert.FirstPrice)
            {
                ModelState.AddModelError("FirstPrice", "The price cannot be negative");
                __errors++;
            }


            if (_model.Advert.DateShowEnd < DateTimeOffset.Now) // Ad won't be shown if date less then today
            {
                ModelState.AddModelError("DateShowEnd", "Advert won't be shown if date less then today (Period of validity [end date])");
                __errors++;
            }

            if(30 < (_model.Advert.DateShowEnd - _model.Advert.DateShowStart).Days) // Period of validity must be less 31 days
            {
                ModelState.AddModelError("DateShowEnd", "Period of validity must be less 30 days => " + (_model.Advert.DateShowEnd - _model.Advert.DateShowStart).Days.ToString());
                __errors++;
            }
            
            if(0 < __errors)
            {
                return View(await this.Init_PetAdvsEditViewModel(_model));
            }

            _model.Advert.DateUpdated = DateTimeOffset.Now;

            await m_RepositoryPetAdverts.Update(_model.Advert, _model.State);

            return RedirectToAction("Index", new { t = (int)t });
        }

        public async Task<ActionResult> Details(int? id, AdvertType t = AdvertType.Error)
        {
            if (null == id || t == AdvertType.Error)
            {
                return RedirectToAction("Index", new { t = t });
            }

            var __item = await m_RepositoryPetAdverts.GetById((int)id);

            if (null == __item || __item.Type != t)
            {
                return HttpNotFound();
            }
        
            return View(__item);
        }
        
        [Authorize]
        public async Task<ActionResult> WizRemove(int? id)
        {
            if (null == id || 0 == id)
            {
                return HttpNotFound();
            }

            var __user = await this.GetMemberAsync();
            var __item = await m_RepositoryPetAdverts.GetById((int)id);

            if (null == __item)
            {
                return HttpNotFound();
            }

            if (__item.Pet.MemberId != __user.Id && this.IsAdmin != true)
            {
                return RedirectToAction(c_Error, new { _msg = "You're not allowed to remove this advert. This advert doesn't belong to you!", ReturnUrl = this.ReferUrl });
            }

            int __petId = __item.Pet.Id;

            await m_RepositoryPetAdverts.Remove(__petId);

            // Redirects to the page with all user pets
            return RedirectToAction("Details", "Pet", new { id = __petId });
        }


        #endregion Pet Advert

        #region Pet Review

        [Authorize, HttpPost]
        public async Task<ActionResult> WizReviewEdit(int PetId, string Note, int? Id, bool IsEdit = false)
        {
            PetReview __item;

            if (IsEdit)
            {
                __item = await this.Db.PetReviews.FindAsync(Id);

                if (null == __item)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                __item.Note = Note;
                __item.DateUpdated = DateTimeOffset.Now;

                this.Db.Entry(__item).State = EntityState.Modified;
            }
            else
            {
                var __user = await this.GetMemberAsync();

                __item = new PetReview(PetId, __user.Id, Note);
                this.Db.PetReviews.Add(__item);
            }

            await this.Db.SaveChangesAsync();

            return PartialView("PetReviewDetails", __item);
        }

        [Authorize, HttpPost, ActionName("ReviewRemove")]
        public async Task<string> WizReviewRemove(int? id)
        {
            if (null == id)
            {
                return c_Error;
            }

            var __item = await this.Db.PetReviews.FindAsync(id);

            if (null == __item)
            {
                return c_Error;
            }

            // return HttpNotFound();

            this.Db.PetReviews.Remove(__item);
            this.Db.SaveChanges();

            return string.Empty;
        }

        #endregion Pet Review

        #region Pet Item

        [Authorize]
        public async Task<ActionResult> WizPetEdit(int? p = null, AdvertType t = AdvertType.Error)
        {
            Pet __item = null;
            if (null != p)
            {
                __item = await m_RepositoryPets.GetById((int)p);
            }

            string __referUrl = this.ReferUrl;

            var __entityState = EntityState.Added;

            if (null == __item)
            { // Add new item
                __item = new Pet();
                __item.Breed = await m_RepositoryPets.GetPetBreed_First();
                __item.DateOfBirth = DateTime.Parse($"{DateTime.Today.Year}-{DateTime.Today.Month}-01");
            }
            else
            { // Edit item
                var __user = await this.GetMemberAsync();

                if (__item.MemberId != __user.Id && this.IsAdmin != true)
                {
                    return RedirectToAction(c_Error, new { _msg = "Not allowed to edit this pet. This isn't your pet!", ReturnUrl = __referUrl });
                }

                __entityState = EntityState.Modified;
            }

            var __model = new PetEditViewModel(__item, __item.Breed.Kind, __item.Breed, __referUrl);
            __model.AdvertType = t;
            __model.State = __entityState;
            __model.Kinds = await m_RepositoryPets.GetPetKinds();

            __model.Title = __entityState == EntityState.Added ? "Add new" : "Edit";
            return View(__model);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> WizPetEdit(PetEditViewModel _model)
        {
            if (!ModelState.IsValid)
            {
                return View(_model);
            }

            var __user = await this.GetMemberAsync();

            if (null == __user)
            {
                return View(_model);
            }

            _model.Pet.BreedId = _model.Breed.Id;
            _model.Pet.DateUpdated = DateTimeOffset.Now;

            // if admin will edit so user not deleted by accident
            if (string.IsNullOrEmpty(_model.Pet.MemberId))
            {
                _model.Pet.MemberId = __user.Id;
            }

            await m_RepositoryPets.Update(_model.Pet, _model.State);

            string[] __ddv = _model.ReturnUrl.Split('?');
            string __dd = __ddv[0], __dd1 = null;

            if (_model.AdvertType != AdvertType.Error)
            {
                __dd1 = $"&t={(int)_model.AdvertType}";
            }
            else
            {
                if (_model.ReturnUrl.Contains("&t="))
                {
                    __dd1 = __ddv[1].Split('&')[1]; 
                }
                else if (_model.ReturnUrl.Contains("?t="))
                {
                    __dd1 = __ddv[1]; 
                }
                else
                    return RedirectToAction("Index");

                __dd1 = null != __dd1 ? ("&" + __dd1) : null;
            }

            return Redirect($"{__dd}?p={ _model.Pet.Id}{__dd1}");
            
            // return RedirectToAction("WizEdit",  new { p = _model.Pet.Id, t = (int)_model.Pet. }
            // return Redirect(_model.ReturnUrl.Contains("?p=") || _model.ReturnUrl.Contains("&p=") ? _model.ReturnUrl : $"{_model.ReturnUrl}&p={_model.Pet.Id}");
        }

        #endregion Pet Item
        
        #region Pet Image

        [Authorize]
        public async Task<ActionResult> ImageLoad(int PetId)
        {
            var __pet = await m_RepositoryPets.GetById(PetId);

            if (null == __pet)
            {
                return HttpNotFound();
            }

            var __image = new PetImage { Pet = __pet, PetId = __pet.Id, ReturnUrl = this.ReferUrl };

            return View(__image);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ImageLoad(PetImage _img, HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid && null != UploadImage)
            {
                byte[] __imageData = null;

                // read transferred file into binary reader
                using (var binaryReader = new BinaryReader(UploadImage.InputStream))
                {
                    __imageData = binaryReader.ReadBytes(UploadImage.ContentLength);
                }

                // installation of byte structure
                _img.Image = __imageData;

                if (_img.IsAvatar)
                {
                    var __pet = await m_RepositoryPets.GetById(_img.PetId);
                    __pet.PicAvatar = _img.Name;
                    __pet.Images.Where(zzz => zzz.IsAvatar).ToList().ForEach(zzz =>
                    {
                        zzz.IsAvatar = false;
                    });
                }

                this.Db.PetImages.Add(_img);
                await this.Db.SaveChangesAsync();

                return Redirect(_img.ReturnUrl);
            }

            return View(_img);
        }

        #endregion Pet Image
        
        [Authorize, HttpPost]
        public async Task<ActionResult> WizPetSelectAnother(int? PetId)
        {
            Pet __pet = await m_RepositoryPets.GetById((int)PetId);

            return PartialView("PetDetailsSmallUpdate", __pet);
        }

        /// <summary> Cascade GetBreeds </summary>
        [HttpPost]
        public async Task<ActionResult> GetBreeds(string _id)
        {
            int __id;

            var __items = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(_id))
            {
                __id = int.Parse(_id);

                var __breeds = await m_RepositoryPetAdverts.GetPetBreeds_ByKindId(__id);

                __breeds.ToList().ForEach(zzz =>
                {
                    __items.Add(new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() });
                });

            }

            return Json(__items, JsonRequestBehavior.AllowGet);
        }
        
        void AddErrors(IdentityResult _result)
        {
            foreach (var _error in _result.Errors)
            {
                this.ModelState.AddModelError(null, _error);
            }
        }

        string GetReturnUrl(string _returnUrl) => _returnUrl ?? this.ReferUrl;
    }
}