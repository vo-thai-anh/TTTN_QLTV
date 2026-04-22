using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLTV_API.Models;
using QLTV_API.ModelsDTO;
using System;
using System.Linq;

namespace QLTV_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SachMuonController : ControllerBase
    {
        private QuanLyThuVienContext db = new QuanLyThuVienContext();

        [HttpGet]
        public IActionResult GetDanhSachSachMuon()
        {
            try
            {
                var kq = db.SachMuons.Select(t => new
                {
                    masachmuon = t.MaSachMuon,
                    masach = t.MaSach,
                    tinhtrang = t.TinhTrang,
                    trangthai = t.TrangThai
                }).ToList();
                return Ok(kq);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ThemSachMuon(CSachMuonDTO sm)
        {
            try
            {
                SachMuon a = new SachMuon
                {
                    MaSach = sm.MaSach,
                    TinhTrang = sm.TinhTrang,
                    // Mặc định khi nhập sách mới vào kho là 0 (Sẵn sàng)
                    TrangThai = sm.TrangThai ?? 0
                };

                db.SachMuons.Add(a);
                db.SaveChanges();
                return Ok(a);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult SuaSachMuon(CSachMuonDTO sm)
        {
            try
            {
                SachMuon a = db.SachMuons.Find(sm.MaSachMuon);
                if (a == null) return NotFound();

                a.MaSach = sm.MaSach;
                a.TinhTrang = sm.TinhTrang;
                a.TrangThai = sm.TrangThai; // Cập nhật lại trạng thái nếu cần

                db.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult XoaSachMuon(int id)
        {
            try
            {
                SachMuon a = db.SachMuons.Find(id);
                if (a == null) return NotFound();

                db.SachMuons.Remove(a);
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