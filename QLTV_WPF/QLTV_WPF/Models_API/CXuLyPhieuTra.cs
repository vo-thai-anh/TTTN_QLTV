using QLTV_WPF.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace QLTV_WPF.Models_API
{
    class CXuLyPhieuTra
    {
        private static string strurl = @"https://localhost:7293/api/PhieuTras";

        public static List<PhieuTra> getds()
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    hc.Timeout = TimeSpan.FromSeconds(10);
                    var kq = hc.GetFromJsonAsync<List<PhieuTra>>(strurl);
                    kq.Wait();
                    return kq.IsCompletedSuccessfully ? kq.Result : new List<PhieuTra>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải danh sách phiếu trả:\n{ex.InnerException?.Message ?? ex.Message}",
                                "Lỗi kết nối", MessageBoxButton.OK, MessageBoxImage.Warning);
                return new List<PhieuTra>();
            }
        }

        public static bool them(PhieuTra pt)
        {
            try
            {
                // Xóa navigation trước khi gửi để tránh API bị lỗi validation
                pt.MaNhanVienNavigation = null;
                pt.ChiTietMuons?.Clear();

                using (HttpClient hc = new HttpClient())
                {
                    hc.Timeout = TimeSpan.FromSeconds(10);
                    var kq = hc.PostAsJsonAsync(strurl, pt);
                    kq.Wait();
                    if (kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode)
                        return true;

                    // Đọc thông báo lỗi từ API
                    var errMsg = kq.Result.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"API trả về lỗi: {errMsg}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm phiếu trả:\n{ex.InnerException?.Message ?? ex.Message}",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool sua(PhieuTra pt)
        {
            try
            {
                // Xóa navigation trước khi gửi
                pt.MaNhanVienNavigation = null;
                pt.ChiTietMuons?.Clear();

                using (HttpClient hc = new HttpClient())
                {
                    hc.Timeout = TimeSpan.FromSeconds(10);
                    var kq = hc.PutAsJsonAsync($"{strurl}/{pt.MaPhieuTra}", pt);
                    kq.Wait();
                    if (kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode)
                        return true;

                    var errMsg = kq.Result.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"API trả về lỗi: {errMsg}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật phiếu trả:\n{ex.InnerException?.Message ?? ex.Message}",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool xoa(int id)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    hc.Timeout = TimeSpan.FromSeconds(10);
                    var kq = hc.DeleteAsync($"{strurl}/{id}");
                    kq.Wait();
                    if (kq.IsCompletedSuccessfully && kq.Result.IsSuccessStatusCode)
                        return true;

                    var errMsg = kq.Result.Content.ReadAsStringAsync().Result;
                    MessageBox.Show($"API trả về lỗi: {errMsg}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa phiếu trả:\n{ex.InnerException?.Message ?? ex.Message}",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}