using QLTV_WPF.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace QLTV_WPF.Models_API
{
    class CXuLyPhieuMuon
    {
        private static string strurl = @"https://localhost:7293/api/PhieuMuons";

        public static List<PhieuMuon> getds()
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<PhieuMuon>>(strurl);
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<PhieuMuon>();
            }
        }

        public static bool them(PhieuMuon pm)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PostAsJsonAsync(strurl, pm);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool sua(PhieuMuon pm)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{pm.MaPhieuMuon}", pm);
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

        public static List<PhieuMuon> search(string keyword)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<PhieuMuon>>($"{strurl}/Search?keyword={keyword}");
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<PhieuMuon>();
            }
        }
    }
}