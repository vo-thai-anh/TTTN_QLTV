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
    /// Interaction logic for MeNu_NV.xaml
    /// </summary>
    public partial class MeNu_NV : Window
    {
        public MeNu_NV()
        {
            InitializeComponent();
        }

        private void MenuItem_ClickSach(object sender, RoutedEventArgs e)
        {
            var window = new QL_Sach("Nhân viên");
            window.Show();
        }

        private void QLDocGia_Click(object sender, RoutedEventArgs e)
        {
            QL_DocGia f = new QL_DocGia();
            f.Show();
        }

        private void QLPhieuMuon_Click(object sender, RoutedEventArgs e)
        {
            QL_PhieuMuon f = new QL_PhieuMuon();
            f.Show();
        }

        private void QLPhieuTra_Click(object sender, RoutedEventArgs e)
        {
            QL_PhieuTra f = new QL_PhieuTra();
            f.Show();
        }

        private void QLChiTietMuon_Click(object sender, RoutedEventArgs e)
        {
            QL_ChiTietMuon f = new QL_ChiTietMuon(0);
            f.Show();
        }

        private void MenuItem_ClickSachMuon(object sender, RoutedEventArgs e)
        {
            QL_SachMuon f = new QL_SachMuon();
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
