using QLTV_WPF.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace QLTV_WPF.Models_API
{
    class CXulyChiTietMuon
    {
        private static string strurl = @"https://localhost:7293/api/ChiTietMuons";

        // Lấy tất cả
        public static List<ChiTietMuon> getds()
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<ChiTietMuon>>(strurl);
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<ChiTietMuon>();
            }
        }

        // Lấy theo phiếu mượn
        public static List<ChiTietMuon> getByPhieuMuon(int maPM)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.GetFromJsonAsync<List<ChiTietMuon>>($"{strurl}/phieumuon/{maPM}");
                kq.Wait();
                return kq.IsCompletedSuccessfully ? kq.Result : new List<ChiTietMuon>();
            }
        }

        // Thêm
        public static bool them(ChiTietMuon ct)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    var kq = hc.PostAsJsonAsync(strurl, ct);
                    kq.Wait();

                    // Nếu API trả về 200 OK
                    if (kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode)
                        return true;

                    // NẾU THẤT BẠI: Đọc lỗi thật từ Server
                    var errMsg = kq.Result.Content.ReadAsStringAsync().Result;
                    System.Windows.MessageBox.Show($"API báo lỗi chi tiết:\n{errMsg}", "Lỗi Server",
                                                   System.Windows.MessageBoxButton.OK,
                                                   System.Windows.MessageBoxImage.Error);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Lỗi kết nối:\n{ex.Message}", "Lỗi",
                                               System.Windows.MessageBoxButton.OK,
                                               System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        // Cập nhật (trả sách)
        public static bool sua(ChiTietMuon ct)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.PutAsJsonAsync(strurl, ct);
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }

        // Xóa
        public static bool xoa(int maPM, int maSach)
        {
            using (HttpClient hc = new HttpClient())
            {
                var kq = hc.DeleteAsync($"{strurl}?maPhieuMuon={maPM}&maSachMuon={maSach}");
                kq.Wait();
                return kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode;
            }
        }
    }
}