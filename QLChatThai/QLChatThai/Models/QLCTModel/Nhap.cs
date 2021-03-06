//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLChatThai.Models.QLCTModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Nhap")]
    
    public partial class Nhap
    {
        public Nhap()
        {
            this.CTNHaps = new HashSet<CTNHap>();
        }
        [Key]
        public int sophieu { get; set; }
        public string Sochungtu { get; set; }
        public string Code_KH { get; set; }
        public int Makho { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        //[Required(ErrorMessage = "Date is required")]
        
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ngaynhan { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]

        //[Required(ErrorMessage = "Date is required")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ngaynhap { get; set; }
        public string NV_Gioithieu1 { get; set; }
        public string NV_Gioithieu2 { get; set; }
        public string NV_Gioithieu3 { get; set; }
        public string NV_KDMua1 { get; set; }
        public string NV_KDMua2 { get; set; }
        public string NV_KDMua3 { get; set; }
        public string Ghichu { get; set; }
        public string status { get; set; }
        public string manv { get; set; }
        public string Soxe { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        //public string Kinhdoanh {
        //    get
        //    {
        //        return NV_KDMua1 + "\r" + NV_KDMua2 + "\r" + NV_KDMua3;
        //    }        
        //}
        //public string Moigioi {
        //    get
        //    {
        //        return NV_Gioithieu1 + "<br/>" + NV_Gioithieu2 + "<br/>" + NV_Gioithieu3;
        //    }
        //}
        public virtual ICollection<CTNHap> CTNHaps { get; set; }
        public virtual DMKHO DMKHO { get; set; }
        public virtual DMXe DMXe { get; set; }
        public virtual Khachhang Khachhang { get; set; }
        public virtual Nhanvien Nhanvien { get; set; }
    }
}
