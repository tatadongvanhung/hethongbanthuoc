using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using api_shop_ban_thuoc_btl_cnltth_2020.Models;
using api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Models;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Controllers
{
    public class LoginController : Controller
    {
        MyDBContext Context = new MyDBContext();

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(Account acc)
        {
            
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44373/api/");
                    var responseTask = await client.PostAsJsonAsync<Account>("quantri/login", acc);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var result = await responseTask.Content.ReadAsAsync<TAIKHOANQUANTRI>();
                        if (result != null)
                        {
                            acc.Role = new ROLE();
                            //foreach (ROLE it in result.ROLES)
                            //{
                            //    acc.Roles.Add(it.RoleName);
                            //}
                            acc.Role = result.ROLE1;
                            acc.HoTen = result.HoTen;
                            acc.MaQT = result.MaQT;
                            Session["Login"] = acc;
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            TempData["Login"] = "Tài khoản hoặc mật khẩu không đúng!";
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return View();
                    }
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult Exit()
        {
            Session["Login"] = null;
            return RedirectToAction("Login");
        }

    }
}
