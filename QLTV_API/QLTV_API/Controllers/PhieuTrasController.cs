using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTV_API.Models;
using System;
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

        // Helper: chỉ lấy các trường scalar, KHÔNG load navigation properties
        private static PhieuTra ToFlat(PhieuTra p) => new PhieuTra
        {
            MaPhieuTra = p.MaPhieuTra,
            MaNhanVien = p.MaNhanVien,
            NgayTra = p.NgayTra,
            TongTienPhat = p.TongTienPhat,
            GhiChu = p.GhiChu
        };

        // 1. GET: api/PhieuTras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhieuTra>>> GetPhieuTras()
        {
            try
            {
                // Dùng Select() để chỉ lấy scalar fields — tuyệt đối không load navigation
                var list = await _context.PhieuTras
                    .Select(p => new PhieuTra
                    {
                        MaPhieuTra = p.MaPhieuTra,
                        MaNhanVien = p.MaNhanVien,
                        NgayTra = p.NgayTra,
                        TongTienPhat = p.TongTienPhat,
                        GhiChu = p.GhiChu
                    })
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tải phiếu trả: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // 2. GET: api/PhieuTras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhieuTra>> GetPhieuTra(int id)
        {
            try
            {
                var pt = await _context.PhieuTras
                    .Where(p => p.MaPhieuTra == id)
                    .Select(p => new PhieuTra
                    {
                        MaPhieuTra = p.MaPhieuTra,
                        MaNhanVien = p.MaNhanVien,
                        NgayTra = p.NgayTra,
                        TongTienPhat = p.TongTienPhat,
                        GhiChu = p.GhiChu
                    })
                    .FirstOrDefaultAsync();

                if (pt == null)
                    return NotFound("Không tìm thấy phiếu trả.");

                return Ok(pt);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // 3. POST: Thêm mới
        [HttpPost]
        public async Task<ActionResult<PhieuTra>> PostPhieuTra([FromBody] PhieuTra pt)
        {
            try
            {
                // Đặt lại ID để DB tự sinh (identity)
                pt.MaPhieuTra = 0;

                // Xóa navigation để EF không cố insert entity liên quan
                pt.MaNhanVienNavigation = null;
                pt.ChiTietMuons?.Clear();

                // Validation
                if (pt.MaNhanVien == null)
                    return BadRequest("Lỗi: Phải chọn nhân viên.");

                if (pt.NgayTra == null)
                    return BadRequest("Lỗi: Ngày trả không được để trống.");

                if (pt.TongTienPhat.HasValue && pt.TongTienPhat < 0)
                    return BadRequest("Lỗi: Tiền phạt không hợp lệ.");

                // Kiểm tra nhân viên tồn tại
                if (!await _context.NhanViens.AnyAsync(nv => nv.MaNv == pt.MaNhanVien))
                    return BadRequest($"Lỗi: Nhân viên mã {pt.MaNhanVien} không tồn tại.");

                _context.PhieuTras.Add(pt);
                await _context.SaveChangesAsync();

                return Ok(ToFlat(pt));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi thêm phiếu trả: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // 4. PUT: Cập nhật
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhieuTra(int id, [FromBody] PhieuTra pt)
        {
            try
            {
                pt.MaPhieuTra = id;

                // Xóa navigation để EF không cố update entity liên quan
                pt.MaNhanVienNavigation = null;
                pt.ChiTietMuons?.Clear();

                if (pt.MaNhanVien == null)
                    return BadRequest("Lỗi: Phải chọn nhân viên.");

                if (pt.NgayTra == null)
                    return BadRequest("Lỗi: Ngày trả không được để trống.");

                if (!await _context.NhanViens.AnyAsync(nv => nv.MaNv == pt.MaNhanVien))
                    return BadRequest($"Lỗi: Nhân viên mã {pt.MaNhanVien} không tồn tại.");

                if (!await _context.PhieuTras.AnyAsync(e => e.MaPhieuTra == id))
                    return NotFound("Không tìm thấy phiếu trả.");

                _context.Entry(pt).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("Cập nhật phiếu trả thành công.");
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Dữ liệu đã bị thay đổi bởi người khác, vui lòng tải lại.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // 5. DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhieuTra(int id)
        {
            try
            {
                var pt = await _context.PhieuTras.FindAsync(id);

                if (pt == null)
                    return NotFound("Không tìm thấy phiếu trả.");

                _context.PhieuTras.Remove(pt);
                await _context.SaveChangesAsync();
                return Ok("Đã xóa phiếu trả thành công.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Không thể xóa: Phiếu trả này còn chi tiết liên quan. ({ex.InnerException?.Message ?? ex.Message})");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi xóa: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // 6. SEARCH
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<PhieuTra>>> SearchPhieuTra(string keyword)
        {
            try
            {
                var query = _context.PhieuTras.AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(p => p.GhiChu != null && p.GhiChu.Contains(keyword));

                var list = await query
                    .Select(p => new PhieuTra
                    {
                        MaPhieuTra = p.MaPhieuTra,
                        MaNhanVien = p.MaNhanVien,
                        NgayTra = p.NgayTra,
                        TongTienPhat = p.TongTienPhat,
                        GhiChu = p.GhiChu
                    })
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi tìm kiếm: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }
}