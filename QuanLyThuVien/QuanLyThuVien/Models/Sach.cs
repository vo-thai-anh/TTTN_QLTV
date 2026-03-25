using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class Sach
    {
        public Sach()
        {
            SachMuons = new HashSet<SachMuon>();
            TacGias = new HashSet<TacGiac>();
            LoaiSachs = new HashSet<LoaiSach>();
        }

        public int MaSach { get; set; }
        public string Tensach { get; set; } = null!;
        public int? Namxb { get; set; }
        public int? Sotrang { get; set; }
        public string Tomtat { get; set; } = null!;
        public int? Soluong { get; set; }
        public int? Maloai { get; set; }
        public int? Manxb { get; set; }
        public virtual ICollection<LoaiSach> LoaiSachs { get; set; }
        public virtual ICollection<TacGiac> TacGias {   get;set; }
        public virtual NhaXuatBan? ManxbNavigation { get; set; }
        public virtual ICollection<SachMuon> SachMuons { get; set; }
    }
}g
