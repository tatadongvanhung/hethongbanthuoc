namespace api_shop_ban_thuoc_btl_cnltth_2020.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TAIKHOANQUANTRI")]
    public partial class TAIKHOANQUANTRI
    {
        [Key]
        public int MaQT { get; set; }

        [StringLength(255)]
        public string HoTen { get; set; }

        [Required]
        [StringLength(20)]
        public string SDT { get; set; }

        [Required]
        [StringLength(100)]
        public string MatKhau { get; set; }

        public int Role { get; set; }

        public virtual ROLE ROLE1 { get; set; }
    }
}
