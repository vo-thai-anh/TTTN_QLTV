using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class SachMuon
    {
        public SachMuon()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int MaSachMuon { get; set; }
        public int? Masach { get; set; }
        public string Tinhtrang { get; set; } = null!;

        public virtual Sach? MasachNavigation { get; set; }
        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
