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
    public class SachController : ControllerBase
    {
        // Khởi tạo Context giống như LoaiSachController
        private QuanLyThuVienContext db = new QuanLyThuVienContext();

        // 1. GET: Lấy danh sách toàn bộ Sách
        [HttpGet]
        public IActionResult GetDanhSachSach()
        {
            try
            {
                var kq = db.Saches.Select(t => new
                {
                    masach = t.MaSach,
                    tensach = t.TenSach,
                    namxb = t.NamXb,
                    sotrang = t.SoTrang,
                    tomtat = t.TomTat,
                    soluong = t.SoLuong,
                    maloai = t.MaLoai,
                    manxb = t.MaNxb,

                    tenloai = t.MaLoaiNavigation != null ? t.MaLoaiNavigation.TenLoai : "",
                    tennxb = t.MaNxbNavigation != null ? t.MaNxbNavigation.TenNxb : "",
                    tentacgia = string.Join(", ", t.MaTgs.Select(tg => tg.TenTg))
                }).ToList();
                return Ok(kq);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 2. POST: Thêm mới một cuốn Sách
        [HttpPost]
        public IActionResult ThemSach(CSachDTO s)
        {
            try
            {
                Sach a = new Sach
                {
                    TenSach = s.TenSach,
                    NamXb = s.NamXb,
                    SoTrang = s.SoTrang,
                    TomTat = s.TomTat,
                    SoLuong = s.SoLuong,
                    MaLoai = s.MaLoai,
                    MaNxb = s.MaNxb
                };

                //Nếu có danh sách tác giả gửi kèm thì thêm luôn vào sách
                if (s.MaTGIds != null && s.MaTGIds.Count > 0)
                {
                    var tacGias = db.TacGia.Where(tg => s.MaTGIds.Contains(tg.MaTg)).ToList();
                    foreach (var tg in tacGias)
                    {
                        a.MaTgs.Add(tg);
                    }
                }

                db.Saches.Add(a);
                db.SaveChanges(); 

                return Ok(a);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // 3. GET: Lấy thông tin 1 cuốn sách theo ID
        [HttpGet("{id}")]
        public IActionResult GetSach(int id)
        {
            try
            {
                Sach a = db.Saches.Find(id);

                if (a == null)
                    return NotFound();

                return Ok(new
                {
                    masach = a.MaSach,
                    tensach = a.TenSach,
                    namxb = a.NamXb,
                    sotrang = a.SoTrang,
                    tomtat = a.TomTat,
                    soluong = a.SoLuong,
                    maloai = a.MaLoai,
                    manxb = a.MaNxb
                });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // 4. PUT: Sửa thông tin cuốn sách
        [HttpPut("{id}")]
        public IActionResult SuaSach(CSachDTO x)
        {
            try
            {
                // Tìm sách dựa vào MaSach truyền qua DTO tương tự như LoaiSachController
                Sach a = db.Saches.Find(x.MaSach);

                if (a == null)
                    return NotFound();

                a.TenSach = x.TenSach;
                a.NamXb = x.NamXb;
                a.SoTrang = x.SoTrang;
                a.TomTat = x.TomTat;
                a.SoLuong = x.SoLuong;
                a.MaLoai = x.MaLoai;
                a.MaNxb = x.MaNxb;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // 5. DELETE: Xóa sách theo ID
        [HttpDelete("{id}")]
        public IActionResult XoaSach(int id)
        {
            try
            {
                Sach a = db.Saches.Find(id);

                if (a == null)
                    return NotFound();

                db.Saches.Remove(a);
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