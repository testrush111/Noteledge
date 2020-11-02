using System.Web;
using System.Web.Mvc;

namespace prjMSIT127_G2_Noteledge
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
