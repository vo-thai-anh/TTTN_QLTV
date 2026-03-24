using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class Nhanvien
    {
        public Nhanvien()
        {
            PhieuMuons = new HashSet<PhieuMuon>();
            PhieuTras = new HashSet<PhieuTra>();
        }

        public int Manv { get; set; }
        public string Hoten { get; set; }
        public string Chucvu { get; set; }
        public string Taikhoan { get; set; }
        public string Matkhau { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }

        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; }
        public virtual ICollection<PhieuTra> PhieuTras { get; set; }
    }
}
