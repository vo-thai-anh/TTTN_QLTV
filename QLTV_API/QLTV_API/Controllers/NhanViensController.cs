using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTV_API.Models;
using System.Text.Json.Serialization;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanViensController : ControllerBase
    {
        private readonly QuanLyThuVienContext _context;

        public NhanViensController(QuanLyThuVienContext context)
        {
            _context = context;
        }

        // 1. LẤY DANH SÁCH NHÂN VIÊN
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanViens()
        {
            return await _context.NhanViens.ToListAsync();
        }

        // 2. LẤY THÔNG TIN CHI TIẾT 1 NHÂN VIÊN
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound("Không tìm thấy nhân viên!");
            return nv;
        }



        // 4. THÊM MỚI NHÂN VIÊN (Kiểm tra Chức vụ & Tài khoản)
        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVien nv)
        {
            // Ép mã về 0 để SQL Server tự nhảy số (1, 2, 3...)
            nv.MaNv = 0;

            // Chỉ kiểm tra các trường bắt buộc khác
            if (string.IsNullOrWhiteSpace(nv.HoTen) || string.IsNullOrWhiteSpace(nv.TaiKhoan))
                return BadRequest("Họ tên và Tài khoản không được để trống!");

            // Kiểm tra chức vụ
            var dsChucVu = new List<string> { "Nhân viên", "Quản lý" };
            if (!dsChucVu.Contains(nv.ChucVu))
                return BadRequest("Chức vụ phải là 'Nhân viên' hoặc 'Quản lý'.");

            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();

            return Ok(nv);
        }
        // 5. CẬP NHẬT THÔNG TIN NHÂN VIÊN
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanVien(int id, NhanVien nv)
        {
            nv.MaNv = id;

            var dsChucVu = new List<string> { "Nhân viên", "Quản lý" };

            if (!dsChucVu.Contains(nv.ChucVu))
                return BadRequest("Lỗi: Chức vụ không hợp lệ.");

            _context.Entry(nv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Cập nhật thông tin nhân viên thành công.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.NhanViens.Any(e => e.MaNv == id)) return NotFound("Không tìm thấy nhân viên.");
                else throw;
            }
        }

        // 6. XÓA NHÂN VIÊN
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            _context.NhanViens.Remove(nv);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        //Tìm kiếm theo keyword
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> SearchNhanVien(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await _context.NhanViens.ToListAsync();
            }

            // Tìm theo tên hoặc SĐT (vì mã là int nên mình tìm theo keyword chứa trong tên)
            var kq = await _context.NhanViens
                .Where(x => x.HoTen.Contains(keyword) || x.Sdt.Contains(keyword))
                .ToListAsync();

            return kq;
        }
       
    }
}