namespace api_shop_ban_thuoc_btl_cnltth_2020.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VIEWCHITIET")]
    public partial class VIEWCHITIET
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaDH { get; set; }

        public int? MaKH { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string MaSP { get; set; }

        [StringLength(100)]
        public string TenSP { get; set; }

        [StringLength(255)]
        public string HinhAnh { get; set; }

        public int? SoLuong { get; set; }

        public int? DonGia { get; set; }

        public int? ThanhTien { get; set; }
    }
}
