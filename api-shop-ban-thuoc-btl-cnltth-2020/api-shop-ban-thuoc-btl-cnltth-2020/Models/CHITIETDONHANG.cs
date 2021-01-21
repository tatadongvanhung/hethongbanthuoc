namespace api_shop_ban_thuoc_btl_cnltth_2020.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CHITIETDONHANG")]
    public partial class CHITIETDONHANG
    {
        [Key]
        public int ID_CT { get; set; }

        public int MaDH { get; set; }

        [Required]
        [StringLength(15)]
        public string MaSP { get; set; }

        public int? SoLuong { get; set; }

        public int? DonGia { get; set; }

        public int? ThanhTien { get; set; }

        public virtual DONHANG DONHANG { get; set; }

        public virtual SANPHAM SANPHAM { get; set; }
    }
}
