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
    public class ChiTietMuonsController : ControllerBase
    {
        private readonly QuanLyThuVienContext _context;

        public ChiTietMuonsController(QuanLyThuVienContext context)
        {
            _context = context;
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietMuon>>> Get()
        {
            return await _context.ChiTietMuons
                .Include(c => c.MaPhieuMuonNavigation)
                .Include(c => c.MaSachMuonNavigation)
                .ToListAsync();
        }

        // GET theo phiếu mượn
        [HttpGet("phieumuon/{maPhieuMuon}")]
        public async Task<ActionResult<IEnumerable<ChiTietMuon>>> GetByPhieuMuon(int maPhieuMuon)
        {
            return await _context.ChiTietMuons
                .Where(c => c.MaPhieuMuon == maPhieuMuon)
                .Include(c => c.MaSachMuonNavigation)
                .ToListAsync();
        }

        // POST (thêm sách vào phiếu mượn)
        [HttpPost]
        public async Task<IActionResult> Post(ChiTietMuon ct)
        {
            if (ct.MaPhieuMuon == 0)
                return BadRequest("Thiếu mã phiếu mượn");

            if (ct.MaSachMuon == 0)
                return BadRequest("Thiếu mã sách");

            _context.ChiTietMuons.Add(ct);
            await _context.SaveChangesAsync();

            return Ok("Thêm chi tiết mượn thành công");
        }

        // PUT (trả sách)
        [HttpPut]
        public async Task<IActionResult> Put(ChiTietMuon ct)
        {
            var existing = await _context.ChiTietMuons
                .FirstOrDefaultAsync(x => x.MaPhieuMuon == ct.MaPhieuMuon && x.MaSachMuon == ct.MaSachMuon);

            if (existing == null)
                return NotFound("Không tìm thấy chi tiết mượn");

            existing.NgayTraThucTe = ct.NgayTraThucTe;
            existing.TienPhat = ct.TienPhat;
            existing.LyDoPhat = ct.LyDoPhat;
            existing.MaPhieuTra = ct.MaPhieuTra;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật trả sách thành công");
        }

        // DELETE
        [HttpDelete]
        public async Task<IActionResult> Delete(int maPhieuMuon, int maSachMuon)
        {
            var ct = await _context.ChiTietMuons
                .FirstOrDefaultAsync(x => x.MaPhieuMuon == maPhieuMuon && x.MaSachMuon == maSachMuon);

            if (ct == null)
                return NotFound("Không tìm thấy");

            _context.ChiTietMuons.Remove(ct);
            await _context.SaveChangesAsync();

            return Ok("Đã xóa");
        }
    }
}