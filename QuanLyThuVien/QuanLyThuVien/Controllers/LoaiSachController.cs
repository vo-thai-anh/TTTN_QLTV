using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSachController : ControllerBase
    {
        private qltvContext db = new qltvContext();
        [HttpGet]
        public IActionResult getdsls()
        {
            try
            {
                var kq = db.LoaiSaches.Select(t => new
                {
                    maloai = t.MaLoai,
                    tenloai = t.Tenloai,
                    mota = t.Mota
                }).ToList();

                return Ok(kq);
            }
            catch (Exception)
            {
                return BadRequest();
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
                    Tenloai = ls.Tenloai,
                    Mota = ls.Mota
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
