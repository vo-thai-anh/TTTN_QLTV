using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // BẮT BUỘC PHẢI CÓ DÒNG NÀY ĐỂ DÙNG .Include()
using QLTV_API.Models;
using System.Collections.Generic;
using System.Linq;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SachTacGiaController : ControllerBase
    {
        private QuanLyThuVienContext db = new QuanLyThuVienContext();

        // 1. Lấy danh sách ID Tác giả của 1 cuốn sách
        [HttpGet("{maSach}")]
        public IActionResult GetTacGiaCuaSach(int maSach)
        {
            // Truy vấn trực tiếp từ bảng Sách, xuyên qua danh sách Tác giả của sách đó
            var ids = db.Saches
                        .Where(s => s.MaSach == maSach)
                        .SelectMany(s => s.MaTgs) // Lấy ra danh sách các tác giả
                        .Select(tg => tg.MaTg)    // Chỉ lấy ID
                        .ToList();

            return Ok(ids);
        }

        // 2. Cập nhật danh sách tác giả cho 1 cuốn sách
        [HttpPost("update")]
        public IActionResult UpdateTacGia(SachTacGiaUpdateDTO dto)
        {
            try
            {
                // Bước 1: Tìm cuốn sách đó, đồng thời TẢI LUÔN danh sách tác giả hiện tại của nó (dùng Include)
                var sach = db.Saches
                             .Include(s => s.MaTgs)
                             .FirstOrDefault(s => s.MaSach == dto.MaSach);

                if (sach == null) return NotFound("Không tìm thấy sách");

                // Bước 2: Xóa sạch danh sách tác giả cũ
                sach.MaTgs.Clear();

                // Bước 3: Tìm các object Tác giả mới dựa trên mảng ID truyền vào
                var tacGiasMoi = db.TacGia
                                   .Where(tg => dto.MaTGIds.Contains(tg.MaTg))
                                   .ToList();

                // Bước 4: Thêm các tác giả mới vào cuốn sách
                foreach (var tg in tacGiasMoi)
                {
                    sach.MaTgs.Add(tg);
                }

                // Lưu lại, EF Core sẽ tự động viết lệnh DELETE và INSERT vào bảng ẩn Sach_TacGia ở dưới SQL
                db.SaveChanges();

                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class SachTacGiaUpdateDTO
    {
        public int MaSach { get; set; }
        public List<int> MaTGIds { get; set; }
    }
}