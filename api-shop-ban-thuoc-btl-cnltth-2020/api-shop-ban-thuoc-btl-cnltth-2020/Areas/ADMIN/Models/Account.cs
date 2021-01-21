using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using api_shop_ban_thuoc_btl_cnltth_2020.Models;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Areas.ADMIN.Models
{
    
    public class Account
    {
        public int MaQT { get; set; }
        public string HoTen { get; set; }
        public string MatKhau { get; set; }
        public string SDT { get; set; }
        public ROLE Role { get; set; }
    }
}