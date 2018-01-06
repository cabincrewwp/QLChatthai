using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLChatThai.Models.QLCTModel
{
    public class CTLapCT
    {
        public string Code_KH { get; set; }
        public string TenCT { get; set; }
        public string MaQLCTNH { get; set; }
        public string dvt { get; set; }
        public string trangthai { get; set; }
        public string phuongphapxl { get; set; }
        public bool tuxuly { get; set; }
        public double soluong { get; set; }
    }
}