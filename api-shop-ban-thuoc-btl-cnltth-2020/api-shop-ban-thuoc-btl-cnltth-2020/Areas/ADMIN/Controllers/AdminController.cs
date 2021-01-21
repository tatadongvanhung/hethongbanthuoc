using api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Models;
using api_shop_ban_thuoc_btl_cnltth_2020.Models;
using api_shop_ban_thuoc_btl_cnltth_2020.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Controllers
{
    
    public class AdminController : Controller
    {
        private MyDBContext context = new MyDBContext();
        public ActionResult _DangNhap()
        {
            var acc = (Account)Session["Login"];
            ViewBag.Username = acc.HoTen;
            ViewBag.ID = acc.MaQT;
            return View();
        }
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> HoSo(int ID)
        {
            TAIKHOANQUANTRI model = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("quantri/getTKQT/"+ID.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<TAIKHOANQUANTRI>();
                }
                else
                {
                    model = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> HoSo(TAIKHOANQUANTRI model, string NewPass, string Confirm)
        {
            TAIKHOANQUANTRI obj = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("quantri/getTKQT/" + model.MaQT.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    obj = await responseTask.Content.ReadAsAsync<TAIKHOANQUANTRI>();
                }
                else
                {
                    obj = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            if (obj.MatKhau.Contains(model.MatKhau))
            {
                model.MatKhau = NewPass;
                if (NewPass.CompareTo(Confirm) == 0)
                {
                    using (var client = new HttpClient())
                    {
                        var putTask = await client.PutAsJsonAsync<TAIKHOANQUANTRI>("danhmuc/updateTKQT", model);
                        if (!putTask.IsSuccessStatusCode)
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator."); 
                    }
                    return RedirectToAction("Users");
                }
                else
                {
                    TempData["Alert"] = "Xác nhận mật khẩu không đúng! Vui lòng thử lại!";
                    return View("HoSo", obj);
                }
            }
            else
            {
                TempData["Alert"] = "Mật khẩu không đúng! Vui lòng thử lại!";
                return View("HoSo", obj);
            }
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        public ActionResult Index()
        {
            int[] a = new int[4];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = client.GetAsync("quantri/getSL");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int[]>();
                    readTask.Wait();
                    a = readTask.Result;
                }
                else
                {
                    a = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            ViewBag.SLThanhVien = a[0];
            ViewBag.SLSanPham = a[1];
            ViewBag.SLDanhMuc = a[2];
            ViewBag.SLDonHang = a[3];
            return View();
        }

        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Users()
        {
            IEnumerable<TAIKHOANQUANTRI> model = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("quantri/getlistTKQT");
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<IList<TAIKHOANQUANTRI>>();
                }
                else
                {
                    model = Enumerable.Empty<TAIKHOANQUANTRI>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return View(model);
        }
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Add_User()
        {
            IEnumerable<ROLE> model1 = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("role/getlistrole");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<ROLE>>();
                    readTask.Wait();

                    model1 = readTask.Result;
                }
                else
                {
                    model1 = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(model1);
        }
        [HttpPost]
        public async Task<ActionResult> Add_User(TAIKHOANQUANTRI model, string reMatKhau, int ROLE)
        {
            try
            {
                if (model.MatKhau.CompareTo(reMatKhau) == 0)
                {
                    model.Role = ROLE;
                    using (var client = new HttpClient())
                    {
                        //gọi api thêm đơn hàng
                        client.BaseAddress = new Uri("https://localhost:44373/api/");
                        var responseTask = await client.PostAsJsonAsync<TAIKHOANQUANTRI>("quantri/addTKQT", model);
                        if (!responseTask.IsSuccessStatusCode)
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                    return RedirectToAction("Users");
                }
                else
                {
                    TempData["Alert"] = "Xác nhận mật khẩu không đúng! Vui lòng thử lại!";
                    return View();
                }

            }
            catch
            {
                TempData["Alert"] = "SDT đã tồn tại! Vui lòng thử lại!";
                return View();
            }

        }
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Edit_User(int ID)
        {
            IEnumerable<ROLE> model1 = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("role/getlistrole");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<ROLE>>();
                    readTask.Wait();

                    model1 = readTask.Result;
                }
                else
                {
                    model1 = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            ViewBag.role = model1;

            TAIKHOANQUANTRI model = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                //HTTP GET
                var responseTask = client.GetAsync("quantri/getTKQT/" + ID.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<TAIKHOANQUANTRI>();
                    readTask.Wait();
                    model = readTask.Result;
                }
                else
                {
                    model = null;
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(model);
        }
        [HttpPost]
       public async Task<ActionResult> Edit_User(TAIKHOANQUANTRI model, int ROLE)
        {
            TAIKHOANQUANTRI obj = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("quantri/getTKQT/" + model.MaQT.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    obj = await responseTask.Content.ReadAsAsync<TAIKHOANQUANTRI>();                   
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            obj.Role = ROLE;
            using (var client =new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var putTask = await client.PutAsJsonAsync<TAIKHOANQUANTRI>("quantri/updateTKQT", obj);
                if (!putTask.IsSuccessStatusCode)
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return RedirectToAction("Users");

        }
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Del_User(string ID)
        {
            //TAIKHOANQUANTRI model = null;

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://localhost:44373/api/");
            //    //HTTP GET
            //    var responseTask = client.GetAsync("quantri/getTKQT/" + ID.ToString());
            //    responseTask.Wait();
            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<TAIKHOANQUANTRI>();
            //        readTask.Wait();

            //        model = readTask.Result;
            //    }
            //    else
            //    {
            //        model = null;

            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            //model.ROLES.Clear();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var putTask = await client.DeleteAsync("quantri/delTKQT/" + ID.ToString());
                if (!putTask.IsSuccessStatusCode)
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return RedirectToAction("Users");
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        [HttpPost]
        public ActionResult TimKiemUsers(string search)
        {
            IEnumerable<TAIKHOANQUANTRI> model = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = client.GetAsync("quantri/searchTKQT/"+search.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<TAIKHOANQUANTRI>>();
                    readTask.Wait();
                    model = readTask.Result;
                }
                else
                {
                    model = Enumerable.Empty<TAIKHOANQUANTRI>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            ViewBag.search = search;
            return View("Users", model);
        }

        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> DanhMuc()
        {
            IEnumerable<DANHMUC> model = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("danhmuc/getlistdanhmuc");
                if (responseTask.IsSuccessStatusCode)
                {
                    var readTask = responseTask.Content.ReadAsAsync<IList<DANHMUC>>();
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
        [CustomAuthorize(Roles = "Admin,Member")]
        public ActionResult Add_DanhMuc()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Add_DanhMuc(DANHMUC model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //gọi api thêm đơn hàng
                    client.BaseAddress = new Uri("https://localhost:44373/api/");
                    var responseTask = await client.PostAsJsonAsync<DANHMUC>("danhmuc/adddanhmuc", model);
                    if (!responseTask.IsSuccessStatusCode)
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                return RedirectToAction("DanhMuc");
            }
            catch
            {
                TempData["Alert"] = "Đã xảy ra lỗi. Có thể Mã danh mục đã tồn tại, Vui lòng thử lại!";
                return View("Add_DanhMuc");
            }

        }
        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> Edit_DanhMuc(int ID)
        {
            DANHMUC model = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("danhmuc/getdanhmuc/" + ID);
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<DANHMUC>();
                }
                else
                    model = null;
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit_DanhMuc(DANHMUC model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var putTask = await client.PutAsJsonAsync<DANHMUC>("danhmuc/updatedanhmuc", model);
                if (!putTask.IsSuccessStatusCode)
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return RedirectToAction("DanhMuc");
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> Del_DanhMuc(int ID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var putTask = await client.DeleteAsync("danhmuc/deldanhmuc/" + ID.ToString());
                if (!putTask.IsSuccessStatusCode)
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return RedirectToAction("DanhMuc");
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        [HttpPost]
        public async Task<ActionResult> TimKiemDanhMuc(string search)
        {
            IEnumerable<DANHMUC> model = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("danhmuc/searchdanhmuc/" + search);
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<IList<DANHMUC>>();
                }
                else
                    model = Enumerable.Empty<DANHMUC>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            ViewBag.search = search;
            return View("DanhMuc", model);
        }



        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> SanPham()
        {
            IEnumerable<SANPHAM> model = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("product/getlistthuoc");
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<IList<SANPHAM>>();
                }
                else
                    model = Enumerable.Empty<SANPHAM>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(model);
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        [HttpPost]
        public async Task<ActionResult> TimKiemSP(string search)
        {
            SANPHAM model = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("product/searchthuoc/"+search.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<SANPHAM>();
                }
                else
                    model = null;
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            ViewBag.search = search;
            return View("SanPham", model);
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        public ActionResult Add_SanPham()
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
        [HttpPost]
        public async Task<ActionResult> Add_SanPham(HttpPostedFileBase file, SANPHAM model)
        {

            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    // lấy tên tệp tin
                    var fileName = Path.GetFileName(file.FileName);
                    // lưu trữ tệp tin vào folder ~/App_Data/uploads
                    var path = Path.Combine(Server.MapPath("~/Content/assets/img/"), fileName);
                    file.SaveAs(path);
                    model.HinhAnh = fileName;
                    using (var client = new HttpClient())
                    {
                        //gọi api thêm đơn hàng
                        client.BaseAddress = new Uri("https://localhost:44373/api/");
                        var responseTask = await client.PostAsJsonAsync<SANPHAM>("product/addthuoc", model);
                        if (!responseTask.IsSuccessStatusCode)
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                    return RedirectToAction("SanPham");
                }
                else
                {
                    return View("Add_SanPham");
                }
            }
            catch
            {
                return View("Add_SanPham");
            }


        }
        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> Del_SanPham(string ID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var putTask = await client.DeleteAsync("product/delthuoc/" + ID.ToString());
                if (!putTask.IsSuccessStatusCode)
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return RedirectToAction("SanPham");
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> Edit_SanPham(string ID)
        {
            SANPHAM model = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("product/getProductbyID/"+ID.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<SANPHAM>();
                }
                else
                    model = null;
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit_SanPham(HttpPostedFileBase file, SANPHAM model)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    // lấy tên tệp tin
                    var fileName = Path.GetFileName(file.FileName);
                    // lưu trữ tệp tin vào folder ~/App_Data/uploads
                    var path = Path.Combine(Server.MapPath("~/Content/assets/img/"), fileName);
                    file.SaveAs(path);
                    model.HinhAnh = fileName;
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:44373/api/");
                        var putTask = await client.PutAsJsonAsync<SANPHAM>("product/updatethuoc", model);
                        if (!putTask.IsSuccessStatusCode)
                            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }
                    return RedirectToAction("SanPham");
                }
                else
                {
                    return View("Edit_SanPham",model);
                }
            }
            catch
            {
                return View("Edit_SanPham",model);
            }
        }
        //Đơn Hàng
        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> DonHang()
        {
            IEnumerable<VIEWDONHANG> model = null;
            using (var client = new HttpClient())
            {
                //gọi api thêm đơn hàng
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("donhang/getlistviewdonhang");
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<IList<VIEWDONHANG>>();
                }
                else
                    model = Enumerable.Empty<VIEWDONHANG>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(model);
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> ChiTietDonHang(int ID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("TTDH/getlistTTDH");
                if (responseTask.IsSuccessStatusCode)
                {
                    IEnumerable<TRANGTHAIDONHANG> ttdh = await responseTask.Content.ReadAsAsync<IEnumerable<TRANGTHAIDONHANG>>();
                    ViewBag.ttdh = ttdh;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            IEnumerable<VIEWCHITIET> model = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("chitietdonhang/getviewchitiet/" + ID.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    model = await responseTask.Content.ReadAsAsync<IList<VIEWCHITIET>>();
                }
                else
                    model = Enumerable.Empty<VIEWCHITIET>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            DONHANG modelDH = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("donhang/getdonhang/" + ID.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    modelDH = await responseTask.Content.ReadAsAsync<DONHANG>();
                }
                else
                    modelDH = null;
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            ViewBag.MaDonHang = ID;
            ViewBag.TinhTrang = modelDH.TRANGTHAIDONHANG;
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ChiTietDonHang(int TRANGTHAIDONHANG, int ID)
        {
            DONHANG obj = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var responseTask = await client.GetAsync("donhang/getdonhang/" + ID.ToString());
                if (responseTask.IsSuccessStatusCode)
                {
                    obj = await responseTask.Content.ReadAsAsync<DONHANG>();
                }
                else
                    obj = null;
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            obj.TrangThai = TRANGTHAIDONHANG;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var putTask = await client.PutAsJsonAsync<DONHANG>("donhang/updatedonhang", obj);
                if (!putTask.IsSuccessStatusCode)
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return RedirectToAction("DonHang");
        }
        [CustomAuthorize(Roles = "Admin,Member")]
        public async Task<ActionResult> Del_DonHang(int ID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var putTask = await client.DeleteAsync("chitietdonhang/delCTdonhang/" + ID.ToString());
                if (!putTask.IsSuccessStatusCode)
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44373/api/");
                var putTask = await client.DeleteAsync("donhang/deldonhang/" + ID.ToString());
                if (!putTask.IsSuccessStatusCode)
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return RedirectToAction("DonHang");
        }
        //[CustomAuthorize(Roles = "Admin,Member")]
        //[HttpPost]
        //public ActionResult TimKiemDH(string search)
        //{
        //    var model = context.DONHANGs.Where(X => X.Email.Contains(search) || X.SDT.Contains(search)
        //    || X.TinhTrang.Contains(search) || X.TenKhachHang.Contains(search)).ToList();
        //    ViewBag.search = search;
        //    return View("DonHang", model);
        //}
    }
}
