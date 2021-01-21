using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using api_shop_ban_thuoc_btl_cnltth_2020.Models;
using FastMember;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Controllers.API
{
    [RoutePrefix("api/donhang")]
    public class DonHangController : ApiController
    {
        //lấy tất cả danh mục
        // GET: api/danhmuc
        [HttpGet]
        [Route("getlistviewdonhang")]
        public IEnumerable<VIEWDONHANG> GetListsDanhMuc()
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.VIEWDONHANGs.ToList();
            }
        }
        [HttpGet]
        [Route("getView")]
        public IHttpActionResult GetView()
        {
            MyDBContext context = new MyDBContext();
            IEnumerable<VIEWDONHANG> data = context.VIEWDONHANGs.ToList();
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data, "MaDH", "HoTen", "NgayLap", "TongTien"))
            {
                table.Load(reader);
            }
            return Json(table);




        }
        //lấy tất đơn hàng của khách hàng có mã
        // GET: api/danhmuc
        [HttpGet]
        [Route("dsdonhangKH/{makh}")]
        public IEnumerable<VIEWDONHANG> GetListsDHKhachHang(int makh)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.VIEWDONHANGs.Where(x => x.MaKH == makh).ToList();
            }
        }

        [HttpGet]
        [Route("getIDdonhang")]
        public int GetIDDonHang()
        {
            using (MyDBContext context = new MyDBContext())
            {
                var DonHang = context.DONHANGs.ToList();
                int ID = DonHang[0].MaDH;
                foreach (var it in DonHang)
                {
                    if (it.MaDH > ID)
                        ID = it.MaDH;
                }
                return ID;
            }
        }


        //lấy theo mã đơn hàng
        [HttpGet]
        [Route("getdonhang/{id}")]
        public DONHANG GetDonhang(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.DONHANGs.Find(id);
            }
        }

        //thêm đơn hàng
        [HttpPost]
        [Route("adddonhang")]
        public bool ThemDonHang(DONHANG donhang)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                context.DONHANGs.Add(donhang);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //sửa đơn hàng
        [HttpPut]
        [Route("updatedonhang")]
        public bool SuaDonHang(DONHANG donhang)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                DONHANG DH = context.DONHANGs.Find(donhang.MaDH);
                if (DH == null) return false;
                //DH.MaKH = donhang.MaKH;
                //DH.NgayLap = donhang.NgayLap;
                DH.TrangThai = donhang.TrangThai;
                DH.TongTien = donhang.TongTien;
                //DH.GhiChu = donhang.GhiChu;
                DH.HoTen= donhang.HoTen;
                DH.Email = donhang.Email;
                DH.Diachi = donhang.Diachi;
                //DH.SDT = donhang.SDT;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //sửa trạng thái đơn hàng
        [HttpPut]
        [Route("updateTTdonhang/{status}")]
        public bool SuaTTDonHang(int status, DONHANG donhang)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                DONHANG DH = context.DONHANGs.Find(donhang.MaDH);
                if (DH == null) return false;
                DH.TrangThai = status;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //xoá đơn hàng + xoá các chi tiết đơn liên quan
        [HttpDelete]
        [Route("deldonhang/{id}")]
        public bool DelDonHang(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                var dh = context.DONHANGs.Find(id);
                if (dh == null)
                    return false;
                else
                {
                    var ctdh = context.CHITIETDONHANGs.Where(X => X.MaDH == id).ToList();
                    foreach (var it in ctdh)
                    {
                        context.CHITIETDONHANGs.Remove(it);
                    }
                    context.DONHANGs.Remove(dh);
                    context.SaveChanges();
                    return true;
                }

            }
        }
    }
}