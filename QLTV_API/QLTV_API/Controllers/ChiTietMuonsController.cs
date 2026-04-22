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

            // KIỂM TRA SÁCH CÓ THỂ MƯỢN ĐƯỢC HAY KHÔNG
            var sachMuon = await _context.SachMuons.FirstOrDefaultAsync(s => s.MaSachMuon == ct.MaSachMuon);
            if (sachMuon == null)
                return NotFound("Không tìm thấy mã Sách Mượn này trong kho.");
            
            if (sachMuon.TrangThai == 1)
                return BadRequest("Cuốn sách này đang được người khác mượn, chưa trả lại kho!");

            // Dùng Transaction để đảm bảo tính toàn vẹn 2 bản ghi
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.ChiTietMuons.Add(ct);
                    sachMuon.TrangThai = 1;
                    _context.Entry(sachMuon).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok("Thêm mục mượn sách thành công");
                }
                catch (System.Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Lỗi máy chủ khi mượn sách: " + ex.Message);
                }
            }
        }

        // PUT (trả sách)
        [HttpPut]
        public async Task<IActionResult> Put(ChiTietMuon ct)
        {
            var existing = await _context.ChiTietMuons
                .FirstOrDefaultAsync(x => x.MaPhieuMuon == ct.MaPhieuMuon && x.MaSachMuon == ct.MaSachMuon);

            if (existing == null)
                return NotFound("Không tìm thấy chi tiết mượn");

            var sachMuon = await _context.SachMuons.FirstOrDefaultAsync(s => s.MaSachMuon == ct.MaSachMuon);
            if (sachMuon == null)
                return NotFound("Không tìm thấy mã sách vật lý này.");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Cập nhật Phiếu Trả vào ChiTietMuon
                    existing.NgayTraThucTe = ct.NgayTraThucTe ?? System.DateTime.Now;
                    existing.TienPhat = ct.TienPhat;
                    existing.LyDoPhat = ct.LyDoPhat;
                    existing.MaPhieuTra = ct.MaPhieuTra;

                    // 2. Mở khóa nhả Sách về kho (Trạng thái = 0)
                    sachMuon.TrangThai = 0;
                    _context.Entry(sachMuon).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok("Cập nhật trả sách và thu hồi sách về kho thành công");
                }
                catch (System.Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Lỗi máy chủ khi trả sách: " + ex.Message);
                }
            }
        }

        // DELETE (xóa nhầm sách khỏi giỏ hàng trước khi trả)
        [HttpDelete]
        public async Task<IActionResult> Delete(int maPhieuMuon, int maSachMuon)
        {
            var ct = await _context.ChiTietMuons
                .FirstOrDefaultAsync(x => x.MaPhieuMuon == maPhieuMuon && x.MaSachMuon == maSachMuon);

            if (ct == null)
                return NotFound("Không tìm thấy mục chi tiết mượn này");

            var sachMuon = await _context.SachMuons.FirstOrDefaultAsync(s => s.MaSachMuon == maSachMuon);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Gỡ khỏi hóa đơn mượn
                    _context.ChiTietMuons.Remove(ct);

                    // 2. Mở khóa nhả lại sách về kệ đọc (Nếu sách đã lỡ chuyển Trạng Thái = 1)
                    if (sachMuon != null && sachMuon.TrangThai == 1)
                    {
                        sachMuon.TrangThai = 0;
                        _context.Entry(sachMuon).State = EntityState.Modified;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok("Hủy bỏ chi tiết mượn và cập nhật lại kho");
                }
                catch (System.Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, "Lỗi khi xóa chi tiết mượn: " + ex.Message);
                }
            }
        }
    }
}