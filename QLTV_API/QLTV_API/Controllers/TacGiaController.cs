using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTV_API.Models;
using System;
using System.Linq;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacGiaController : ControllerBase
    {
        private QuanLyThuVienContext db = new QuanLyThuVienContext();

        // 1. GET: Lấy danh sách Tác giả
        [HttpGet]
        public IActionResult GetDanhSach()
        {
            try
            {
                var kq = db.TacGia.Select(t => new
                {
                    matg = t.MaTg,
                    tentg = t.TenTg,
                    tieusu = t.TieuSu
                }).ToList();
                return Ok(kq);
            }
            catch (Exception ex)
            {
            
                return BadRequest(ex.Message);
            }
        }

        // 2. POST: Thêm Tác giả
        [HttpPost]
        public IActionResult Them(TacGia tg)
        {
            try
            {
                TacGia a = new TacGia
                {
                    TenTg = tg.TenTg,
                    TieuSu = tg.TieuSu
                };
                db.TacGia.Add(a);
                db.SaveChanges();
                return Ok(a);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // 3. PUT: Sửa Tác giả
        [HttpPut("{id}")]
        public IActionResult Sua(int id, TacGia tg)
        {
            try
            {
                TacGia a = db.TacGia.Find(id);
                if (a == null) return NotFound();

                a.TenTg = tg.TenTg;
                a.TieuSu = tg.TieuSu;
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // 4. DELETE: Xóa Tác giả
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