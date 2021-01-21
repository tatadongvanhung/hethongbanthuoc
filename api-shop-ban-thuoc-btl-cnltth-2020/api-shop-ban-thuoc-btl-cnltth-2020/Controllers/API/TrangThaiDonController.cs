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
    [RoutePrefix("api/TTDH")]
    public class TrangThaiDonController : ApiController
    {
        [HttpGet]
        [Route("getlistTTDH")]
        public IEnumerable<TRANGTHAIDONHANG> GetThuocLists()
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.TRANGTHAIDONHANGs.ToList();
            }
        }
        [HttpGet]
        [Route("getView")]
        public IHttpActionResult GetView()
        {
            MyDBContext context = new MyDBContext();
            IEnumerable<TRANGTHAIDONHANG> data = context.TRANGTHAIDONHANGs.ToList();
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data, "MaTT", "TenTT"))
            {
                table.Load(reader);
            }
            return Json(table);
        }
    }
}
