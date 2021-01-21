using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_shop_ban_thuoc_btl_cnltth_2020.Models;
using api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Models;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Controllers.API
{
    [RoutePrefix("api/quantri")]
    public class TaiKhoanQuanTriController : ApiController
    {
        [HttpPost]
        [Route("login1")]
        public TAIKHOANQUANTRI Login1(TAIKHOANQUANTRI acc)
        {
            TAIKHOANQUANTRI ac = new TAIKHOANQUANTRI();
            using (MyDBContext context = new MyDBContext())
            {
                var result = context.TAIKHOANQUANTRIs.Where(a => a.SDT.Equals(acc.SDT) &&
                                       a.MatKhau.Equals(acc.MatKhau)).FirstOrDefault();
                if (result != null)
                {
                    ac.MaQT = result.MaQT;
                    ac.SDT = result.SDT;
                    ac.MatKhau = result.MatKhau;
                    ac.HoTen = result.HoTen;
                    ac.Role = result.Role;
                    //foreach (ROLE it in result.ROLES)
                    //{
                    //    ac.Roles.Add(it.RoleName);
                    //}
                    return ac;
                }
                else
                    return null;
            }
        }

        [HttpGet]
        [Route("getTKQT/{id}")]
        public TAIKHOANQUANTRI GetListsDanhMuc(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.TAIKHOANQUANTRIs.Where(x => x.MaQT == id).FirstOrDefault();
            }
        }
        [HttpGet]
        [Route("getRoleTKQT/{id}")]
        public ROLE GetRoleTKQT(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.ROLES.Where(x => x.IDRole == id).FirstOrDefault();
            }
        }
        [HttpGet]
        [Route("getlistTKQT")]
        public IEnumerable<TAIKHOANQUANTRI> GetListsTKQT()
        {
            using (MyDBContext context = new MyDBContext())
            {
                var model = context.TAIKHOANQUANTRIs.Include(b => b.ROLE1).ToList();
                return model;
            }
        }

        [HttpGet]
        [Route("searchTKQT/{search}")]
        public IEnumerable<TAIKHOANQUANTRI> searchTKQT(string search)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.TAIKHOANQUANTRIs.Where(X => X.HoTen.Contains(search)).Include(b => b.ROLE1).ToList();
            }
        }

        [HttpPost]
        [Route("addTKQT")]
        public bool ThemQuanTri(TAIKHOANQUANTRI dc)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                context.TAIKHOANQUANTRIs.Add(dc);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPut]
        [Route("updateTKQT")]
        public bool UpdateQuanTri(TAIKHOANQUANTRI dc)
        {
            try
            {
                MyDBContext context = new MyDBContext();
                var QT = context.TAIKHOANQUANTRIs.Find(dc.MaQT);
                if (QT == null)
                    return false;
                else
                {
                    QT.Role = dc.Role;
                    QT.HoTen = dc.HoTen;
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        [HttpDelete]
        [Route("delTKQT/{id}")]
        public bool DeleteTKQT(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                var thuoc = context.TAIKHOANQUANTRIs.Find(id);
                if (thuoc == null)
                    return false;
                else
                    context.TAIKHOANQUANTRIs.Remove(thuoc);
                context.SaveChanges();
                return true;
            }
        }
        [HttpPost]
        [Route("login")]
        public TAIKHOANQUANTRI Login(Account acc)
        {
            using (MyDBContext context = new MyDBContext())
            {
                var result = context.TAIKHOANQUANTRIs.Where(a => a.SDT.Equals(acc.SDT) &&
                                       a.MatKhau==acc.MatKhau).Include(x=>x.ROLE1).FirstOrDefault();
                if (result != null)
                    return result;
                else
                    return null;
            }       
        }
        [HttpGet]
        [Route("getSL")]
        public int[] GetSL()
        {
            using (MyDBContext context = new MyDBContext())
            {
                int[] a = new int[4];
                a[0] = context.TAIKHOANQUANTRIs.Count();
                a[1] = context.SANPHAMs.Count();
                a[2] = context.DANHMUCs.Count();
                a[3] = context.DONHANGs.Count();
                return a;
            }
        }

        [HttpGet]
        [Route("getRole/{id}")]
        public ROLE GetRole(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                var model = context.ROLES.Where(x => x.IDRole == id).FirstOrDefault();
                return model;
            }
        }

    }
}
