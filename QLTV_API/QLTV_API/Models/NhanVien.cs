using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QLTV_API.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            PhieuMuons = new HashSet<PhieuMuon>();
            PhieuTras = new HashSet<PhieuTra>();
        }

        public int MaNv { get; set; }
        public string HoTen { get; set; } = null!;
        public string? ChucVu { get; set; }
        public string? TaiKhoan { get; set; }
        public string? MatKhau { get; set; }
        public string? Email { get; set; }
        public string? Sdt { get; set; }
        [JsonIgnore]
        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; }
        [JsonIgnore]
        public virtual ICollection<PhieuTra> PhieuTras { get; set; }
    }
}
