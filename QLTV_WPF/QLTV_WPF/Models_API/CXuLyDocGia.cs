using QLTV_WPF.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace QLTV_WPF.Models_API
{
    class CXuLyDocGia
    {
        private static string strurl = @"https://localhost:7293/api/DocGias";

        public static List<DocGia> getdsdg()
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<DocGia>>(strurl);
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<DocGia>();
            }
        }

        public static bool themdg(DocGia dg)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PostAsJsonAsync(strurl, dg);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool suadg(DocGia dg)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{dg.MaDocGia}", dg);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool xoadg(int id)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}/{id}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static List<DocGia> searchdg(string keyword)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<DocGia>>($"{strurl}/Search?keyword={keyword}");
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<DocGia>();
            }
        }
    }
}