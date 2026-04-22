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
    /// Interaction logic for QL_SachMuon.xaml
    /// </summary>
    public partial class QL_SachMuon : Window
    {
        public QL_SachMuon(int? maSachTruyenSang = null)
        {
            InitializeComponent();
            DataContext = new SachMuonVM(maSachTruyenSang);
        }
      
       
    }
}
