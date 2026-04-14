using QLTV_WPF.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace QLTV_WPF.Models_API
{
    class CXuLySachMuon
    {
        private static string strurl = @"https://localhost:7293/api/SachMuon"; // Nhớ đổi port nếu khác

        public static List<CSachMuon> getdssachmuon()
        {
            HttpClient hc = new HttpClient();
            var kq = hc.GetFromJsonAsync<List<CSachMuon>>(strurl);
            kq.Wait();
            return kq.IsCompletedSuccessfully ? kq.Result : null;
        }

        public static bool themsachmuon(CSachMuon sm)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PostAsJsonAsync<CSachMuon>(strurl, sm);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool suasachmuon(CSachMuon sm)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{sm.MaSachMuon}", sm);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool xoasachmuon(int maSachMuon)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}/{maSachMuon}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
    }
}