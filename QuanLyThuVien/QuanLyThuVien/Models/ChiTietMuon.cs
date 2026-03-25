using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class ChiTietMuon
    {
        public ChiTietMuon()
        {
            SachMuons = new HashSet<SachMuon>();
            PhieuTras = new HashSet<PhieuTra>();
        }
        public int Maphieumuon { get; set; }
        public int Masachmuon { get; set; }
        public DateTime? Ngaytrathucte { get; set; }
        public int? Maphieutra { get; set; }
        public virtual ICollection<SachMuon> SachMuons { get; set; }
        public virtual ICollection< PhieuTra> PhieuTras { get; set; }
        public virtual PhieuMuon MaphieumuonNavigation { get; set; } = null!;
    }
}
