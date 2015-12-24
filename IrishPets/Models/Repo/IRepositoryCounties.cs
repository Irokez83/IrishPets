using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public interface IRepositoryCounties<TEnt, TPk> : IRepositoryEx<TEnt, TPk> where TEnt : class
    {
        Task<IEnumerable<SelectListItem>> GetCounties();
    } // --- IRepositoryCounties : interface ---
}