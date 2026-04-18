using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTV_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        // 1. GET - Lấy toàn bộ danh sách
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocGia>>> GetDocGias()
        {
            return await _context.DocGia.ToListAsync();
        }

        // 2. GET - Xem chi tiết theo Mã (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<DocGia>> GetDocGia(int id)
        {
            var docGia = await _context.DocGia.FindAsync(id);
            if (docGia == null) return NotFound("Không tìm thấy độc giả.");
            return docGia;
        }

        // 3. POST
        [HttpPost]
        public async Task<ActionResult<DocGia>> PostDocGia(DocGia docGia)
        {
            docGia.MaDocGia = 0;

            // 1. Kiểm tra SĐT (Phải đúng 10 số)
            if (!Regex.IsMatch(docGia.Sdt ?? "", @"^\d{10}$"))
                return BadRequest("Lỗi: Số điện thoại phải đúng 10 chữ số.");

            // 2. Kiểm tra Họ tên (Không ký tự lạ)
            string patternTen = @"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ\s]+$";
            if (!Regex.IsMatch(docGia.HoTen ?? "", patternTen))
                return BadRequest("Lỗi: Họ tên không được chứa số hoặc ký tự đặc biệt.");

            _context.DocGia.Add(docGia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocGia), new { id = docGia.MaDocGia }, docGia);
        }

        // 4. PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocGia(int id, DocGia docGia)
        {
            if (id != docGia.MaDocGia) docGia.MaDocGia = id;

            if (!Regex.IsMatch(docGia.Sdt ?? "", @"^\d{10}$"))
                return BadRequest("Lỗi: Số điện thoại phải đúng 10 chữ số.");

            if (string.IsNullOrWhiteSpace(docGia.HoTen))
                return BadRequest("Lỗi: Họ tên không được để trống.");

            docGia.Email = string.IsNullOrWhiteSpace(docGia.Email) ? null : docGia.Email;

            _context.Entry(docGia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.DocGia.Any(e => e.MaDocGia == id)) return NotFound();
                else throw;
            }
            return Ok("Cập nhật thông tin thành công.");
        }

        // 5. DELETE:
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocGia(int id)
        {
            // kiểm tra phiếu mượn
            bool daTungMuonSach = await _context.PhieuMuons.AnyAsync(p => p.MaDocGia == id);

            if (daTungMuonSach)
            {
                return BadRequest("Không thể xóa độc giả này!");
            }

            // Nếu chưa từng mượn gì 
            var docGia = await _context.DocGia.FindAsync(id);
            if (docGia == null) return NotFound("Không tìm thấy độc giả này.");

            _context.DocGia.Remove(docGia);
            await _context.SaveChangesAsync();

            return Ok("Đã xóa độc giả thành công.");
        }
        // 6. GET - Tìm kiếm tên không dấu hoặc SĐT
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