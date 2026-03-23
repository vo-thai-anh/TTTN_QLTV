using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class Docgia
    {
        public Docgia()
        {
            PhieuMuons = new HashSet<PhieuMuon>();
        }

        public int Madocgia { get; set; }
        public string Hoten { get; set; }
        public string Diachi { get; set; }
        public string Email { get; set; }
        public string Sdt { get; set; }

        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; }
    }
}
