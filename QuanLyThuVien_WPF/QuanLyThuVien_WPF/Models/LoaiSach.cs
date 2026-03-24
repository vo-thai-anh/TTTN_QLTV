using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models
{
    public partial class Loaisach
    {
        public Loaisach()
        {
            Saches = new HashSet<Sach>();
        }

        public int Maloai { get; set; }
        public string Tenloai { get; set; }
        public string Mota { get; set; }

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
