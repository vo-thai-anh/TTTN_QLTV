using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLTV_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MenuItemLS_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_LoaiSach f = new UI.QL_LoaiSach();
            f.Show();
        }

        private void MenuItemDG_Click(object sender, RoutedEventArgs e)


        private void QLDocGia_Click(object sender, RoutedEventArgs e)

        {
            UI.QL_DocGia f = new UI.QL_DocGia();
            f.Show();
        }

        private void MenuItemNV_Click(object sender, RoutedEventArgs e)


        private void QLNhanVien_Click(object sender, RoutedEventArgs e)

        {
            UI.QL_NhanVien f = new UI.QL_NhanVien();
            f.Show();
        }
        private void MenuItemS_Click(object sender, RoutedEventArgs e)

        private void MenuItem_ClickSach(object sender, RoutedEventArgs e)

        {
            UI.QL_Sach f = new UI.QL_Sach();
            f.Show();
        }

        private void MenuItemSM_Click(object sender, RoutedEventArgs e)

        private void MenuItem_ClickSachMuon(object sender, RoutedEventArgs e)

        {
            UI.QL_SachMuon f = new UI.QL_SachMuon();
            f.Show();
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
            UI.QL_ChiTietMuon f = new UI.QL_ChiTietMuon();
            f.Show();
        }

    }
}