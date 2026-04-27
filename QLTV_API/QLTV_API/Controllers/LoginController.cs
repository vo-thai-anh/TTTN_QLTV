using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLTV_API.Models;
using QLTV_API.ModelsDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly QuanLyThuVienContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(QuanLyThuVienContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        private string GenerateToken(NhanVien nv)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, nv.MaNv.ToString()),
            new Claim(ClaimTypes.Name, nv.HoTen),
            new Claim(ClaimTypes.Role, nv.ChucVu)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(CLogin model)
        {
            var nv = await _context.NhanViens
                .FirstOrDefaultAsync(x => x.TaiKhoan == model.TaiKhoan);
            if (nv == null)
                return Unauthorized("Sai tài khoản");

            var hasher = new PasswordHasher<NhanVien>();

            var result = hasher.VerifyHashedPassword(nv, nv.MatKhau, model.MatKhau);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Sai mật khẩu");

            var token = GenerateToken(nv);

            return Ok(new
            {
                token = token,
                role = nv.ChucVu,
                username = nv.HoTen,
                Manv = nv.MaNv
            });
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                return Ok(new
                {
                    message = "Đăng xuất thành công"
                });
            }
            catch
            {
                return BadRequest("Lỗi đăng xuất");
            }
        }

    }
}
