using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLChatThai.Models.QLCTModel
{
    public class NhapDetail
    {
        public int idctnhap { get; set; }
        public int sophieu { get; set; }

        public string Tenct { get; set; }
        public string dvt { get; set; }
        public string trangthai { get; set; }
        public string phuongphap { get; set; }
        public bool loai { get; set; }
        public Nullable<float> slthuc { get; set; }
        public Nullable<double> dongianhap { get; set; }
    }
}