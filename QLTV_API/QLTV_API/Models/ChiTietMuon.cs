using System;
using System.Collections.Generic;

namespace QLTV_API.Models
{
    public partial class ChiTietMuon
    {
        public int MaPhieuMuon { get; set; }
        public int MaSachMuon { get; set; }
        public DateTime? NgayTraThucTe { get; set; }
        public decimal? TienPhat { get; set; }
        public string? LyDoPhat { get; set; }
        public int? MaPhieuTra { get; set; }

        public virtual PhieuMuon MaPhieuMuonNavigation { get; set; } = null!;
        public virtual PhieuTra? MaPhieuTraNavigation { get; set; }
        public virtual SachMuon MaSachMuonNavigation { get; set; } = null!;
    }
}
