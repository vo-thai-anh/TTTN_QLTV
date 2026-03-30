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
    /// Interaction logic for QL_TacGia.xaml
    /// </summary>
    public partial class QL_TacGia : Window
    {
        QuanLyThuVienContext db = new QuanLyThuVienContext();

        public QL_TacGia()
        {
            InitializeComponent();
            DataContext = new TacGiaVM();
        }

        // Hàm tải dữ liệu từ Database

        private void dgTacGia_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgTacGia.SelectedItem is TacGium selected)
            {
                txtMaTg.Text = selected.MaTg.ToString();
                txtTenTg.Text = selected.TenTg;
                txtTieuSu.Text = selected.TieuSu;
            }
        }
    }
}
