using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QLTV_WPF.Models
{
    public partial class DocGia
    {
        public DocGia()
        {
            PhieuMuons = new HashSet<PhieuMuon>();
        }

        public int MaDocGia { get; set; }
        public string HoTen { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? Email { get; set; }
        public string Sdt { get; set; }

        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; }
        public virtual ICollection<PhieuTra>? PhieuTras { get; set; }
    }
}
