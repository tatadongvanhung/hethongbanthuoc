namespace api_shop_ban_thuoc_btl_cnltth_2020.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            CHITIETDONHANGs = new HashSet<CHITIETDONHANG>();
        }

        [Key]
        [StringLength(15)]
        public string MaSP { get; set; }

        [StringLength(100)]
        public string TenSP { get; set; }

        [StringLength(100)]
        public string ThanhPhan { get; set; }

        [StringLength(100)]
        public string CongDung { get; set; }

        [StringLength(255)]
        public string LieuLuong { get; set; }

        public int? GiaBan { get; set; }

        public int? MaDM { get; set; }

        [Required]
        [StringLength(100)]
        public string DonVi { get; set; }

        [Required]
        [StringLength(100)]
        public string DangThuoc { get; set; }

        [StringLength(255)]
        public string HinhAnh { get; set; }

        [StringLength(255)]
        public string MoTa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETDONHANG> CHITIETDONHANGs { get; set; }

        public virtual DANHMUC DANHMUC { get; set; }
    }
}
