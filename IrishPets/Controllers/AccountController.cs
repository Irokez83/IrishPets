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
    public class AccountController : Controller
    {
        private ApplicationSignInManager m_SignInManager;
        private ApplicationUserManager m_UserManager;
        private IrishPetsDb m_Db = IrishPetsDb.Create();

        public AccountController() { }

        public AccountController(ApplicationUserManager _userManager, ApplicationSignInManager _signInManager )
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

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel _model, string returnUrl)
        {
            if (! ModelState.IsValid)
                return View(_model);

            //User not locked-out if types in wrong login credentials, however this can be changed if change - shouldLockout: true
            var __result = await SignInManager.PasswordSignInAsync(_model.Email, _model.Password, _model.RememberMe, shouldLockout: false);
            switch (__result)
            {
                case SignInStatus.Success:
                    var __user = await UserManager.FindAsync(_model.Email, _model.Password);
                    __user.DateOfLastLogin = DateTime.Now;
                    await UserManager.UpdateAsync(__user);

                    #region E-mail registration confirmation

                    if (__user.EmailConfirmed)
                    {
                        await SignInManager.SignInAsync(__user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "E-mail not confirmed.");
                        return View(_model);
                    }
                #endregion E-mail registration confirmation

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = __returnUrl, RememberMe = _model.RememberMe });

                case SignInStatus.Failure:

                default:
                    ModelState.AddModelError(string.Empty, "The login attempt failed.");
                    return View(_model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string _provider, string returnUrl, bool _rememberMe)
        {
            //Request user entry
            if (!await SignInManager.HasBeenVerifiedAsync())
                return View("Error");

            return View(new VerifyCodeViewModel { Provider = _provider, ReturnUrl = returnUrl, RememberMe = _rememberMe });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel _model)
        {
            if (! ModelState.IsValid)
                return View(_model);

            //Code security against two-factor authentication trials 
            //If user enters wrong passowrd several times - his account will become unavailable for a certain period of time 
            //Blocking parameters may be changed in IdentityConfig
            var __result = await SignInManager.TwoFactorSignInAsync(_model.Provider, _model.Code, isPersistent:  _model.RememberMe, rememberBrowser: _model.RememberBrowser);
            switch (__result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(_model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError(string.Empty, "Wrong code.");
                    return View(_model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            var __model = new RegisterViewModel();

            __model.DateOfBirth = DateTime.Now.AddYears(-25); 
            __model.Counties = await m_Db
                .Counties
                .Select(zzz => new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() })
                .ToListAsync();

            return View(__model);
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel _model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _model.UserName = _model.Email;

                    var __user = _model.CreateNew(_model.UserName, _model.Email);
                    __user.CountyId = _model.CountyId;

                    var __result = await UserManager.CreateAsync(__user, _model.Password);
                    if (__result.Succeeded)
                    {
                        //if registration successfull then add "User" role
                        await UserManager.AddToRoleAsync(__user.Id, "User");

                        #region E-mail registration confirmation

                        //generate token for registration
                        var __code = await UserManager.GenerateEmailConfirmationTokenAsync(__user.Id);
                        //create hyperlink for registration
                        var __callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = __user.Id, code = __code }, protocol: Request.Url.Scheme);
                        
                        //sending an e-mail
                        await UserManager.SendEmailAsync(__user.Id, "Confirmation e-mail",
                        "In order to complete the registration please click on the following link:: <a href=\"" + __callbackUrl + "\">Click me</a>");

                        return View("DisplayEmail");

                        #endregion E-mail registration confirmation
                    }

                    AddErrors(__result);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException _ex)
                {
                    var __sb = new System.Text.StringBuilder();

                    foreach (var _failure in _ex.EntityValidationErrors)
                    {
                        __sb.AppendFormat($"{_failure.Entry.Entity.GetType()} failed validation\n");
                        foreach (var _error in _failure.ValidationErrors)
                        {
                            __sb.AppendFormat($"- {_error.PropertyName} : {_error.ErrorMessage}");
                            __sb.AppendLine();
                        }
                    }

                    throw new System.Data.Entity.Validation
                        .DbEntityValidationException($"Entity Validation Failed - errors:\n{__sb.ToString()}", _ex ); // Add the original exception as the innerException

                }
                catch (Exception _ex)
                {
                    System.Diagnostics.Debug.WriteLine(_ex.Message);
                    ModelState.AddModelError(string.Empty, _ex.Message);
                }
            }

            _model.Counties = await m_Db
                .Counties
                .Select(zzz => new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() })
                .ToListAsync()
                ;
            //If this message appears - form is used twice
            return View(_model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (null == userId || null == code)
            {
                return View("Error");
            }
            var __result = await this.UserManager.ConfirmEmailAsync(userId, code);
            return View(__result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword() => View();


        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel _model)
        {
            if (ModelState.IsValid)
            {
                var __user = await this.UserManager.FindByNameAsync(_model.Email);
                if (null == __user || !(await this.UserManager.IsEmailConfirmedAsync(__user.Id)))
                {
                    // Do not show if user is not registered or registration confirmed
                    return View("ForgotPasswordConfirmation");
                }

                string __code = await UserManager.GeneratePasswordResetTokenAsync(__user.Id);

                var __callbackUrl = Url.Action("ResetPassword", "Account", new { userId = __user.Id, code = __code }, protocol: Request.Url.Scheme);

                await UserManager.SendEmailAsync(__user.Id, "ResetPassword", "In order to reset the password, please follow the link <a href=\"" + __callbackUrl + "\">Click me</a>");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            //If this message appears - form is used twice
            return View(_model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation() => View();

        [AllowAnonymous]
        public ActionResult ResetPassword(string _code) =>  null == _code ? View("Error") : View();
        
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel _model)
        {
            if (!ModelState.IsValid)
                return View(_model);

            var __user = await UserManager.FindByNameAsync(_model.Email);
            if (null == __user)
                // Do not show that user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");

            var __result = await UserManager.ResetPasswordAsync(__user.Id, _model.Code, _model.Password);
            if (__result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation", "Account");

            AddErrors(__result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation() => View();

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var __userId = await SignInManager.GetVerifiedUserIdAsync();

            if (null == __userId)
            {
                return View("Error");
            }

            var __userFactors = await this.UserManager.GetValidTwoFactorProvidersAsync(__userId);
            var __factorOptions = __userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = __factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel _model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Marker created and sent
            if (!await SignInManager.SendTwoFactorCodeAsync(_model.SelectedProvider))
            {
                return View("Error");
            }

            return RedirectToAction("VerifyCode", new
            {
                Provider = _model.SelectedProvider, ReturnUrl = _model.ReturnUrl, RememberMe = _model.RememberMe
            });
        }

       
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        #region Edit and delete users

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed()
        {
            Member __user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (null != __user)
            {
                IdentityResult __result = await UserManager.DeleteAsync(__user);
                if (__result.Succeeded)
                    return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Edit()
        {
            Member __user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (null != __user)
            {
                var __model = new RegisterViewModel
                {
                    UserName = __user.UserName, Email = __user.Email
                };

                __model.Counties = await m_Db
                    .Counties
                    .Select(zzz => new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() })
                    .ToListAsync();
                
                return View(__model);
            }


            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RegisterViewModel _model)
        {
            Member __user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (null != __user)
            {
                __user.Email = _model.Email;
                __user.UserName = _model.UserName;
                IdentityResult __result = await UserManager.UpdateAsync(__user);
                if (__result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError(string.Empty, "Error occured - something went wrong.");
            }
            else
                ModelState.AddModelError(string.Empty, "This UserName can not be found");

            return View(_model);
        }
        
        #endregion Edit and delete users

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure() => View();

        protected override void Dispose(bool _disposing)
        {
            if (_disposing)
            {
                if (null != m_UserManager)
                {
                    m_UserManager.Dispose();
                    m_UserManager = null;
                }

                if (null != m_SignInManager)
                {
                    m_SignInManager.Dispose();
                    m_SignInManager = null;
                }

                if (null != m_Db)
                {
                    m_Db.Dispose();
                    m_Db = null;
                }
            }

            base.Dispose(_disposing);
        }

        #region Extra features

        // Protect against XSRF-attacks
        private const string XsrfKey = "XsrfId";
        private string __returnUrl = null;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult _result)
        {
            foreach (var _error in _result.Errors)
                ModelState.AddModelError(string.Empty, _error);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string _provider, string _redirectUri) : this(_provider, _redirectUri, null)
            {
            }

            public ChallengeResult(string _provider, string _redirectUri, string _userId)
            {
                this.LoginProvider = _provider;
                this.RedirectUri = _redirectUri;
                this.UserId = _userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext _context)
            {
                var __properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (null != this.UserId)
                    __properties.Dictionary[XsrfKey] = this.UserId;

                _context.HttpContext.GetOwinContext().Authentication.Challenge(__properties, LoginProvider);
            }
        }

        #endregion Extra features
    }
}