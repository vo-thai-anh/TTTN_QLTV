using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QLTV_API.Models
{
    public partial class Sach
    {
        public Sach()
        {
            SachMuons = new HashSet<SachMuon>();
            MaTgs = new HashSet<TacGia>();
        }

        public int MaSach { get; set; }
        public string TenSach { get; set; } = null!;
        public int? NamXb { get; set; }
        public int? SoTrang { get; set; }
        public string? TomTat { get; set; }
        public int? SoLuong { get; set; }
        public int? MaNxb { get; set; }
        public int? MaLoai { get; set; }

        [JsonIgnore]
        public virtual LoaiSach? MaLoaiNavigation { get; set; }
        [JsonIgnore]
        public virtual NhaXuatBan? MaNxbNavigation { get; set; }

        [JsonIgnore]
        public virtual ICollection<SachMuon> SachMuons { get; set; }

        [JsonIgnore]
        public virtual ICollection<TacGia> MaTgs { get; set; }
    }
}