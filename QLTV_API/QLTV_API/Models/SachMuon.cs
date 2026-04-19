using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QLTV_API.Models
{
    public partial class SachMuon
    {
        public SachMuon()
        {
            ChiTietMuons = new HashSet<ChiTietMuon>();
        }

        public int MaSachMuon { get; set; }
        public int? MaSach { get; set; }
        public string? TinhTrang { get; set; }
        public int? TrangThai { get; set; }

        [JsonIgnore]
        public virtual Sach? MaSachNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<ChiTietMuon> ChiTietMuons { get; set; }
    }
}
