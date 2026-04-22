using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTV_API.Models;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaXuatBanController : ControllerBase
    {
        private QuanLyThuVienContext db = new QuanLyThuVienContext();

        // 1. GET: Lấy danh sách Nhà xuất bản
        [HttpGet]
        public IActionResult GetDanhSach()
        {
            try
            {
                var kq = db.NhaXuatBans.Select(t => new
                {
                    manxb = t.MaNxb,
                    tennxb = t.TenNxb,
                    diachi = t.DiaChi,
                    sdt = t.Sdt
                }).ToList();
                return Ok(kq);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 2. POST: Thêm Nhà xuất bản
        [HttpPost]
        public IActionResult Them(NhaXuatBan nxb)
        {
            try
            {
                NhaXuatBan a = new NhaXuatBan
                {
                    TenNxb = nxb.TenNxb,
                    DiaChi = nxb.DiaChi,
                    Sdt = nxb.Sdt
                };
                db.NhaXuatBans.Add(a);
                db.SaveChanges();
                return Ok(a);
            }
            catch (Exception) { return BadRequest(); }
        }

        // 3. PUT: Sửa Nhà xuất bản
        [HttpPut("{id}")]
        public IActionResult Sua(int id, NhaXuatBan nxb)
        {
            try
            {
                NhaXuatBan a = db.NhaXuatBans.Find(id);
                if (a == null) return NotFound();

                a.TenNxb = nxb.TenNxb;
                a.DiaChi = nxb.DiaChi;
                a.Sdt = nxb.Sdt;
                db.SaveChanges();
                return Ok();
            }
            catch (Exception) { return BadRequest(); }
        }

        // 4. DELETE: Xóa Nhà xuất bản
        [HttpDelete("{id}")]
        public IActionResult Xoa(int id)
        {
            try
            {
                NhaXuatBan a = db.NhaXuatBans.Find(id);
                if (a == null) return NotFound();

                db.NhaXuatBans.Remove(a);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception) { return BadRequest(); }
        }
    }
}
