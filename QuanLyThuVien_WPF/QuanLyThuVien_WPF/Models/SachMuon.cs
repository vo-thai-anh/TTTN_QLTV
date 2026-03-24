using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class SachMuon
    {
        public SachMuon()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int Masachmuon { get; set; }
        public int? Masach { get; set; }
        public string Tinhtrang { get; set; }

        public virtual Sach MasachNavigation { get; set; }

        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
