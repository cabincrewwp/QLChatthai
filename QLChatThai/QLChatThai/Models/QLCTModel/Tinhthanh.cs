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

    [Table("Tinhthanh")]
    
    public partial class Tinhthanh
    {
        public Tinhthanh()
        {
            this.Khachhangs = new HashSet<Khachhang>();
        }
        [Key]
        public int Ma_tinh { get; set; }
        public int Ma_KV { get; set; }
        public string Ten { get; set; }
    
        public virtual ICollection<Khachhang> Khachhangs { get; set; }
        public virtual Khuvuc Khuvuc { get; set; }
    }
}
