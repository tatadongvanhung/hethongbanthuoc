using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api_shop_ban_thuoc_btl_cnltth_2020.Models;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Controllers.API
{
    [RoutePrefix("api/role")]
    public class ROLEController : ApiController
    {
        // GET: api/ROLE
        [Route("getlistrole")]
        [HttpGet]
        public IEnumerable<ROLE> Get()
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.ROLES.ToList();
            }
        }

        // GET: api/ROLE/5
        [Route("getrole/{id}")]
        [HttpGet]
        public ROLE GetRole(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.ROLES.Where(X => X.IDRole == id).FirstOrDefault();
            }
        }

        // POST: api/ROLE
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ROLE/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ROLE/5
        public void Delete(int id)
        {
        }
    }
}
