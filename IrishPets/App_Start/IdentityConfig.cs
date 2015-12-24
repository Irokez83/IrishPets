using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IrishPets
{
    using Models;

    /// <summary> User dispatcher settings tune-up. UserManager loads in ASP.NET Identity and used by application. </summary>
    public class ApplicationUserManager : UserManager<Member>
    {
        public ApplicationUserManager(IUserStore<Member> _store) : base(_store) { }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> _options, IOwinContext _context) 
        {
            var __manager = new ApplicationUserManager(new UserStore<Member>(_context.Get<IrishPetsDb>()));
            
            // Logic settings to check validation of user names
            __manager.UserValidator = new UserValidator<Member>(__manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Logic settings to check validation of passwords
            __manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6, //min password length
                RequireDigit = true, //at least one digit
                RequireNonLetterOrDigit = false, //at least one special character
                RequireLowercase = false, //at least one small letter
                RequireUppercase = false, //at least one capital letter
            };

            // Settings to lock-out user from logging
            __manager.UserLockoutEnabledByDefault = true;
            __manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            __manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            __manager.RegisterTwoFactorProvider("Your code from message", new EmailTokenProvider<Member>
            {
                Subject = "Security code", BodyFormat = "Your security code: {0}"
            });

            __manager.EmailService = new EmailService();

            var __dataProtectionProvider = _options.DataProtectionProvider;
            if (__dataProtectionProvider != null)
            {
                __manager.UserTokenProvider = new DataProtectorTokenProvider<Member>(__dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return __manager;
        }
    }

    /// <summary> App login tune-up </summary>
    public class ApplicationSignInManager : SignInManager<Member, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(Member _user) 
            => _user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> _options, IOwinContext _context)
            => new ApplicationSignInManager(_context.GetUserManager<ApplicationUserManager>(), _context.Authentication);
    }
}
