using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QLTV_API.Models
{
    public partial class DocGia
    {
        public DocGia()
        {
            PhieuMuons = new HashSet<PhieuMuon>();
        }

        public int MaDocGia { get; set; }

        public string HoTen { get; set; } = null!;

        public string? DiaChi { get; set; }

        public string? Email { get; set; }

        public string Sdt { get; set; }

        [JsonIgnore]
        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; }

        // PhieuTra không có FK trực tiếp đến DocGia trong DB
        // → đánh dấu NotMapped để EF Core không tạo shadow property 'DocGiaMaDocGia'
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<PhieuTra>? PhieuTras { get; set; }
    }
}