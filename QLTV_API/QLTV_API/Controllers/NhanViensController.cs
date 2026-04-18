using Microsoft.AspNetCore.Identity;
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



        // 4. THÊM MỚI NHÂN VIÊN
        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVien nv)
        {
            // Ép mã về 0 để SQL Server tự tăng
            nv.MaNv = 0;

            if (string.IsNullOrWhiteSpace(nv.HoTen) ||
                string.IsNullOrWhiteSpace(nv.TaiKhoan) ||
                string.IsNullOrWhiteSpace(nv.MatKhau))
            {
                return BadRequest("Họ tên, Tài khoản và Mật khẩu không được để trống!");
            }

            //Kiểm tra chức vụ ( ?? "" để tránh cảnh báo)
            var dsChucVu = new List<string> { "Nhân viên", "Quản lý" };
            if (!dsChucVu.Contains(nv.ChucVu ?? ""))
            {
                return BadRequest("Chức vụ phải là 'Nhân viên' hoặc 'Quản lý'.");
            }

            var hasher = new PasswordHasher<NhanVien>();
            // Băm mật khẩu trước khi đưa vào Database
            nv.MatKhau = hasher.HashPassword(nv, nv.MatKhau!);

            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();

            return Ok("Thêm nhân viên mới thành công.");
        }
        // 5. CẬP NHẬT THÔNG TIN NHÂN VIÊN
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanVien(int id, NhanVien nv)
        {
            // thông tin nhân viên cũ
            var nvOld = await _context.NhanViens.AsNoTracking().FirstOrDefaultAsync(x => x.MaNv == id);

            if (nvOld == null)
                return NotFound("Không tìm thấy nhân viên.");

            nv.MaNv = id;

            var dsChucVu = new List<string> { "Nhân viên", "Quản lý" };
            if (!dsChucVu.Contains(nv.ChucVu))
                return BadRequest("Lỗi: Chức vụ không hợp lệ.");

            //XỬ LÝ MẬT KHẨU
            var hasher = new PasswordHasher<NhanVien>();

            if (string.IsNullOrWhiteSpace(nv.MatKhau))
            {
                // Nếu ô mật khẩu trống -> Lấy lại mật khẩu cũ đã mã hóa trong DB để giữ nguyên
                nv.MatKhau = nvOld.MatKhau;
            }
            else
            {
                // Nếu có nhập mật khẩu mới -> Tiến hành băm (Hash) mật khẩu mới
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

        // 6. XÓA NHÂN VIÊN
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(int id)
        {
            // Kiểm tra xem nhân viên này có lập phiếu mượn nào không
            bool hasPhieuMuon = await _context.PhieuMuons.AnyAsync(p => p.MaNhanVien == id);

            if (hasPhieuMuon)
            {
                return BadRequest("Không thể xóa! Nhân viên này đã lập phiếu mượn.");
            }

            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound("Không tìm thấy nhân viên.");

            _context.NhanViens.Remove(nv);
            await _context.SaveChangesAsync();

            return Ok("Xóa nhân viên thành công.");
        }
        //Tìm kiếm theo keyword
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> SearchNhanVien(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return await _context.NhanViens.ToListAsync();
            }

            // Tìm theo tên hoặc SĐT
            var kq = await _context.NhanViens
                .Where(x => x.HoTen.Contains(keyword) || x.Sdt.Contains(keyword))
                .ToListAsync();

            return kq;
        }
       
    }
}