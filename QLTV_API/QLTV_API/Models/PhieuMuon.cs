using System;
using System.Collections.Generic;

namespace QLTV_API.Models
{
    public partial class PhieuMuon
    {
        public PhieuMuon()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int MaPhieuMuon { get; set; }
        public int? MaDocGia { get; set; }
        public int? MaNhanVien { get; set; }
        public DateTime? NgayMuon { get; set; }
        public DateTime? NgayTra { get; set; }
        public string? GhiChu { get; set; }

        public virtual DocGium? MaDocGiaNavigation { get; set; }
        public virtual NhanVien? MaNhanVienNavigation { get; set; }
        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
