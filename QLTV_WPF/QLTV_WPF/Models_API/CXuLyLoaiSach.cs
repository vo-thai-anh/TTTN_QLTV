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
    }
}
