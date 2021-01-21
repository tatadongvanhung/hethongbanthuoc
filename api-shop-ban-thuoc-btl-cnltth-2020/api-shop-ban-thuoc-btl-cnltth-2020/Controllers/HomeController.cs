using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

using api_shop_ban_thuoc_btl_cnltth_2020.Models;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Controllers
{
    public class HomeController : Controller
    {

        // Display view page Home: index
        public ActionResult Index()
        {
            IEnumerable<SANPHAM> model = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("product/getlistthuoc");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<SANPHAM>>();
                    readTask.Wait();

                    model = readTask.Result;
                }
                else 
                {


                    model = Enumerable.Empty<SANPHAM>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(model);
        }


       

        // Display view page product details
        public ActionResult Product(String id)
        {
            //return View();
            SANPHAM model = null;
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("product/getProductbyID/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<SANPHAM>();
                    readTask.Wait();

                    model = readTask.Result;
                }
                else 
                {

                    model = null;

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            //Trả về sản phẩm này

            ViewBag.SP = model;
            // Lấy danh sách sản phẩm cùng loại


            IEnumerable<SANPHAM> sp = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("product/getProductCategoryExceptID/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<SANPHAM>>();
                    readTask.Wait();

                    sp = readTask.Result;
                }
                else 
                {


                    sp = Enumerable.Empty<SANPHAM>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(sp);
            
            //return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(String search)
        {
            IEnumerable<SANPHAM> model = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("product/searchthuoc/" + search);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<SANPHAM>>();
                    readTask.Wait();

                    model = readTask.Result;
                }
                else
                {
                    model = Enumerable.Empty<SANPHAM>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(model);
        }

        public ActionResult Category(int id)
        {
            IEnumerable<SANPHAM> model = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("product/getProductbyIDcategory/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<SANPHAM>>();
                    readTask.Wait();

                    model = readTask.Result;
                }
                else 
                {
                    model = Enumerable.Empty<SANPHAM>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View("Index", model);
        }
        //Display view category medicinew
        public ActionResult _Slidebar()
        {
            IEnumerable<DANHMUC> model = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("danhmuc/getlistdanhmuc");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<DANHMUC>>();
                    readTask.Wait();

                    model = readTask.Result;
                }
                else 
                {


                    model = Enumerable.Empty<DANHMUC>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(model);
        }

    }
}