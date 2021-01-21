using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using api_shop_ban_thuoc_btl_cnltth_2020.Models;
using FastMember;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Controllers.API
{
    [RoutePrefix("api/khachhang")]
    public class KhachHangController : ApiController
    {
        [HttpPost]
        [Route("check")]
        public IHttpActionResult Check(KHACHHANG kh)
        {
            using (MyDBContext context = new MyDBContext())
            {
                KHACHHANG model = context.KHACHHANGs.Where(X => X.SDT == kh.SDT).FirstOrDefault();
                if (model != null)
                {
                    Boolean check = BCrypt.Net.BCrypt.Verify(kh.MatKhau, model.MatKhau.Trim());
                    if (check)
                        return Ok();
                    else
                        return NotFound();
                }
                else
                    return NotFound();
            }
        }
        //lấy tất cả khach hang
        [HttpGet]
        [Route("getlistkhachhang")]
        public IEnumerable<KHACHHANG> GetListsKhachHang()
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.KHACHHANGs.ToList();
            }
        }
        [HttpGet]
        [Route("getView")]
        public IHttpActionResult GetView()
        {
            MyDBContext context = new MyDBContext();
            IEnumerable<KHACHHANG> data = context.KHACHHANGs.ToList();
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data, "MaKH", "HoTen"))
            {
                table.Load(reader);
            }
            return Json(table);
        }
        //lấy theo mã khachhang
        [HttpGet]
        [Route("getkhachhang/{id}")]
        public KHACHHANG GetKhachHang(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.KHACHHANGs.Find(id);
            }
        }

        [HttpGet]
        [Route("getkhachhangsdt/{sdt}")]
        public KHACHHANG GetKhachHangsdt(string sdt)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.KHACHHANGs.Where(x=>x.SDT==sdt).FirstOrDefault();
            }
        }

        [HttpPost]
        [Route("addkhachhang")]
        public bool ThemKhachHang(KHACHHANG dc)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                context.KHACHHANGs.Add(dc);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPut]
        [Route("updateKhachHang")]
        public bool SuaKhachHang(KHACHHANG dc)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                KHACHHANG DC = context.KHACHHANGs.Find(dc.MaKH);
                if (DC == null) return false;
                DC.HoTen = dc.HoTen;
                DC.SDT = dc.SDT;
                DC.Email = dc.Email;
                DC.MatKhau = dc.MatKhau;
                DC.Diachi = dc.Diachi;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete]
        [Route("delKhachHang/{id}")]
        public bool DelKhachHang(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                var dc = context.KHACHHANGs.Find(id);
                if (dc == null)
                    return false;
                else
                {
                    context.KHACHHANGs.Remove(dc);
                    context.SaveChanges();
                    return true;
                }

            }
        }

    }
}