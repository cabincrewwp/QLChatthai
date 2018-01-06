using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLChatThai.Models.QLCTModel
{
    public class Chungtu
    {
        public string  Code_KH { get; set; }
        public string Ten_KH { get; set; }
        public string Ma_QLCTNH { get; set; }
        public string Ten_CT { get; set; }
        public int idct { get; set; }
        public string dvt { get; set; }
        public string trangthai { get; set; }
        public int idxl { get; set; }
        public string phuongphap { get; set; }
        public Nullable<float> soluong { get; set; }
        //public Nullable<double> Dongianhap { get; set; }

    }
}