using QLTV_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_WPF.Models_API
{
    class CXuLyLoaiSach
    {
        private static string strurl = @"https://localhost:7293/api/LoaiSach";
        public static List<LoaiSach> getdsls()
        {
            HttpClient hc = new HttpClient();

            var kq = hc.GetFromJsonAsync<List<LoaiSach>>(strurl);

            kq.Wait();

            if (kq.IsCompletedSuccessfully == false)
            {
                return null;
            }
            return kq.Result;
        }
        public static bool themls(Cloaisach ls)
        {
                using (HttpClient hc = new HttpClient())
                {
                    var kq = hc.PostAsJsonAsync<Cloaisach>(strurl, ls);
                    kq.Wait();

                    if (kq.IsCompletedSuccessfully)
                    {
                        return kq.Result.IsSuccessStatusCode;
                    }
                }

            return false;
        }
        public static bool suals(Cloaisach ls)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{ls.MaLoai}", ls);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        // Xóa dữ liệu
        public static bool xoals(int maLoai)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}/{maLoai}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
    }
}
