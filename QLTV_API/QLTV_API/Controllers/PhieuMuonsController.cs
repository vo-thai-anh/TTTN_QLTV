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
    public class PhieuMuonsController : ControllerBase
    {
        private readonly QuanLyThuVienContext _context;

        public PhieuMuonsController(QuanLyThuVienContext context)
        {
            _context = context;
        }

        // 1. GET: api/PhieuMuons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhieuMuon>>> GetPhieuMuons()
        {
            return await _context.PhieuMuons
                .Include(p => p.MaDocGiaNavigation)
                .Include(p => p.MaNhanVienNavigation)
                .ToListAsync();
        }

        // 2. GET: api/PhieuMuons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhieuMuon>> GetPhieuMuon(int id)
        {
            var pm = await _context.PhieuMuons
                .Include(p => p.MaDocGiaNavigation)
                .Include(p => p.MaNhanVienNavigation)
                .FirstOrDefaultAsync(p => p.MaPhieuMuon == id);

            if (pm == null)
                return NotFound("Không tìm thấy phiếu mượn.");

            return pm;
        }

        // 3. POST: Thêm mới
        [HttpPost]
        public async Task<ActionResult<PhieuMuon>> PostPhieuMuon(PhieuMuon pm)
        {
            // Reset ID (tránh lỗi identity)
            pm.MaPhieuMuon = 0;

            // VALIDATION
            if (pm.MaDocGia == null)
                return BadRequest("Phải chọn độc giả.");

            if (pm.MaNhanVien == null)
                return BadRequest("Phải chọn nhân viên.");

            if (pm.NgayMuon == null)
                return BadRequest("Ngày mượn không được để trống.");

            // Nếu chưa nhập ngày trả → để null OK

            _context.PhieuMuons.Add(pm);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPhieuMuon), new { id = pm.MaPhieuMuon }, pm);
        }

        // 4. PUT: Cập nhật
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhieuMuon(int id, PhieuMuon pm)
        {
            pm.MaPhieuMuon = id;

            if (pm.MaDocGia == null)
                return BadRequest("Phải chọn độc giả.");

            _context.Entry(pm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PhieuMuons.Any(e => e.MaPhieuMuon == id))
                    return NotFound("Không tìm thấy phiếu mượn.");
                else throw;
            }

            return Ok("Cập nhật phiếu mượn thành công.");
        }

        // 5. DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhieuMuon(int id)
        {
            var pm = await _context.PhieuMuons.FindAsync(id);

            if (pm == null)
                return NotFound("Không tìm thấy phiếu mượn.");

            try
            {
                _context.PhieuMuons.Remove(pm);
                await _context.SaveChangesAsync();
                return Ok("Đã xóa phiếu mượn.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thể xóa vì đã có chi tiết mượn liên quan.");
            }
        }

        // 6. SEARCH (tuỳ chọn)
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<PhieuMuon>>> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return await _context.PhieuMuons.ToListAsync();

            var result = await _context.PhieuMuons
                .Where(p => p.GhiChu.Contains(keyword))
                .ToListAsync();

            return Ok(result);
        }
    }
}