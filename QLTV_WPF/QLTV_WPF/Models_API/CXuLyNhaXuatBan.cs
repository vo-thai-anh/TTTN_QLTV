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
    class CXuLyNhaXuatBan
    {
        private static string strurl = @"https://localhost:7293/api/NhaXuatBan";

        public static List<NhaXuatBan> getdsnxb()
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<NhaXuatBan>>(strurl);
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : null;
            }
        }
        public static bool themnxb(NhaXuatBan nxb)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PostAsJsonAsync(strurl, nxb);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool suanxb(NhaXuatBan nxb)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync($"{strurl}/{nxb.MaNxb}", nxb);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        public static bool xoanxb(int maNXB)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}/{maNXB}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
    }
}
