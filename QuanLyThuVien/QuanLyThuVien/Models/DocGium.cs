using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public partial class DocGium
    {
        public DocGium()
        {
            PhieuMuons = new HashSet<PhieuMuon>();
        }

        public int MaDocGia { get; set; }
        public string Hoten { get; set; } = null!;
        public string Diachi { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Sdt { get; set; } = null!;

        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; }
    }
}
