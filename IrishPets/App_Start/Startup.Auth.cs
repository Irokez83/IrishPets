using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace IrishPets
{
    using Models;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder _app)
        {
            // DB context tune-up
            _app.CreatePerOwinContext(IrishPetsDb.Create);
            _app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            _app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Cookies tune-up and starting
            _app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // User authorisation check (in case of password change or adding login name)
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, Member>
                        (validateInterval: TimeSpan.FromMinutes(30), 
                         regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            // Use a cookies to temporarily store information about a user logging in with a third party login provider
            _app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            
            // Allows app temporarily to hold user information 
            _app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Allows app to remember user authentification
            _app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

        }
    }
}