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
    class CXuLyTacGia
    {
        private static string strurl = @"https://localhost:7293/api/TacGia";
        public static List<TacGia> getdstg()
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<TacGia>>(strurl);
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : null;
            }
        }
        public static bool themtg(TacGia tg)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PostAsJsonAsync<TacGia>(strurl, tg);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
        public static bool suatg(TacGia tg)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{tg.MaTg}", tg);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
        public static bool xoatg(int maTG)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}/{maTG}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
    }
}
