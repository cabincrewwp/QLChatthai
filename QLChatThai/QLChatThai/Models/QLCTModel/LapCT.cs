using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLChatThai.Models.QLCTModel
{
    public class LapCT
    {
        public Khachhang khachhang { set; get; }
        public DateTime tu_ngay { set; get; }
        public DateTime toi_ngay { set; get; }
        public List<CTLapCT> CTlapct {set; get; } 
    }
}