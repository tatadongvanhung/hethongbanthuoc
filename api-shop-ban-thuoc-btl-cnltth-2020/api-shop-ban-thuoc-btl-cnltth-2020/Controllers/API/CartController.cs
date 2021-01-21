using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using api_shop_ban_thuoc_btl_cnltth_2020.Models;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Controllers.API
{
    [RoutePrefix("api/giohang")]
    public class CartController : ApiController
    {
        //lấy theo mã khachhang
        //[HttpGet]
        //[Route("getgiohang/{id}")]
        //public GIOHANG GetGioHang(int id)
        //{
        //    using (MyDBContext context = new MyDBContext())
        //    {
        //        return context.GIOHANGs.Where(X=>X.MaKH==id).FirstOrDefault();
        //    }
        //}

        //[HttpGet]
        //[Route("getctgiohang/{id}")]
        //public IEnumerable<CHITIETGIOHANG> GetCTGioHang(int id)
        //{
        //    using (MyDBContext context = new MyDBContext())
        //    {
        //        return context.CHITIETGIOHANGs.Where(X => X.MaGioHang == id).ToList();
        //    }
        //}

        //[HttpGet]
        //[Route("getctgiohang1/{id}")]
        //public IEnumerable<CHITIETGIOHANG> GetCTGioHang1(int id)
        //{
        //    using (MyDBContext context = new MyDBContext())
        //    { 
        //        int GioHangID =context.GIOHANGs.Where(X => X.MaKH == id).FirstOrDefault().MaGioHang;
        //        return context.CHITIETGIOHANGs.Where(X => X.MaGioHang == GioHangID).ToList();
        //    }
        //}


        //[HttpPost]
        //[Route("addgiohang")]
        //public bool ThemGioHang(CHITIETGIOHANG ct)
        //{
        //    try
        //    {
        //        MyDBContext context = new MyDBContext();
        //        context.CHITIETGIOHANGs.Add(ct);
        //        context.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //[HttpPut]
        //[Route("updategiohang")]
        //public bool SuaGioHang(int id, CHITIETGIOHANG ct)
        //{
        //    try
        //    {
        //        MyDBContext context = new MyDBContext();
        //        CHITIETGIOHANG DM = context.CHITIETGIOHANGs.Find(id);
        //        if (DM == null) return false;
        //        DM = ct;
        //        DM.MaGioHang = id;
        //        context.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //[HttpDelete]
        //[Route("delgiohang/{id}")]
        //public bool DelDanhMuc(int id)
        //{
        //    using (MyDBContext context = new MyDBContext())
        //    {
        //        var dm = context.CHITIETGIOHANGs.Find(id);
        //        if (dm == null)
        //            return false;
        //        else
        //            context.CHITIETGIOHANGs.Remove(dm);
        //        context.SaveChanges();
        //        return true;
        //    }
        //}
    }
}