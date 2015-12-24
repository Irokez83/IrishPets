using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IrishPets.Controllers
{
    using Microsoft.Owin.Security;
    using Models;

    public class BaseController : Controller
    {
        public const string c_Error = "Error";

        public BaseController()
        {

        }

        private ApplicationUserManager m_UserManager;
        public ApplicationUserManager UserManager
        {
            get { return m_UserManager ?? this.HttpContext?.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { m_UserManager = value; }
        }

        public IrishPetsDb Db = IrishPetsDb.Create();
 
        public string GetString(string t = null, int? p = null)
        {
            if (null == t || null == p)
                return null;

            string[] __ddv = t.Split('?');

            string __dd = __ddv[0], __dd1 = null;
            if (t.Contains("&t="))
            {
                __dd1 = __ddv[1].Split('&')[1]; // get t=2 - Type of Advert
            }
            else if (t.Contains("?t="))
            {
                __dd1 = __ddv[1]; // get t=2 - Type of Advert
            }

            t = $"{__dd}?p={p}{(null != __dd1 ? ("&" + __dd1) : null)}";

            return t;
        }

        public ActionResult Error(string _msg = null, string ReturnUrl = null)
        {
            ViewBag.Message = _msg;
            ViewBag.ReturnUrl = ReturnUrl;
            return View("~/Views/Shared/Error.cshtml");
        }
        
        void AddErrors(IdentityResult _result)
        {
            foreach (var _error in _result.Errors)
            {
                this.ModelState.AddModelError(null, _error);
            }
        }

        string GetReturnUrl(string _returnUrl) => _returnUrl ?? this.ReferUrl;

        public string ReferUrl => null == Request?.UrlReferrer ? c_Error : Request.UrlReferrer.PathAndQuery;
        public bool IsUserAuthenticated => this.User.Identity.IsAuthenticated;
        public bool? IsAdmin => this.User?.IsInRole(Properties.Resources.DefAdmin);

        /// <summary> For testing </summary>
        public Func<string> GetUserId => () => this.User?.Identity.GetUserId();

        /// <summary> Registered user view </summary>
        public string GetInfo()
        {
            var __identity = (System.Security.Claims.ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;

            var __email = HttpContext.User.Identity.Name;

            var __gender = __identity.Claims
                        .Where(c => c.Type == "Gender")
                        .Select(c => c.Value)
                        .SingleOrDefault();

            var __dateOfBirth = __identity.Claims
                        .Where(c => c.Type == "DateOfBirth")
                        .Select(c => c.Value)
                        .SingleOrDefault();

            return "<p>E-mail: " + __email + "</p><p>Gender: " + __gender + "</p><p> Date Of Birth: " + __dateOfBirth + "</p>";
        }

        /// <summary> Shows user which is authorised in the system. Returns current user which is authorised in the system </summary>
        public Member GetMember() => this.UserManager?.FindById(this.GetUserId());
        public async Task<Member> GetMemberAsync() => await this.UserManager?.FindByIdAsync(this.GetUserId());

        public void DDD(IPrincipal _user)
        {
            HttpContext.User = _user;
        }

        // Add this private variable
        private IAuthenticationManager m_AuthnManager;

        // Modified this from private to public and add the setter
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (null == m_AuthnManager)
                    m_AuthnManager = HttpContext.GetOwinContext().Authentication;

                return m_AuthnManager;
            }
            set { m_AuthnManager = value; }
        }

        public static HttpContext Dummy_CreateHttpContext(bool _userLoggedIn)
        {
            var __roles = _userLoggedIn ? new[] { "Admin", "User" } : new string[0];
            var __ident = new GenericIdentity(_userLoggedIn ? "Admin001@IrishPets.ie" : null);

            var __httpContext = new HttpContext(new HttpRequest(string.Empty, Properties.Resources.PayPal_ReturnUrl, string.Empty), new HttpResponse(new StringWriter()))
            {
                User = new GenericPrincipal(__ident, __roles)
            };

            return __httpContext;
        }


        protected override void Dispose(bool _disposing)
        {
            if (_disposing)
            {
                if (null != m_UserManager)
                {
                    m_UserManager.Dispose();
                    m_UserManager = null;
                }

                if (null != this.Db)
                {
                    this.Db.Dispose();
                    this.Db = null;
                }
            }

            base.Dispose(_disposing);
        }
    }
}