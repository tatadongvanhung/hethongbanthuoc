using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using api_shop_ban_thuoc_btl_cnltth_2020.Models;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Controllers
{
    public class DonHangController : Controller
    {
        //private MyDBContext db = new MyDBContext();
        // GET: DonHang //Get listdanhsachdonhang

        public ActionResult DonHang()
        {
            var account = (KHACHHANG)Session["LoginKH"];
            IEnumerable<VIEWDONHANG> model = null;
            if (account != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44373/api/");
                    //HTTP GET
                    var responseTask = client.GetAsync("donhang/dsdonhangKH/"+account.MaKH);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<VIEWDONHANG>>();
                        readTask.Wait();

                        model = readTask.Result;
                    }
                    else 
                    {
    

                        model = Enumerable.Empty<VIEWDONHANG>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                }

                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        public ActionResult ChiTietDonHang(int id)
        {
            IEnumerable<VIEWCHITIET> model = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("chitietdonhang/getviewchitiet/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<VIEWCHITIET>>();
                    readTask.Wait();

                    model = readTask.Result;
                }
                else 
                {
                    model = Enumerable.Empty<VIEWCHITIET>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(model);
        }
    }
}