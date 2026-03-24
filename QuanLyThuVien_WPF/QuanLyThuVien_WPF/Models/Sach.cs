using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class Sach
    {
        public Sach()
        {
            SachMuons = new HashSet<SachMuon>();
        }

        public int Masach { get; set; }
        public string Tensach { get; set; }
        public int? Namxb { get; set; }
        public int? Sotrang { get; set; }
        public string Tomtat { get; set; }
        public int? Soluong { get; set; }
        public int? Maloai { get; set; }
        public int? Manxb { get; set; }

        public virtual Loaisach MaloaiNavigation { get; set; }
        public virtual Nhaxuatban ManxbNavigation { get; set; }

        public virtual ICollection<SachMuon> SachMuons { get; set; }
    }
}
