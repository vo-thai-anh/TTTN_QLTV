using Microsoft.AspNetCore.Identity;
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

        // 3. THÊM MỚI NHÂN VIÊN (Chỉ cho phép tạo chức vụ Nhân viên)
        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVien nv)
        {
            // Chặn nếu cố tình đặt chức vụ Quản lý/Admin hoặc đặt tài khoản là admin
            if (string.Equals(nv.ChucVu, "Quản lý", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(nv.ChucVu, "Admin", StringComparison.OrdinalIgnoreCase) ||
                nv.TaiKhoan.ToLower() == "admin")
            {
                return BadRequest("Hệ thống chỉ cho phép tạo tài khoản Nhân viên.");
            }

            // Luôn gán cứng chức vụ là Nhân viên để đảm bảo an toàn
            nv.ChucVu = "Nhân viên";

            // Băm mật khẩu khi tạo mới
            var hasher = new PasswordHasher<NhanVien>();
            nv.MatKhau = hasher.HashPassword(nv, nv.MatKhau);

            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetNhanVien", new { id = nv.MaNv }, nv);
        }

        // 4. CẬP NHẬT THÔNG TIN NHÂN VIÊN
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanVien(int id, NhanVien nv)
        {
            var nvOld = await _context.NhanViens.AsNoTracking().FirstOrDefaultAsync(x => x.MaNv == id);

            if (nvOld == null)
                return NotFound("Không tìm thấy nhân viên.");


            nv.MaNv = id;
            // Ép chức vụ về Nhân viên để tránh việc đổi từ Nhân viên lên Quản lý qua API
            //nv.ChucVu = "Nhân viên";

            // XỬ LÝ MẬT KHẨU
            var hasher = new PasswordHasher<NhanVien>();
            if (string.IsNullOrWhiteSpace(nv.MatKhau))
            {
                nv.MatKhau = nvOld.MatKhau;
            }
            else
            {
                nv.MatKhau = hasher.HashPassword(nv, nv.MatKhau);
            }

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

        // 5. XÓA NHÂN VIÊN
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound("Không tìm thấy nhân viên.");

            // QUÉT CHỨC VỤ: Chặn xóa nếu chức vụ là Quản lý hoặc Admin
            if (string.Equals(nv.ChucVu, "Quản lý", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(nv.ChucVu, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Đây là tài khoản Quản lý hệ thống, không thể xóa!");
            }

            _context.NhanViens.Remove(nv);
            await _context.SaveChangesAsync();
            return Ok("Đã xóa nhân viên thành công.");
        }

        // 6. TÌM KIẾM
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> SearchNhanVien(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await _context.NhanViens.ToListAsync();
            }

            var kq = await _context.NhanViens
                .Where(x => x.HoTen.Contains(keyword) || x.Sdt.Contains(keyword))
                .ToListAsync();

            return kq;
        }
    }
}