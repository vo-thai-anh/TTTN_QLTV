using System;
using System.Collections.Generic;

namespace QLTV_WPF.Models
{
    public partial class TacGia
    {
        public TacGia()
        {
            MaSaches = new HashSet<Sach>();
        }

        public int MaTg { get; set; }
        public string TenTg { get; set; } = null!;
        public string? TieuSu { get; set; }

        public virtual ICollection<Sach> MaSaches { get; set; }
    }
}
