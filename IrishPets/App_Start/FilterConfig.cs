using System.Web.Mvc;

namespace IrishPets
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection _filters)
        {
            _filters.Add(new HandleErrorAttribute());
        }
    }
}
