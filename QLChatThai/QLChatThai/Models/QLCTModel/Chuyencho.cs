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

    [Table("Chuyencho")]
    
    public partial class Chuyencho
    {
        [Key]
        public int id { get; set; }
        public Nullable<int> Ma_xe { get; set; }
        public string Ma_CT { get; set; }
    }
}
