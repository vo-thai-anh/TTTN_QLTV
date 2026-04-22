using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QLTV_WPF.Models_API
{
    class CXuLySachTacGia
    {
        private static string strurl = @"https://localhost:7293/api/SachTacGia";

        public static List<int> getMaTGCuaSach(int maSach)
        {
            HttpClient hc = new HttpClient();
            var kq = hc.GetFromJsonAsync<List<int>>($"{strurl}/{maSach}");
            kq.Wait();
            return kq.Result;
        }

        public static bool capNhatTacGia(int maSach, List<int> danhSachMaTG)
        {
            using (HttpClient hc = new HttpClient())
            {
                var dto = new { MaSach = maSach, MaTGIds = danhSachMaTG };
                var kq = hc.PostAsJsonAsync($"{strurl}/update", dto);
                kq.Wait();
                return kq.Result.IsSuccessStatusCode;
            }
        }
    }
}