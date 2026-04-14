using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_WPF.Models
{
    public class CSach
    {
        public int MaSach { get; set; }
        public string TenSach { get; set; } = null!;
        public int? NamXb { get; set; }
        public int? SoTrang { get; set; }
        public string? TomTat { get; set; }
        public int? SoLuong { get; set; }
        public int? MaLoai { get; set; }
        public int? MaNxb { get; set; }

        public string? TenLoai { get; set; }
        public List<int>? MaTGIds { get; set; }
    }
}