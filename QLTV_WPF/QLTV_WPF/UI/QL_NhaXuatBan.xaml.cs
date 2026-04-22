using QLTV_API.Models;
using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace QLTV_WPF.UI
{
    /// <summary>
    /// Interaction logic for QL_NhaXuatBan.xaml
    /// </summary>
    public partial class QL_NhaXuatBan : Window
    {
        QuanLyThuVienContext db = new QuanLyThuVienContext();

        public QL_NhaXuatBan()
        {
            InitializeComponent();
            DataContext = new NhaXuatBanVM();
        }

        // Hàm tải dữ liệu từ Database

        private void dgNhaXuatBan_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgNhaXuatBan.SelectedItem is NhaXuatBan selected)
            {
                txtMaNXB.Text = selected.MaNxb.ToString();
                txtTenNXB.Text = selected.TenNxb;
                txtDiaChi.Text = selected.DiaChi;
                txtSDT.Text = selected.Sdt;
            }
        }
    }
}
