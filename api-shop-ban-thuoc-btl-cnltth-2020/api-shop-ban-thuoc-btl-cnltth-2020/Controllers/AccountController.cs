using api_shop_ban_thuoc_btl_cnltth_2020.Models;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Account()
        {
            KHACHHANG auth = null;
            auth = (KHACHHANG)Session["LoginKH"];

             return View(auth);

        }
        public ActionResult Dangxuat()
        {

            Session["LoginKH"] = null;
            return RedirectToAction("index", "home");
            //return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DangNhap(KHACHHANG acc)
        {
            TempData["success"] = TempData["error"] = null;
             using (var client = new HttpClient())
             {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var postTask = await client.PostAsJsonAsync<KHACHHANG>("khachhang/check", acc);
                if (postTask.IsSuccessStatusCode)
                {
                    KHACHHANG model = null;
                    var responseTask = await client.GetAsync("khachhang/getkhachhangsdt/" + acc.SDT.ToString());
                    if (responseTask.IsSuccessStatusCode)
                    {
                        model = await responseTask.Content.ReadAsAsync<KHACHHANG>();
                        Session["LoginKH"] = model;
                        TempData["success"] = "Đăng nhập thành công!";                       
                    }
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    TempData["error"] = "Tài khoản hoặc mật khẩu không đúng!";
                    return RedirectToAction("login");
                }
            }

        }
        public ActionResult logincart()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Logincart(KHACHHANG acc)
        {
            TempData["success"] = TempData["error"] = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var postTask = await client.PostAsJsonAsync<KHACHHANG>("khachhang/check", acc);
                if (postTask.IsSuccessStatusCode)
                {
                    KHACHHANG model = null;
                    var responseTask = await client.GetAsync("khachhang/getkhachhangsdt/" + acc.SDT.ToString());
                    if (responseTask.IsSuccessStatusCode)
                    {
                        model = await responseTask.Content.ReadAsAsync<KHACHHANG>();
                        Session["LoginKH"] = model;
                        TempData["success"] = "Đăng nhập thành công!";
                    }
                    return RedirectToAction("payment", "Cart");
                }
                else
                {
                    TempData["error"] = "Tài khoản hoặc mật khẩu không đúng!";
                    return RedirectToAction("logincart");
                }
            }
        }

        //
        public ActionResult Regiter()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Regiter(KHACHHANG kh)
        {
            KHACHHANG regiter = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("khachhang/getkhachhangsdt/" + kh.SDT.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    regiter = await responseTask.Content.ReadAsAsync<KHACHHANG>();
                }
                else
                    regiter = null;
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            if (regiter == null)
            {
                try
                {
                    //HTTP POST
                    using (var client = new HttpClient())
                    {
                        //gọi api thêm khascch hàng
                        client.BaseAddress = new Uri("https://localhost:44373/api/");
                        //Mã hóa
                        kh.MatKhau = BCrypt.Net.BCrypt.HashPassword(kh.MatKhau, 14);
                        var postTask = await client.PostAsJsonAsync<KHACHHANG>("khachhang/addkhachhang", kh);
                        if (postTask.IsSuccessStatusCode)
                        {
                            TempData["success"] = "Đăng ký tài khoản thành công!";
                        }
                        else
                        {
                            TempData["error"] = "Đăng ký tài khoản thất bại!";
                            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                        }

                    }
                }
                catch (Exception ex)
                {
                    //ghi loi
                    return RedirectToAction("Error", "Cart");
                }
            }
            else
            {
                TempData["error"] = "Số điện thoại đã được sử dụng!";
            }
            return View("Regiter");
        }

        [HttpPost]
        public async Task<ActionResult> ChangeInfo(KHACHHANG kh)
        {
            KHACHHANG khachhang = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("khachhang/getkhachhang/" + kh.MaKH.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    khachhang = await responseTask.Content.ReadAsAsync<KHACHHANG>();
                }
                else
                    khachhang = null;
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            if (khachhang.SDT == kh.SDT) // Số điện thoại không thay đổi
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44373/api/");
                    var putTask = await client.PutAsJsonAsync<KHACHHANG>("khachhang/updateKhachHang", kh);
                    if (!putTask.IsSuccessStatusCode)
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                TempData["success"] = "Cập nhật thành công!";
            }
            else
            {
                KHACHHANG sdt = null;
                using (var client = new HttpClient())
                {
                    //gọi api thêm đơn hàng
                    client.BaseAddress = new Uri("https://localhost:44373/api/");
                    var responseTask = await client.GetAsync("khachhang/getkhachhang/" + kh.SDT.ToString());
                    if (responseTask.IsSuccessStatusCode)
                    {
                        sdt = await responseTask.Content.ReadAsAsync<KHACHHANG>();
                    }
                    else
                        sdt = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                if (sdt == null)// nếu số đt chưa được dùng
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:44373/api/");
                        var putTask = await client.PutAsJsonAsync<KHACHHANG>("khachhang/updateKhachHang", kh);
                        if (!putTask.IsSuccessStatusCode)
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                    TempData["success"] = "Cập nhật thành công!";
                }
                else
                {
                    TempData["error"] = "Số điện thoại đã được sử dụng!";
                }
            }
            return RedirectToAction("Account");
        }
        public async Task<ActionResult> ChangePassword(KHACHHANG kh, String matkhaucu)
        {
            KHACHHANG khachhang = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("khachhang/getkhachhang/" + kh.MaKH.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    khachhang = await responseTask.Content.ReadAsAsync<KHACHHANG>();
                }
                else
                    khachhang = null;
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            Boolean check = BCrypt.Net.BCrypt.Verify(matkhaucu, khachhang.MatKhau.Trim());
            if (check)
            {
                kh.MatKhau = BCrypt.Net.BCrypt.HashPassword(kh.MatKhau, 14);
                khachhang.MatKhau = kh.MatKhau;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44373/api/");
                    var putTask = await client.PutAsJsonAsync<KHACHHANG>("khachhang/updateKhachHang", khachhang);
                    if (!putTask.IsSuccessStatusCode)
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                TempData["success"] = "Cập nhật mật khẩu thành công!";
            }
            else
            {
                TempData["error"] = "Mật khẩu hiện tại không đúng!";
            }
            return RedirectToAction("Account");
        }
        
    }
}