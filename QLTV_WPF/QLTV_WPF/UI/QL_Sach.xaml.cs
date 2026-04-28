using QLTV_WPF.Models_API; // Thêm namespace chứa ViewModel của bạn
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
    /// Interaction logic for QL_Sach.xaml
    /// </summary>
    public partial class QL_Sach : Window
    {
        public QL_Sach(string role)
        {
            InitializeComponent();

            var vm = new SachVM();
            vm.UserRole = role;

            this.DataContext = vm;
        }
    }
}