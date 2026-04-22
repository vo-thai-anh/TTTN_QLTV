using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_WPF.Models
{
    public class CSachMuon
    {
        public int MaSachMuon { get; set; }
        public int? MaSach { get; set; }
        public string TinhTrang { get; set; }
        public int? TrangThai { get; set; }

        // Thuộc tính phụ giúp hiển thị chữ trên lưới thay vì hiển thị số
        public string TenTrangThai
        {
            get
            {
                if (TrangThai == 0) return "0 - Sẵn sàng";
                if (TrangThai == 1) return "1 - Đang mượn";
                if (TrangThai == 2) return "2 - Bảo trì";
                if (TrangThai == 3) return "3 - Mất";
                return "Không xác định";
            }
        }
    }
}
