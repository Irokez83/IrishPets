using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IrishPets.Controllers
{
    using Models;

    /// <summary> Admin area </summary>
    [Authorize(Roles = "Admin")]
    public class AdaController : BaseController
    {
        public async Task<ActionResult> Index(MemberSort? Sort = null, string q = null, int? CountyId = null)
        {
            var __now = DateTimeOffset.Now;

            IQueryable<Member> __itemsQ = this.Db.Users.AsNoTracking();

            if (!string.IsNullOrEmpty(q))
            {
                __itemsQ = __itemsQ
                        .Where(zzz => (
                               zzz.FirstName.Contains(q)
                            || zzz.Surname.Contains(q)
                            || zzz.Email.Contains(q)
                            || zzz.UserName.Contains(q)
                            || zzz.Note.Contains(q)
                            || zzz.PhoneNumber.Contains(q)
                            || zzz.Postcode.Contains(q)
                            || zzz.County.Name.Contains(q)
                            || zzz.Street.Contains(q)
                            || zzz.Town.Contains(q)
                            || zzz.UserName.Contains(q)));
            }

            if (null != CountyId)
            {
                __itemsQ = __itemsQ.Where(zzz => zzz.CountyId == CountyId);
            }

            var __advertsQOrder = __itemsQ;

            switch (Sort)
            {
                case MemberSort.DateOfLastLogin:
                    __itemsQ = __advertsQOrder.OrderBy(zzz => zzz.DateOfLastLogin);
                    break;
                case MemberSort.Email:
                    __itemsQ = __advertsQOrder.OrderBy(zzz => zzz.Email);
                    break;
                case MemberSort.EmailConfirmed:
                    __itemsQ = __advertsQOrder.OrderBy(zzz => zzz.EmailConfirmed);
                    break;
                case MemberSort.DateOfBirth:
                    __itemsQ = __advertsQOrder.OrderBy(zzz => zzz.DateOfBirth);
                    break;
                case MemberSort.County:
                    __itemsQ = __advertsQOrder.OrderBy(zzz => zzz.County);
                    break;
                case MemberSort.Username:
                default:
                    __itemsQ = __advertsQOrder.OrderBy(zzz => zzz.UserName);
                    break;
            }


            var __model = new MemberViewModel();
            __model.Members = await __itemsQ.ToListAsync();
            __model.Counties = new SelectList(await this.Db.Counties.ToListAsync(), "Id", "Name");

            return View(__model);
        }
        
        public async Task<ActionResult> Edit(string id)
        {
            if (null == id)
            {
                return View("Error");
            }

            var __user = await this.UserManager.FindByIdAsync(id);

            if (null == __user)
            {
                return HttpNotFound();
            }

            var __model = new ContactInfoViewModel2(__user);

            __model.Counties = await this.Db
                .Counties
                .Select(zzz => new SelectListItem { Text = zzz.Name, Value = zzz.Id.ToString() })
                .ToListAsync()
                ;

            return View(__model);
        }

        public async Task<ActionResult> Remove(string id)
        {
            if (null == id)
            {
                return RedirectToAction("Index");
            }

            if (this.IsAdmin == true)
            {
                var __roleAdmin = await this.Db
                                            .Roles
                                            .FirstOrDefaultAsync(zzz => zzz.Name == Properties.Resources.DefAdmin)
                                            ;

                // In the system should be at least one administrator
                if (null != __roleAdmin 
                    && 1 == __roleAdmin.Users?.Count 
                    && null !=__roleAdmin.Users?.FirstOrDefault(zzz=>zzz.UserId == id)
                   )
                {
                    return RedirectToAction("Index");
                }
            }

            // Administrator cannot delete himself
            if (this.User.Identity.GetUserId() == id)
            {
                return RedirectToAction("Index");
            }

            var __item = await this.Db.Users.FirstOrDefaultAsync(zzz=> zzz.Id == id);

            if (null == __item)
            {
                return View();
            }

            if(0 < __item.Pets.Count)
            {
                this.Db.Pets.RemoveRange(__item.Pets.ToList());
            }

            this.Db.Users.Remove(__item);
            await this.Db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /// <summary> Show animal pictures fo all users</summary>
        public async Task<ActionResult> PetImages()
        {
            var __model = await this.Db.PetImages
                .AsNoTracking()
                .ToListAsync()
                ;

            return View(__model);
        }

        public async Task<ActionResult> PetImageRemove(int id)
        {
            var __item = await this.Db.PetImages.FindAsync(id);

            if (null == __item)
                return HttpNotFound();

            this.Db.PetImages.Remove(__item);
            await this.Db.SaveChangesAsync();

            return RedirectToAction("PetImages");
        }
    }
}