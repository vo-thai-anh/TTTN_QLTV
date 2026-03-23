using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class LoaiSach
    {
        public LoaiSach()
        {
            Saches = new HashSet<Sach>();
        }

        public int MaLoai { get; set; }
        public string Tenloai { get; set; } = null!;
        public string Mota { get; set; } = null!;

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
