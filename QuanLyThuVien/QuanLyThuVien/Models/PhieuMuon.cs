using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class PhieuMuon
    {
        public PhieuMuon()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int MaPhieuMuon { get; set; }
        public int? Madocgia { get; set; }
        public int? Manhanvien { get; set; }
        public DateTime? Ngaymuon { get; set; }
        public DateTime? Ngaytra { get; set; }
        public string Ghichu { get; set; } = null!;

        public virtual DocGium? MadocgiaNavigation { get; set; }
        public virtual NhanVien? ManhanvienNavigation { get; set; }
        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
