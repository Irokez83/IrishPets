using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IrishPets.Controllers
{
    using Models;
    using System.Collections.Generic;

    [Authorize(Roles = "Admin")]
    public class PetServiceController : BaseController
    {
        private IRepositoryCounties<PetService, int> m_Repository;

        public PetServiceController(IRepositoryCounties<PetService, int> _repo) : base()
        {
            m_Repository = _repo;
        }

        // GET: PetService
        [AllowAnonymous]
        public async Task<ActionResult> Index(string q = null, int? CountyId = null) => View(await this.GetIndex(q, CountyId));
        
        async Task<PetServiceViewModel> GetIndex(string q = null, int? CountyId = null)
        {
            var __items = await m_Repository.GetAll();

            __items = __items.OrderBy(zzz => zzz.Name);

            if (this?.IsAdmin != true)
            {
                __items = __items.Where(zzz => zzz.Enabled);
            }
            
            if (!string.IsNullOrEmpty(q))
            {
                __items = __items.Where(zzz => zzz.StringForSearch.Contains(q));
            }

            if (null != CountyId)
            {
                __items = __items.Where(zzz => zzz.CountyId == CountyId);
            }

            return new PetServiceViewModel(await m_Repository.GetCounties(), __items.ToList());
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
            var __item = new PetService();
            __item.Counties = await m_Repository.GetCounties();
            return View(__item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PetService petService)
        {
            if (ModelState.IsValid)
            {
                await m_Repository.Update(petService, EntityState.Added);
                return RedirectToAction("Index");
            }

            return View(petService);
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

            __item.Counties = await m_Repository.GetCounties();

            return View(__item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PetService petService)
        {
            if (ModelState.IsValid)
            {
                await m_Repository.Update(petService, EntityState.Modified);
                return RedirectToAction("Index");
            }

            return View(petService);
        }

        public async Task<ActionResult> Delete(int? id)
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

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await m_Repository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
