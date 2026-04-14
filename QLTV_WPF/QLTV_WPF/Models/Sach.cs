using System;
using System.Collections.Generic;

namespace QLTV_WPF.Models
{
    public partial class Sach
    {
        public Sach()
        {
            SachMuons = new HashSet<SachMuon>();
            MaTgs = new HashSet<TacGia>();
        }

        public int MaSach { get; set; }
        public string TenSach { get; set; } = null!;
        public int? NamXb { get; set; }
        public int? SoTrang { get; set; }
        public string? TomTat { get; set; }
        public int? SoLuong { get; set; }
        public int? MaLoai { get; set; }
        public int? MaNxb { get; set; }
        public string? TenLoai { get; set; }

        public virtual LoaiSach? MaLoaiNavigation { get; set; }
        public virtual NhaXuatBan? MaNxbNavigation { get; set; }
        public virtual ICollection<SachMuon> SachMuons { get; set; }

        public virtual ICollection<TacGia> MaTgs { get; set; }
    }
}
