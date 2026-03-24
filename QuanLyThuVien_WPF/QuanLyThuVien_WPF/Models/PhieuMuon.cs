using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class PhieuMuon
    {
        public PhieuMuon()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int Maphieumuon { get; set; }
        public int? Madocgia { get; set; }
        public int? Manhanvien { get; set; }
        public DateTime? Ngaymuon { get; set; }
        public DateTime? Ngaytra { get; set; }
        public string Ghichu { get; set; }

        public virtual Docgia MadocgiaNavigation { get; set; }
        public virtual Nhanvien ManhanvienNavigation { get; set; }

        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
