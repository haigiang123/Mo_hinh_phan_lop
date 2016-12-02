using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Tour.Models
{
    public class ReportInfo
    {
        public int MaDoan { get;  set; }
        public int? MaTour { get; set; }
        public string MoTa { get;  set; }
        public DateTime NgayKetThuc { get;  set; }
        public DateTime NgayKhoiHanh { get;  set; }
        public string TenDoan { get;  set; }
        public string TenTour { get;  set; }
        public int TinhTrang { get; set; }
    }


}