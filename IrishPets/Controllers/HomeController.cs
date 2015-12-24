using IrishPets.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IrishPets.Controllers
{
    public class HomeController : BaseController
    {
        private IRepositoryEx<AdvAda, int> m_AdvAdaRepository;

        public HomeController(IRepositoryEx<AdvAda, int> _advAdaRepo) : base()
        {
            m_AdvAdaRepository = _advAdaRepo;
        }

        public async Task<ActionResult> Index()
        {
            var __now = DateTime.Now;

            var __model = new HomeViewModel();
            var __lst_1 = await m_AdvAdaRepository.GetAll();
            __model.AdvAdas = __lst_1.Where(zzz => zzz.DateShowStart <= __now && __now <= zzz.DateShowEnd && zzz.Enabled)
                              .Take(3) // Take first 3 records 
                              .ToList()
                              ;

            __model.AdvPets = await this.Db.PetAdverts
                              .AsNoTracking()
                              .Where(zzz => zzz.DateShowStart <= __now && __now <= zzz.DateShowEnd && zzz.Enabled
                                      && zzz.TypeOfSale == TypeOfSale.Commercial)
                              .Take(3) // Take first 3 records 
                              .ToListAsync()
                              ;

            return View(__model);
        }

        [Authorize]
        public ActionResult IndexAuthorize()
        {
            ApplicationUserManager __userManager = HttpContext
                                            .GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>()
                                            ;
            IList<string> __roles = new List<string> { "The role is not defined" };

            Member __user = __userManager.FindByEmail(User.Identity.Name);

            if (null != __user)
            {
                __roles = __userManager.GetRoles(__user.Id);
            }

            return View(__roles);
        }

        public ActionResult Contact() => View(new ContactUsViewModel { });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactUsViewModel _model)
        {
            if (ModelState.IsValid)
            {
                if (null != UserManager.EmailService)
                {
                    var __message = new IdentityMessage
                    {
                        Destination = Properties.Resources.DefEmail_Account,
                        Subject = $"[ContactUs] {_model.Subject}",
                        Body = $"{_model.FirstName} {_model.LastName}<br/>Replya e-mail: {_model.Email}<br/>Company: {_model.Company}<br/><br/>{_model.Comments?.Replace(Environment.NewLine, "<br/>")}"
                    };
                    await this.UserManager.EmailService.SendAsync(__message);
                }
                return RedirectToAction("MessageHasBeenSent");
            }
            return View(_model);
        }

        public ActionResult MessageHasBeenSent() => View();
        public ActionResult About() => View();

        /// <summary> Developer database update tool </summary>
        //[Authorize(Roles = "Admin")]
        public void SetU() => Migrations.ConfigurationEx.CreateNewUsersAndRole(IrishPetsDb.Create());
    }
}