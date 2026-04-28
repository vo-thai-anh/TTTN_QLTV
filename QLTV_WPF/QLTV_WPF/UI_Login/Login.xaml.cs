using QLTV_WPF.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace QLTV_WPF.UI
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var loginData = new
            {
                taiKhoan = txtUsername.Text,
                matKhau = txtPassword.Password
            };

            var json = JsonSerializer.Serialize(loginData);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            HttpClient client = new HttpClient();

            var response = await client.PostAsync(
                "https://localhost:7293/api/Login/login",
                content
            );

            if (response.IsSuccessStatusCode)
            {
                var responseData =
                    await response.Content.ReadAsStringAsync();

                var result =
                    JsonSerializer.Deserialize<LoginResponse>(responseData);

                CSessionManager.Token = result.token;
                CSessionManager.Role = result.role;
                CSessionManager.Username = result.username;
                CSessionManager.MaNV = result.manv;

                // CHUYỂN GIAO DIỆN THEO ROLE
                if (result.role == "Quản lý")
                {
                    UI_MeNu.MeNu_QL menu = new UI_MeNu.MeNu_QL();
                    menu.Show();
                }
                else
                {
                    UI_MeNu.MeNu_NV menu = new UI_MeNu.MeNu_NV();
                    menu.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
            }
        }
    }
}