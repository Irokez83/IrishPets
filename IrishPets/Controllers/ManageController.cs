using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IrishPets.Controllers
{
    using Models;

    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager m_SignInManager;
        private ApplicationUserManager m_UserManager;
        private IrishPetsDb m_IrishPetsDb = IrishPetsDb.Create();

        public ManageController() { }

        public ManageController(ApplicationUserManager _userManager, ApplicationSignInManager _signInManager)
        {
            this.UserManager = _userManager;
            this.SignInManager = _signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return m_SignInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { m_SignInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return m_UserManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { m_UserManager = value; }
        }

        //GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? _message)
        {
            ViewBag.StatusMessage =
                _message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : _message == ManageMessageId.ChangeInfoSuccess ? "Your profile info was changed."
                : _message == ManageMessageId.SetPasswordSuccess ? "Specify a password."
                : _message == ManageMessageId.SetTwoFactorSuccess ? "Configured a provider of two-factor authentication."
                : _message == ManageMessageId.Error ? "Error occurred."
                : _message == ManageMessageId.AddPhoneSuccess ? "Your phone number added."
                : _message == ManageMessageId.RemovePhoneSuccess ? "Your phone number removed."
                : string.Empty;

            var __userId = this.User.Identity.GetUserId();

            var __user = await this.UserManager.FindByIdAsync(__userId);
            if (null != __user)
                await this.SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: true);

            var __model = new IndexViewModel
            {
                HasPassword = this.HasPassword(),
                PhoneNumber = await this.UserManager.GetPhoneNumberAsync(__userId),
                TwoFactor = await this.UserManager.GetTwoFactorEnabledAsync(__userId),
                Logins = await this.UserManager.GetLoginsAsync(__userId),
                BrowserRemembered = await this.AuthenticationManager.TwoFactorBrowserRememberedAsync(__userId)
            };

            return View(__model);
        }

        //GET: /Manage/ChangeInfo
        public async Task<ActionResult> ChangeInfo()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer.PathAndQuery;

            var __userId = this.User.Identity.GetUserId();

            var __user = await this.UserManager.FindByIdAsync(__userId);

            var __model = new ContactInfoViewModel2(__user);

            __model.Counties = await m_IrishPetsDb
                .Counties
                .Select(zzz => new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() })
                .ToListAsync();

            return View(__model);
        }

        //POST: /Manage/ChangeInfo
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeInfo(ContactInfoViewModel2 _model, string returnUrl=null)
        {

            _model.Counties = await m_IrishPetsDb
                .Counties
                .Select(zzz => new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() })
                .ToListAsync();


            if (!ModelState.IsValid)
            {
                return View(_model);
            }

            var __user = await this.UserManager.FindByIdAsync(_model.Id);

            if (null == __user)
            {
                return View(_model);
            }

            if (_model.DateOfBirth.HasValue)
            {
                int _age = DateTime.Now.Year - _model.DateOfBirth.Value.Year;


                if (115 < _age)
                {
                    this.ModelState.AddModelError("DateOfBirth", $"Noone lives that many => {_age} years");
                    return View(_model);              
                }
            }

            _model.Update(__user);
            
            var __result = await this.UserManager.UpdateAsync(__user);

            if (__result.Succeeded)
            {
                if(string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index", new { Message = ManageMessageId.ChangeInfoSuccess});
                else
                    return Redirect(returnUrl);
            }

            this.AddErrors(__result);

            return View(_model);
        }

        //POST: /Manage/RemoveLogin
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string _loginProvider, string _providerKey)
        {
            ManageMessageId? __message;
            var __result = await this.UserManager.RemoveLoginAsync(this.User.Identity.GetUserId(), new UserLoginInfo(_loginProvider, _providerKey));
            if (__result.Succeeded)
            {
                var __user = await UserManager.FindByIdAsync(this.User.Identity.GetUserId());
                if (null != __user)
                    await this.SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: false);

                __message = ManageMessageId.RemoveLoginSuccess;
            }
            else
                __message = ManageMessageId.Error;

            return this.RedirectToAction("ManageLogins", new { Message = __message });
        }

        //GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //POST: /Manage/AddPhoneNumber
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel _model)
        {
            if (! ModelState.IsValid)
                return View(_model);

            //Marker created and sent
            var __code = await this.UserManager.GenerateChangePhoneNumberTokenAsync(this.User.Identity.GetUserId(), _model.Number);
            if (null != UserManager.SmsService)
            {
                var __message = new IdentityMessage
                {
                    Destination = _model.Number,
                    Body = "Your security code: " + __code
                };

                await this.UserManager.SmsService.SendAsync(__message);
            }

            return this.RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = _model.Number });
        }

        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await this.UserManager.SetTwoFactorEnabledAsync(this.User.Identity.GetUserId(), true);
            var __user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
            if (null != __user)
                await this.SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", "Manage");
        }

        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await this.UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);

            var __user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (null != __user)
                await this.SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: false);

            return this.RedirectToAction("Index", "Manage");
        }

        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string _phoneNumber)
        {
            var __code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), _phoneNumber);
            //SMS sent to verify phone number
            return _phoneNumber == null ? View("Error") 
                : View(new VerifyPhoneNumberViewModel { PhoneNumber = _phoneNumber });
        }

        // POST: /Manage/VerifyPhoneNumber
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel _model)
        { 
            if (! ModelState.IsValid)
                return View(_model);

            var __result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), _model.PhoneNumber, _model.Code);
            if (__result.Succeeded)
            {
                var __user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (null != __user)
                    await this.SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: false);

                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            //Error means form repeated
            this.ModelState.AddModelError(string.Empty, "Failed to check phone");
            return View(_model);
        }

        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var __result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);

            if (!__result.Succeeded)
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });

            var __user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (null != __user)
                await SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword() => View();

        // POST: /Manage/ChangePassword
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel _model)
        {
            if (!ModelState.IsValid)
                return View(_model);

            var __result = await this.UserManager.ChangePasswordAsync(User.Identity.GetUserId(), _model.OldPassword, _model.NewPassword);
            if (__result.Succeeded)
            {
                var __user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());

                if (null != __user)
                    await SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: false);

                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            this.AddErrors(__result);

            return View(_model);
        }

        // GET: /Manage/SetPassword
        public ActionResult SetPassword() => View();

        // POST: /Manage/SetPassword
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel _model)
        {
            if (this.ModelState.IsValid)
            {
                var __result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), _model.NewPassword);

                if (__result.Succeeded)
                {
                    var __user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (null != __user)
                        await SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }

                this.AddErrors(__result);
            }

            //Error form repeated again
            return View(_model);
        }

        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? _message)
        {
            ViewBag.StatusMessage =
                _message == ManageMessageId.RemoveLoginSuccess ? "External login removed." : 
                _message == ManageMessageId.Error ? "Error occurred." : 
                string.Empty;

            var __user = await this.UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (null == __user)
                return View("Error");

            var __userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var __otherLogins = AuthenticationManager
                .GetExternalAuthenticationTypes()
                .Where(auth => __userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider))
                .ToList();

            ViewBag.ShowRemoveButton = null != __user.PasswordHash || 1 < __userLogins.Count;

            return View(new ManageLoginsViewModel
            {
                CurrentLogins = __userLogins,
                OtherLogins = __otherLogins
            });
        }

        // POST: /Manage/LinkLogin
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            //Request to external supplier to tie up with existing user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), this.User.Identity.GetUserId());
        }

        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var __loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, this.User.Identity.GetUserId());

            if (null == __loginInfo)
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });

            var __result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), __loginInfo.Login);

            return __result.Succeeded ? RedirectToAction("ManageLogins") : 
                RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool _disposing)
        {
            if (_disposing && null != m_UserManager)
            {
                m_UserManager.Dispose();
                m_UserManager = null;
            }

            base.Dispose(_disposing);
        }

        #region Extra features

        //Protect against XSRF-attacks
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication; 

        private void AddErrors(IdentityResult _result)
        {
            foreach (var _error in _result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, _error);
            }
        }

        private bool HasPassword()
        {
            var __user = this.UserManager.FindById(User.Identity.GetUserId());

            if (null != __user)
                return __user.PasswordHash != null;

            return false;
        }

        private bool HasPhoneNumber()
        {
            var __user = this.UserManager.FindById(User.Identity.GetUserId());
            if (null != __user)
                return null != __user.PhoneNumber;

            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangeInfoSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion Extra features
    }
}