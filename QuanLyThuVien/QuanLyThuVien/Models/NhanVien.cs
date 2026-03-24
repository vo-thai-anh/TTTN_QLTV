using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            PhieuMuons = new HashSet<PhieuMuon>();
            PhieuTras = new HashSet<PhieuTra>();
        }

        public int MaNv { get; set; }
        public string Hoten { get; set; } = null!;
        public string Chucvu { get; set; } = null!;
        public string Taikhoan { get; set; } = null!;
        public string Matkhau { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Sdt { get; set; } = null!;

        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; }
        public virtual ICollection<PhieuTra> PhieuTras { get; set; }
    }
}
