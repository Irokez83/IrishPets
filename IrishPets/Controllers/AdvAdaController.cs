using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IrishPets.Controllers
{
    using Models;

    [Authorize(Roles = "Admin")]
    public class AdvAdaController : BaseController
    {
        private IRepositoryEx<AdvAda, int> m_Repository;

        public AdvAdaController(IRepositoryEx<AdvAda, int> _repo) : base()
        {
            m_Repository = _repo;
        }

        // GET: AdvAda
        public async Task<ActionResult> Index(string q = null) => View(await this.GetIndex(q));

        async Task<AdvAdasViewModel> GetIndex(string _searchString = null, int? _countyId = null)
        {
            var __lst = await m_Repository.GetAll();
            __lst = __lst.OrderByDescending(zzz => zzz.DateShowEnd);

            if (string.IsNullOrEmpty(_searchString) == false)
            {
                __lst = __lst.Where(zzz => (zzz.Note.Contains(_searchString)));
            }

            return new AdvAdasViewModel(__lst.ToList());
        }

        public async Task<ActionResult> Edit(int? id = null)
        {
            var __item = default(AdvAda);
            if (null != id)
            {
                __item = await m_Repository.GetById((int)id);
            }

            var __model = new AdvAdaViewModel(__item);
            __model.Title = null == id ? "Add New Advert" : "Edit Advert";

            return View(__model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AdvAdaViewModel _model)
        {
            if (!ModelState.IsValid)
            {
                return View(_model);
            }

            if (string.IsNullOrWhiteSpace(_model.Note))
            {
                ModelState.AddModelError("Note", "Need fill Note.");
                return View(_model);
            }

            if (_model.DateShowEnd < DateTime.Now) // Ad won't be shown if date less then today
            {
                ModelState.AddModelError("DateShowEnd", "Ad won't be shown if date less then today");
                return View(_model);
            }

            await m_Repository.Update(_model.Item, _model.State);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Remove(int? id)
        {
            if (null == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var __item = await m_Repository.GetById((int)id);

            if (null == __item)
            {
                return RedirectToAction("Index");
            }

            await m_Repository.Remove((int)id);

            return RedirectToAction("Index");
        }
    }
}