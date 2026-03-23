using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class ChiTietMuon
    {
        public int Maphieumuon { get; set; }
        public int Masachmuon { get; set; }
        public DateTime? Ngaytrathucte { get; set; }
        public int? Maphieutra { get; set; }

        public virtual PhieuMuon MaphieumuonNavigation { get; set; } = null!;
        public virtual PhieuTra? MaphieutraNavigation { get; set; }
        public virtual SachMuon MasachmuonNavigation { get; set; } = null!;
    }
}
