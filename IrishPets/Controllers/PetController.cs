using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IrishPets.Controllers
{
    using Models;

    [Authorize]
    public class PetController : BaseController
    {
        private IRepositoryPets<Pet, int> m_Repository;

        public PetController(IRepositoryPets<Pet, int> _repo) : base()
        {
            m_Repository = _repo;
        }
        
        public PetController(IRepositoryPets<Pet, int> _repo, UserManager<Member> _mang) : base()
        {
            m_Repository = _repo;

            var __name = (((System.Security.Claims.ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal).Identities.ToList()[0]).Name;
            m_Repository.Member = _mang.Users.FirstOrDefault(zzz => zzz.UserName == __name);
        }

        public async Task<ActionResult> Index(string q = null)
        {
            //var __member = await this.GetMemberAsync();
            //m_Repository.Member = this.GetMember();

            if (null == m_Repository.Member)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            return View(await this.GetIndex(m_Repository.Member, q));
        }
        
        public async Task<PetsViewModel> GetIndex(Member _member, string _searchString = null)
        {
            IEnumerable<Pet> __items;
            if (this.IsAdmin == true)
            {
                __items = await m_Repository.GetAll();
            }
            else
            {
                __items = _member.Pets.OrderBy(zzz => zzz.Name);
            }
            
            if (string.IsNullOrEmpty(_searchString) == false)
            {
                __items = __items.Where(zzz => (zzz.StringForSearch.Contains(_searchString)));
            }

            var __model = new PetsViewModel();
            __model.Pets = __items.ToList();

            return __model;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var __item = await m_Repository.GetById((int)id);
            if (null == __item)
            {
                return HttpNotFound();
            }

            return View(__item);
        }

        public async Task<ActionResult> Create()
        {
            var __kind = await m_Repository.GetPetKind_First();

            var __model = new PetEditViewModel(new Pet(), __kind);
            __model.SetPetKind(await m_Repository.GetPetKinds());
            __model.Pet.DateOfBirth = DateTime.Parse($"{DateTime.Today.Year}-{DateTime.Today.Month}-01");

            return View(__model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PetEditViewModel _model)
        {
            if (!ModelState.IsValid)
            {
                await this.SetPetKind(_model);

                return View(_model);
            }

            // var __member = await this.GetMemberAsync();

            _model.Pet.MemberId = m_Repository.Member.Id;
            _model.Pet.BreedId = _model.Breed.Id;
            _model.Pet.DateUpdated = DateTimeOffset.Now;

            await m_Repository.Update(_model.Pet, EntityState.Added);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var __item = await m_Repository.GetById((int)id);

            if (null == __item)
            {
                return HttpNotFound();
            }

            string __returnUrl = this.ReferUrl;

            //var __member = await this.GetMemberAsync();

            if (__item.MemberId != m_Repository.Member.Id && this.IsAdmin != true)
            {
                return RedirectToAction(c_Error, new
                {
                    _msg = "You're not allowed to edit this pet. This is not your pet!",
                    ReturnUrl = __returnUrl
                });
            }

            var __model = new PetEditViewModel(__item, __item.Breed.Kind, __item.Breed, __returnUrl);
            __model.Kinds = await m_Repository.GetPetKinds();

            return View(__model);
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PetEditViewModel _model)
        {
            if (!ModelState.IsValid)
            {
                await this.SetPetKind(_model);

                return View(_model);
            }

            //var __member = await this.GetMemberAsync();

            if (string.IsNullOrEmpty(_model.Pet.MemberId))
            {
                _model.Pet.MemberId = m_Repository.Member.Id;
            }

            _model.Pet.DateUpdated = DateTimeOffset.Now;
            _model.Pet.BreedId = _model.Breed.Id;

            await m_Repository.Update(_model.Pet, EntityState.Modified);


            return Redirect(_model.ReturnUrl ?? "Index");
        }

        [Authorize]
        public async Task<ActionResult> Remove(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pet __item = await m_Repository.GetById((int)id);

            await m_Repository.Remove((int)id);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ImageLoad(int PetId)
        {
            var __item = await this.Db.Pets.FindAsync(PetId);

            if (null == __item)
                return HttpNotFound();

            var __image = new PetImage
            {
                Pet = __item, PetId = __item.Id, ReturnUrl = this.ReferUrl
            };

            return View(__image);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ImageLoad(PetImage _img, HttpPostedFileBase UploadImage)
        {
            if (ModelState.IsValid && null != UploadImage && 0 != UploadImage.ContentLength)
            {
                byte[] __imageData = null;

                //Move to binary reader
                using (var binaryReader = new BinaryReader(UploadImage.InputStream))
                {
                    __imageData = binaryReader.ReadBytes(UploadImage.ContentLength);
                }

                //Picture type and size
                _img.ContentLength = UploadImage.ContentLength;
                _img.ContentType = UploadImage.ContentType;
                //Picture moved to ad
                _img.Image = __imageData;

                if (_img.IsAvatar)
                {
                    //In case of new picture - automatically marked as main
                    var __pet = await this.Db.Pets.FindAsync(_img.PetId);
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


        /// <summary> Cascade list KindId -> GetBreeds </summary>
        [HttpPost]
        public async Task<ActionResult> GetBreeds(string _id)
        {
            int __id;

            var __items = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(_id))
            {
                __id = int.Parse(_id);

                var __kind = await m_Repository.GetPetKind_ById(__id);

                __kind.Breeds.ToList().ForEach(zzz =>
                        {
                            __items.Add(new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() });
                        });

            }

            return Json(__items, JsonRequestBehavior.AllowGet);
        }

        public async Task<PetEditViewModel> SetPetKind(PetEditViewModel _model)
        {
            PetKind __kind;

            if (null == _model.Kind)
                __kind = await m_Repository.GetPetKind_First();
            else
                __kind = await m_Repository.GetPetKind_ById(_model.Kind.Id);

            _model.SetPetKind(await m_Repository.GetPetKinds(), __kind);

            return _model;
        }

    }
}