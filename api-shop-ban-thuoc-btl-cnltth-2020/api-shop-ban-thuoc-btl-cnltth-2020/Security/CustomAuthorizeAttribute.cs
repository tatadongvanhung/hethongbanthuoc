using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Models;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (HttpContext.Current == null)
            {
                filterContext.Result = new RedirectResult("/ADMIN/Login/Login");
                return;
            }
            var acc = (Account)HttpContext.Current.Session["Login"];

            if (acc == null)
            {
                filterContext.Result = new RedirectResult("/ADMIN/Login/Login");
            }
            else
            {
                var cp = new CustomPrincipal(acc);
                if (!cp.IsInRole(Roles))
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(
                            new { Controller = "Login", Action = "Login" }));
                }
            }
        }
    }
}