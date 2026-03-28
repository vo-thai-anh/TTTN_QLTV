using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTV_API.Models;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSachController : ControllerBase
    {
        private QuanLyThuVienContext db = new QuanLyThuVienContext();

        [HttpGet]
        public IActionResult getdsls()
        {
            try
            {
                var kq = db.LoaiSaches.Select(t => new
                {
                    maloai = t.MaLoai,
                    tenloai = t.TenLoai,
                    mota = t.MoTa
                }).ToList();
                return Ok(kq);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult them(LoaiSach ls)
        {
            try
            {
                LoaiSach a = new LoaiSach
                {
                    MaLoai = ls.MaLoai,
                    TenLoai = ls.TenLoai,
                    MoTa = ls.MoTa
                };
                db.LoaiSaches.Add(a);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
