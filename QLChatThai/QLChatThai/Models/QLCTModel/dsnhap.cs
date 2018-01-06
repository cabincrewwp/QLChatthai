using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLChatThai.Models.QLCTModel
{
    public class dsnhap
    {
        public int sophieu { get; set; }
        public string Sochungtu { get; set; }
        public string Code_KH { get; set; }
        public string khachhang { get; set; }
        public string kho { get; set; }

        public Nullable<System.DateTime> ngaynhan { get; set; }

        public Nullable<System.DateTime> ngaynhap { get; set; }
        public string NV_Gioithieu1 { get; set; }
        public string NV_Gioithieu2 { get; set; }
        public string NV_Gioithieu3 { get; set; }
        public string NV_KDMua1 { get; set; }
        public string NV_KDMua2 { get; set; }
        public string NV_KDMua3 { get; set; }
        public string Ghichu { get; set; }
        public string status { get; set; }
        public string nguoinhan { get; set; }
        public string Soxe { get; set; }
        
    }
}