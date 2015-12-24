using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Unity.Mvc5;

namespace IrishPets
{
    using Controllers;
    using Models;

    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var __container = new UnityContainer();
            
            __container.RegisterType<IUserStore<Member>, UserStore<Member>>();
            __container.RegisterType<UserManager<Member>>();
            //__container.RegisterType<DbContext, IrishPetsDb>();
            __container.RegisterType<DbContext, IrishPetsDb>(new HierarchicalLifetimeManager());
            //__container.RegisterType<ApplicationUserManager>();
            __container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
            __container.RegisterType<UserManager<Member>>(new HierarchicalLifetimeManager());
            __container.RegisterType<IUserStore<Member>, UserStore<Member>>(new HierarchicalLifetimeManager());

            __container.RegisterType<AccountController>(new InjectionConstructor());

            // Register the Repositories in the Unity Container
            __container.RegisterType<IRepositoryEx<AdvAda, int>, EF_AdvAdaRepository>();
            __container.RegisterType<IRepositoryCounties<PetService, int>, EF_PetServiceRepository>();
            __container.RegisterType<IRepositoryPets<Pet, int>, EF_PetRepository>();
            __container.RegisterType<IRepositoryPets<PetAdvert, int>, EF_PetAdvertRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(__container));
        }
    }
}