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
    [RoutePrefix("api/chitietdonhang")]
    public class ChiTietDonHangController : ApiController
    {
        //lấy tất cả chi tiết đơn hàng
        [HttpGet]
        [Route("getlistCTdonhang")]
        public IHttpActionResult GetListCTDonHang()
        {
            using (MyDBContext db = new MyDBContext())
            {
                //var model = context.CHITIETDONHANGs.ToList();
                var model = (from x in db.CHITIETDONHANGs select x).ToList();
                return Ok(model);
            }
        }
       
        //lấy theo mã đơn hàng
        [HttpGet]
        [Route("getCTdonhang/{id}")]
        public IEnumerable<CHITIETDONHANG> GetCTDonhang(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.CHITIETDONHANGs.Where(X => X.MaDH == id).ToList();
            }
        }
        [HttpGet]
        [Route("getView/{id}")]
        public IHttpActionResult GetView(int id)
        {
            MyDBContext context = new MyDBContext();
            IEnumerable<VIEWCHITIET> data = context.VIEWCHITIETs.Where(X => X.MaDH == id).ToList();
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data,"TenSP","SoLuong","DonGia","ThanhTien"))
            {
                table.Load(reader);
            }
            return Json(table);
        }
        //
        [HttpGet]
        [Route("getviewchitiet/{id}")]
        public IEnumerable<VIEWCHITIET> GetViewCT(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.VIEWCHITIETs.Where(X => X.MaDH == id).ToList();
            }
        }

        //thêm chi tiết đơn hàng
        [HttpPost]
        [Route("addCTdonhang")]
        public bool ThemCTDonHang(CHITIETDONHANG dh)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                context.CHITIETDONHANGs.Add(dh);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //sửa chi tiết đơn hàng
        [HttpPut]
        [Route("updateCTdonhang")]
        public bool SuaDonHang(CHITIETDONHANG dh)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                CHITIETDONHANG DH = context.CHITIETDONHANGs.Find(dh.MaDH);
                if (DH == null) return false;
                DH.MaSP = dh.MaSP;
                DH.SoLuong = dh.SoLuong;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //xoá chi tiết đơn hàng
        [HttpDelete]
        [Route("delCTdonhang/{id}")]
        public bool DelCTDonHang(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                var dh = context.CHITIETDONHANGs.Find(id);
                if (dh == null)
                    return false;
                else
                    context.CHITIETDONHANGs.Remove(dh);
                context.SaveChanges();
                return true;
            }
        }
    }
}