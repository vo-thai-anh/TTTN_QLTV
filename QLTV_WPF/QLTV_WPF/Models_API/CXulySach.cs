using QLTV_WPF.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace QLTV_WPF.Models_API
{
    class CXuLySach
    {
        // Đường dẫn tới API Sách (Thay đổi port nếu cần)
        private static string strurl = @"https://localhost:7293/api/Sach";

        // GET: Lấy danh sách Sách
        public static List<Sach> getdssach()
        {
            HttpClient hc = new HttpClient();

            // Trả về List<Sach> để hiển thị trên DataGrid (giống cách LoaiSach hoạt động)
            var kq = hc.GetFromJsonAsync<List<Sach>>(strurl);

            kq.Wait();

            if (kq.IsCompletedSuccessfully == false)
            {
                return null;
            }
            return kq.Result;
        }

        // POST: Thêm sách mới
        public static bool themsach(CSach s)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PostAsJsonAsync<CSach>(strurl, s);
                kq.Wait();

                if (kq.IsCompletedSuccessfully)
                {
                    return kq.Result.IsSuccessStatusCode;
                }
            }

            return false;
        }

        // PUT: Cập nhật sách
        public static bool suasach(CSach s)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{s.MaSach}", s);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        // DELETE: Xóa sách
        public static bool xoasach(int maSach)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}/{maSach}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
    }
}