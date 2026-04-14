using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QLTV_API.Models
{
    public partial class DocGia
    {
        public DocGia()
        {
            PhieuMuons = new HashSet<PhieuMuon>();
        }

        [Key] // Xác định đây là khóa chính
        public int MaDocGia { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [MaxLength(100)]
        public string HoTen { get; set; } = null!;

        [MaxLength(200)]
        public string? DiaChi { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ")]
        public string? Email { get; set; }

        [MaxLength(15)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? Sdt { get; set; }

        [JsonIgnore]
        public virtual ICollection<PhieuMuon> PhieuMuons { get; set; }
    }
}