using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QLTV_WPF.UI
{
    public partial class QL_Sach_TacGia : Window
    {
        private int _maSach;
        public List<CTacGiaChon> ListTacGiaChon { get; set; }

        public QL_Sach_TacGia(int maSach, string tenSach)
        {
            InitializeComponent();
            _maSach = maSach;
            lblTenSach.Text = $"Sách: {tenSach}";

            // 1. Lấy tất cả tác giả có trong hệ thống
            var tatCaTG = CXuLyTacGia.getdstg();

            // 2. Lấy danh sách ID các tác giả đã viết cuốn sách này
            var idsDaChon = CXuLySachTacGia.getMaTGCuaSach(maSach);

            // 3. Trộn dữ liệu: Ai đã viết thì IsSelected = true
            ListTacGiaChon = tatCaTG.Select(x => new CTacGiaChon
            {
                MaTg = x.MaTg,
                TenTg = x.TenTg,
                IsSelected = idsDaChon.Contains(x.MaTg)
            }).ToList();

            this.DataContext = this;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            // Lọc ra những ông được tích chọn
            var idsMoi = ListTacGiaChon.Where(x => x.IsSelected).Select(x => x.MaTg).ToList();

            if (CXuLySachTacGia.capNhatTacGia(_maSach, idsMoi))
            {
                MessageBox.Show("Cập nhật tác giả thành công!");
                this.Close();
            }
        }

        private void btnDong_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}