using System;
using System.Collections.Generic;

namespace QLTV_API.Models
{
    public partial class PhieuTra
    {
        public PhieuTra()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int MaPhieuTra { get; set; }
        public int? MaNhanVien { get; set; }
        public DateTime? NgayTra { get; set; }
        public decimal? TongTienPhat { get; set; }
        public string? GhiChu { get; set; }

        public virtual NhanVien? MaNhanVienNavigation { get; set; }
        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
