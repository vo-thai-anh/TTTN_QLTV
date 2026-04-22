using Microsoft.AspNetCore.Authorization;
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
    public class PhieuTrasController : ControllerBase
    {
        private readonly QuanLyThuVienContext _context;

        public PhieuTrasController(QuanLyThuVienContext context)
        {
            _context = context;
        }

        // 1. GET: api/PhieuTras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhieuTra>>> GetPhieuTras()
        {
            return await _context.PhieuTras
                .Include(p => p.MaNhanVienNavigation)
                .ToListAsync();
        }

        // 2. GET: api/PhieuTras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhieuTra>> GetPhieuTra(int id)
        {
            var pt = await _context.PhieuTras
                .Include(p => p.MaNhanVienNavigation)
                .FirstOrDefaultAsync(p => p.MaPhieuTra == id);

            if (pt == null)
                return NotFound("Không tìm thấy phiếu trả.");

            return pt;
        }

        // 3. POST: Thêm mới
        [HttpPost]
        public async Task<ActionResult<PhieuTra>> PostPhieuTra(PhieuTra pt)
        {
            // Reset ID
            pt.MaPhieuTra = 0;

            // --- VALIDATION ---
            if (pt.MaNhanVien == null)
                return BadRequest("Lỗi: Phải chọn nhân viên.");

            if (pt.NgayTra == null)
                return BadRequest("Lỗi: Ngày trả không được để trống.");

            if (pt.TongTienPhat < 0)
                return BadRequest("Lỗi: Tiền phạt không hợp lệ.");

            if (pt.GhiChu?.Length > 200)
                return BadRequest("Lỗi: Ghi chú tối đa 200 ký tự.");

            // --- LƯU ---
            _context.PhieuTras.Add(pt);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPhieuTra), new { id = pt.MaPhieuTra }, pt);
        }

        // 4. PUT: Cập nhật
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhieuTra(int id, PhieuTra pt)
        {
            pt.MaPhieuTra = id;

            if (pt.MaNhanVien == null)
                return BadRequest("Lỗi: Phải chọn nhân viên.");

            _context.Entry(pt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PhieuTras.Any(e => e.MaPhieuTra == id))
                    return NotFound("Không tìm thấy phiếu trả.");
                else throw;
            }

            return Ok("Cập nhật phiếu trả thành công.");
        }

        // 5. DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhieuTra(int id)
        {
            var pt = await _context.PhieuTras.FindAsync(id);

            if (pt == null)
                return NotFound("Không tìm thấy phiếu trả.");

            try
            {
                _context.PhieuTras.Remove(pt);
                await _context.SaveChangesAsync();
                return Ok("Đã xóa phiếu trả thành công.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Lỗi: Phiếu trả này có dữ liệu liên quan, không thể xóa!");
            }
        }

        // 6. SEARCH
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<PhieuTra>>> SearchPhieuTra(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return await _context.PhieuTras.ToListAsync();

            var result = await _context.PhieuTras
                .Where(p => p.GhiChu.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }
    }
}