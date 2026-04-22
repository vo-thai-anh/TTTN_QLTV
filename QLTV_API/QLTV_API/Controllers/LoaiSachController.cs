using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTV_API.Models;
using QLTV_API.ModelsDTO;

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
        public IActionResult themls(CLoaiSachDTO ls)
        {
            try
            {
                LoaiSach a = new LoaiSach
                {
                    TenLoai = ls.TenLoai,
                    MoTa = ls.MoTa
                };

                db.LoaiSaches.Add(a);
                db.SaveChanges();

                return Ok(a);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult getls(int id)
        {
            try
            {
                LoaiSach a = db.LoaiSaches.Find(id);

                if (a == null)
                    return NotFound();

                return Ok(new
                {
                    MaLoai = a.MaLoai,
                    TenLoai = a.TenLoai,
                    MoTa = a.MoTa
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult suals(CLoaiSachDTO x)
        {
            try
            {
                LoaiSach a = db.LoaiSaches.Find(x.MaLoai);

                if (a == null)
                    return NotFound();
                a.TenLoai = x.TenLoai;
                a.MoTa = x.MoTa;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult xoals(int id)
        {
            try
            {
                LoaiSach a = db.LoaiSaches.Find(id);

                if (a == null)
                    return NotFound();

                db.LoaiSaches.Remove(a);
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
