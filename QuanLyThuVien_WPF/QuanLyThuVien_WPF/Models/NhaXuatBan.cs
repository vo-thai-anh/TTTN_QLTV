using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class Nhaxuatban
    {
        public Nhaxuatban()
        {
            Saches = new HashSet<Sach>();
        }

        public int Manxb { get; set; }
        public string Tennxb { get; set; }
        public string Diachi { get; set; }
        public string Sdt { get; set; }

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
