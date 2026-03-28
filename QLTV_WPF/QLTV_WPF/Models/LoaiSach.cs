using System;
using System.Collections.Generic;

namespace QLTV_WPF.Models
{
    public partial class LoaiSach
    {
        public LoaiSach()
        {
            Saches = new HashSet<Sach>();
        }

        public int MaLoai { get; set; }
        public string TenLoai { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
