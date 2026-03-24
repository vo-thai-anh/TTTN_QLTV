using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class PhieuTra
    {
        public PhieuTra()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int MaPhieuTra { get; set; }
        public int? Manhanvien { get; set; }
        public DateTime? Ngaytra { get; set; }
        public string Ghichu { get; set; } = null!;

        public virtual NhanVien? ManhanvienNavigation { get; set; }
        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
