using QLTV_WPF.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace QLTV_WPF.Models_API
{
    class CXuLyNhanVien
    {
        private static string strurl = @"https://localhost:7293/api/NhanViens"; // Kiểm tra lại Port nhé An

        public static List<NhanVien> GetDsNhanVien()
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<NhanVien>>(strurl);
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<NhanVien>();
            }
        }

        public static bool ThemNhanVien(NhanVien nv)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PostAsJsonAsync(strurl, nv);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool SuaNhanVien(NhanVien nv)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{nv.MaNv}", nv);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool XoaNhanVien(int id)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}/{id}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static List<NhanVien> SearchNhanVien(string keyword)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<NhanVien>>($"{strurl}/Search?keyword={keyword}");
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<NhanVien>();
            }
        }

    }
}