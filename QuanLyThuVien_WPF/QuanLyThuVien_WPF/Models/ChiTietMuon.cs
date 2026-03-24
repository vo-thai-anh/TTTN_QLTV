using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class ChiTietMuon
    {
        public int Maphieumuon { get; set; }
        public int Masachmuon { get; set; }
        public DateTime? Ngaytrathucte { get; set; }
        public int? Maphieutra { get; set; }

        public virtual PhieuMuon MaphieumuonNavigation { get; set; }
        public virtual SachMuon MasachmuonNavigation { get; set; }
        public virtual PhieuTra MaphieutraNavigation { get; set; }
    }
}
