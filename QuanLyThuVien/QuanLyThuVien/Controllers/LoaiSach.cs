using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSach : ControllerBase
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
    }
}
