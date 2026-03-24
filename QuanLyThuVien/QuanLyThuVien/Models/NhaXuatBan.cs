using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class NhaXuatBan
    {
        public NhaXuatBan()
        {
            Saches = new HashSet<Sach>();
        }

        public int MaNxb { get; set; }
        public string Tennxb { get; set; } = null!;
        public string Diachi { get; set; } = null!;
        public string Sdt { get; set; } = null!;

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
