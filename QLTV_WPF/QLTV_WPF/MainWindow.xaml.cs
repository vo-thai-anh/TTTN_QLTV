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
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UI.QL_LoaiSach f = new UI.QL_LoaiSach();
            f.Show();
        }
        private void MenuItem_ClickSach(object sender, RoutedEventArgs e)
        {
            UI.QL_Sach f = new UI.QL_Sach();
            f.Show();
        }
    }
}