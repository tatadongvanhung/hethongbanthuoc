using api_shop_ban_thuoc_btl_cnltth_2020.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
      

        // GET: Cart
        //private MyDBContext context = new MyDBContext();       //
        // GET: /Cart/

        public ActionResult Cart()
        {
            var cart = (Cart)Session["CartSession"];
            if (cart == null)
            {
                cart = new Cart();
            }
            return View(cart);
        }



        public ActionResult AddItem(string id, string returnURL)
        {
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

                    var cart = (Cart)Session["CartSession"];
                    if (cart != null)
                    {
                        cart.AddItem(model);
                        //Gán vào session
                        Session["CartSession"] = cart;
                    }
                    else
                    {
                        //tạo mới đối tượng cart item
                        cart = new Cart();
                        cart.AddItem(model);
                        //Gán vào session
                        Session["CartSession"] = cart;
                    }
                }
                else
                {
                    model = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            if (string.IsNullOrEmpty(returnURL))
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        //

        // GET: /Cart/Details/5
        public ActionResult RemoveLine(string id)
        {

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

                    var cart = (Cart)Session["CartSession"];
                    if (cart != null)
                    {
                        cart.RemoveLine(model);
                        if (cart.Lines.Count() == 0)
                        {
                            cart = null;
                        }
                        //Gán vào session
                        Session["CartSession"] = cart;
                    }
                }
                else 
                {
                    model = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return RedirectToAction("Cart");
            
        }

        public ActionResult UpdateCart(string[] masp, int[] qty)
        {
            var cart = (Cart)Session["CartSession"];
            if (cart != null)
            {
                for (int i = 0; i < masp.Count(); i++)
                {
                    SANPHAM model = null;
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:44373/api/");
                        //HTTP GET
                        var responseTask = client.GetAsync("product/getProductbyID/" + masp[i]);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsAsync<SANPHAM>();
                            readTask.Wait();
                            model = readTask.Result;

                            cart.UpdateItem(model, qty[i]);
                        }
                        else 
                        {
                            model = null;
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        }
                    }
                }
                Session["CartSession"] = cart;
            }

            return RedirectToAction("Cart");

        }


        //
        // GET: /Cart/Details/5
        [HttpGet]
        public ActionResult Payment()
        {
            var account = (KHACHHANG)Session["LoginKH"];
            var cart = (Cart)Session["CartSession"];

            if (account == null)
            {
                return RedirectToAction("Logincart", "Account");
            }

            if (cart == null)
            {
                cart = new Cart();
            }
            ViewBag.account = account;
            return View(cart);
        }

        [HttpPost]
        public async Task<ActionResult> Payment(DONHANG model)
        {
            var account = (KHACHHANG)Session["LoginKH"];
            
            try
            {
                model.TrangThai = 1; // Trạng thái chưa xác nhận
                model.MaKH = account.MaKH;
                model.NgayLap = DateTime.Now;

                //gọi api thêm đơn hàng

                //HTTP POST
                using (var client = new HttpClient())
                {
                    //gọi api thêm đơn hàng
                    client.BaseAddress = new Uri("https://localhost:44373/api/");

                    var postTask = await client.PostAsJsonAsync<DONHANG>("donhang/adddonhang", model);
                    if (postTask.IsSuccessStatusCode)
                    {
                        //Lấy ID đơn hàng vừa thêm
                        var responseTask = await client.GetAsync("donhang/getIDdonhang");
                        int madh = await responseTask.Content.ReadAsAsync<int>();
                        var cart = (Cart)Session["CartSession"];

                        foreach (var item in cart.Lines)
                        {
                            CHITIETDONHANG obj = new CHITIETDONHANG();
                            obj.MaDH = madh;
                            obj.MaSP = item.Thuoc.MaSP;
                            obj.DonGia = item.Thuoc.GiaBan;
                            obj.SoLuong = item.Quantity;
                            obj.ThanhTien = item.ThanhTien;
                            //gọi api thêm chi tiết đơn
                            var postTask1 = await client.PostAsJsonAsync<CHITIETDONHANG>("chitietdonhang/addCTdonhang", obj);
                            if (!postTask1.IsSuccessStatusCode)
                                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                        }
                        cart.Clear();
                        Session["CartSession"] = cart;
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }
            }
            catch (Exception ex)
            {
                //ghi log
                return RedirectToAction("Error");
            }
            return View("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

    }
}
