using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IrishPets.Models
{
    public interface IRepositoryPets<TEnt, TPk> : IRepositoryEx<TEnt, TPk> where TEnt : class
    {
        Task<PetKind> GetPetKind_First();
        Task<PetKind> GetPetKind_ById(int id);
        Task<IEnumerable<SelectListItem>> GetPetKinds();

        Task<PetBreed> GetPetBreed_First();
        Task<IEnumerable<PetBreed>> GetPetBreeds_ByKindId(int id);

        Task<IEnumerable<SelectListItem>> GetCounties();


        Member Member { get; set; }
    } // --- IRepositoryPets : interface ---
}