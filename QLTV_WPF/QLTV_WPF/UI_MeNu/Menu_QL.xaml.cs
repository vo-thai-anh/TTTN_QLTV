using QLTV_WPF.Models;
using QLTV_WPF.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QLTV_WPF.UI_MeNu
{
    /// <summary>
    /// Interaction logic for Menu_QL.xaml
    /// </summary>
    public partial class MeNu_QL : Window
    {
        public MeNu_QL()
        {
            InitializeComponent();
        }
        private void MenuItemLS_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_LoaiSach f = new UI.QL_LoaiSach();
            f.Show();
        }

        private void MenuItem_ClickSach(object sender, RoutedEventArgs e)
        {
            var window = new QL_Sach("Quản lý"); // hoặc "Quản lý"
            window.Show();
        }

        private void QLTacGia_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_TacGia f = new UI.QL_TacGia();
            f.Show();
        }

        private void QLNhaXuatBan_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_NhaXuatBan f = new UI.QL_NhaXuatBan();
            f.Show();
        }

        private void QLDocGia_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_DocGia f = new UI.QL_DocGia();
            f.Show();
        }

        private void QLNhanVien_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_NhanVien f = new UI.QL_NhanVien();
            f.Show();
        }

        private void QLPhieuMuon_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_PhieuMuon f = new UI.QL_PhieuMuon();
            f.Show();
        }

        private void QLPhieuTra_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_PhieuTra f = new UI.QL_PhieuTra();
            f.Show();
        }

        private void QLChiTietMuon_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_ChiTietMuon f = new UI.QL_ChiTietMuon(0);
            f.Show();
        }

        private void MenuItem_ClickSachMuon(object sender, RoutedEventArgs e)
        {
            UI.QL_SachMuon f = new UI.QL_SachMuon();
            f.Show();
        }
        private async Task LogoutFromApi()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        CSessionManager.Token
                    );

                await client.PostAsync(
                    "https://localhost:7293/api/Login/logout",
                    null
                );
            }

            CSessionManager.Logout();
        }


        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            CSessionManager.Logout();

            UI.Login login = new UI.Login();
            login.Show();

            this.Close();
        }

    }
}
