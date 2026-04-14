using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTV_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocGiasController : ControllerBase
    {
        private readonly QuanLyThuVienContext _context;

        public DocGiasController(QuanLyThuVienContext context)
        {
            _context = context;
        }

        // 1. GET: api/DocGias - Lấy toàn bộ danh sách
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocGia>>> GetDocGias()
        {
            return await _context.DocGia.ToListAsync();
        }

        // 2. GET: api/DocGias/5 - Xem chi tiết theo Mã (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<DocGia>> GetDocGia(int id)
        {
            var docGia = await _context.DocGia.FindAsync(id);
            if (docGia == null) return NotFound("Không tìm thấy độc giả.");
            return docGia;
        }

        // 3. POST: api/DocGias - Thêm mới với ràng buộc từ Controller
        [HttpPost]
        public async Task<ActionResult<DocGia>> PostDocGia(DocGia docGia)
        {
            // --- KIỂM TRA RÀNG BUỘC (VALIDATION) ---

            // Mã Độc Giả: Vì là int Identity nên ta để SQL tự tăng, reset về 0 để tránh lỗi
            docGia.MaDocGia = 0;

            // Họ Tên: Mandatory (M), Max 100 chars
            if (string.IsNullOrWhiteSpace(docGia.HoTen))
                return BadRequest("Lỗi: Họ tên không được để trống (M).");
            if (docGia.HoTen.Length > 100)
                return BadRequest("Lỗi: Họ tên tối đa 100 ký tự.");

            // Địa Chỉ: Max 200 chars
            if (docGia.DiaChi?.Length > 200)
                return BadRequest("Lỗi: Địa chỉ tối đa 200 ký tự.");

            // Email: Max 100 chars
            if (docGia.Email?.Length > 100)
                return BadRequest("Lỗi: Email tối đa 100 ký tự.");

            // SĐT: Max 15 chars (Sử dụng AnyAsync để kiểm tra duy nhất nếu An muốn)
            if (docGia.Sdt?.Length > 15)
                return BadRequest("Lỗi: Số điện thoại tối đa 15 ký tự.");

            // --- LƯU VÀO DATABASE ---
            _context.DocGia.Add(docGia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocGia), new { id = docGia.MaDocGia }, docGia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocGia(int id, DocGia docGia)
        {
            // Thay vì báo lỗi nếu không khớp, mình "ép" ID trong JSON phải theo ID trên URL
            docGia.MaDocGia = id;

            // Kiểm tra ràng buộc Họ tên như cũ
            if (string.IsNullOrWhiteSpace(docGia.HoTen))
                return BadRequest("Lỗi: Họ tên không được để trống.");

            _context.Entry(docGia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DocGia.Any(e => e.MaDocGia == id)) return NotFound("Không tìm thấy độc giả.");
                else throw;
            }

            return Ok("Cập nhật thông tin thành công.");
        }

        // 5. DELETE: api/DocGias/5 - Xóa thẻ
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocGia(int id)
        {
            var docGia = await _context.DocGia.FindAsync(id);
            if (docGia == null) return NotFound("Không tìm thấy độc giả.");

            try
            {
                _context.DocGia.Remove(docGia);
                await _context.SaveChangesAsync();
                return Ok("Đã xóa độc giả thành công.");
            }
            catch (DbUpdateException)
            {
                // Bắt lỗi ràng buộc khóa ngoại (Ràng buộc 1.1.2)
                return BadRequest("Lỗi: Độc giả này đang có phiếu mượn sách, không thể xóa!");
            }
        }

        // 6. GET: api/DocGias/Search?keyword=an - Tìm kiếm tên không dấu hoặc SĐT
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<DocGia>>> SearchDocGia(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return await _context.DocGia.ToListAsync();

            var result = await _context.DocGia
                .Where(d => EF.Functions.Collate(d.HoTen, "SQL_Latin1_General_CP1_CI_AI").Contains(keyword)
                         || d.Sdt.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }
    }
}