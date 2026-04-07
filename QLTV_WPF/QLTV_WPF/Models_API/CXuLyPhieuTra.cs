using QLTV_WPF.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace QLTV_WPF.Models_API
{
    class CXuLyPhieuTra
    {
        private static string strurl = @"https://localhost:7293/api/PhieuTras";

        public static List<PhieuTra> getds()
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<PhieuTra>>(strurl);
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<PhieuTra>();
            }
        }

        public static bool them(PhieuTra pt)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PostAsJsonAsync(strurl, pt);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool sua(PhieuTra pt)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{pt.MaPhieuTra}", pt);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool xoa(int id)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}/{id}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
    }
}