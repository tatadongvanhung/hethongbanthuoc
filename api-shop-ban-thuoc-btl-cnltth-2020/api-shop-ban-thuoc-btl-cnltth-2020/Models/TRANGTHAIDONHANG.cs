namespace api_shop_ban_thuoc_btl_cnltth_2020.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRANGTHAIDONHANG")]
    public partial class TRANGTHAIDONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TRANGTHAIDONHANG()
        {
            DONHANGs = new HashSet<DONHANG>();
        }

        [Key]
        public int MaTT { get; set; }

        [StringLength(255)]
        public string TenTT { get; set; }

        [StringLength(255)]
        public string GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONHANG> DONHANGs { get; set; }
    }
}
