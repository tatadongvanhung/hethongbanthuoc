using System.Web;
using System.Web.Mvc;

namespace api_shop_ban_thuoc_btl_cnltth_2020
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
