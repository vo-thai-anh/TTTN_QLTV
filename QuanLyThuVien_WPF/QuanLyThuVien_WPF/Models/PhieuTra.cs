using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class PhieuTra
    {
        public PhieuTra()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int Maphieutra { get; set; }
        public int? Manhanvien { get; set; }
        public DateTime? Ngaytra { get; set; }
        public string Ghichu { get; set; }

        public virtual Nhanvien ManhanvienNavigation { get; set; }

        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
