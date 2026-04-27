using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTV_API.Models;
using QLTV_API.ModelsDTO; // Phải có dòng này để gọi được CTacGiaDTO
using System;
using System.Linq;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacGiaController : ControllerBase
    {
        private QuanLyThuVienContext db = new QuanLyThuVienContext();

    
        [HttpGet]
        public IActionResult GetDanhSach()
        {
            try
            {
                var kq = db.TacGia.Select(t => new CTacGiaDTO
                {
                    MaTg = t.MaTg,
                    TenTg = t.TenTg,
                    TieuSu = t.TieuSu,
                    Butdanh = t.Butdanh,
                    Namsinh = t.Namsinh
                }).ToList();
                return Ok(kq);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Them(CTacGiaDTO tg)
        {
            try
            {
                TacGia a = new TacGia
                {
                    TenTg = tg.TenTg,
                    TieuSu = tg.TieuSu,
                    Butdanh = tg.Butdanh,
                    Namsinh = tg.Namsinh
                };
                db.TacGia.Add(a);
                db.SaveChanges();
                return Ok(tg);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("{id}")]
        public IActionResult Sua(int id, CTacGiaDTO tg)
        {
            try
            {
                TacGia a = db.TacGia.Find(id);
                if (a == null) return NotFound();

                a.TenTg = tg.TenTg;
                a.TieuSu = tg.TieuSu;
                a.Butdanh = tg.Butdanh;
                a.Namsinh = tg.Namsinh;
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        
        [HttpDelete("{id}")]
        public IActionResult Xoa(int id)
        {
            try
            {
                TacGia a = db.TacGia.Find(id);
                if (a == null) return NotFound();
                db.TacGia.Remove(a);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}