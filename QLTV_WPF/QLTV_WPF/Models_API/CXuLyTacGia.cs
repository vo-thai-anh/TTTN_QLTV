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

                if (!kq.Result.IsSuccessStatusCode)
                {
                    // Đọc thông báo lỗi chi tiết từ API trả về
                    string errorDetails = kq.Result.Content.ReadAsStringAsync().Result;

                    // Hiển thị hộp thoại để bạn dễ dàng nhìn thấy nguyên nhân thực sự
                    System.Windows.MessageBox.Show("Chi tiết lỗi từ API:\n" + errorDetails, "Lỗi Server", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                    return false;
                }
                return true;
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