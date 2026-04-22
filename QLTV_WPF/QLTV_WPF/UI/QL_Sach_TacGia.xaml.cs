using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QLTV_WPF.UI
{
    public partial class QL_Sach_TacGia : Window
    {
        private int? _maSach;
        public List<CTacGiaChon> ListTacGiaChon { get; set; }

        // Khai báo biến để trả về danh sách đã chọn
        public List<int> SelectedIds { get; set; }

        // Hàm khởi tạo mới: Thêm tham số dsTam cho trường hợp thêm sách mới
        public QL_Sach_TacGia(int? maSach, string tenSach, List<int> dsTam = null)
        {
            InitializeComponent();
            _maSach = maSach;
            lblTenSach.Text = $"Sách: {tenSach ?? "Sách mới chưa lưu"}";

            var tatCaTG = CXuLyTacGia.getdstg();
            List<int> idsDaChon = new List<int>();

            // Lấy dữ liệu: Từ DB nếu đang sửa sách, hoặc từ dsTam nếu đang thêm mới
            if (maSach.HasValue)
            {
                idsDaChon = CXuLySachTacGia.getMaTGCuaSach(maSach.Value);
            }
            else if (dsTam != null)
            {
                idsDaChon = dsTam;
            }

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
            // Lấy danh sách ID các tác giả được tick
            SelectedIds = ListTacGiaChon.Where(x => x.IsSelected).Select(x => x.MaTg).ToList();

            // Nếu là sách đã có trong DB thì gọi API lưu luôn
            if (_maSach.HasValue)
            {
                CXuLySachTacGia.capNhatTacGia(_maSach.Value, SelectedIds);
                MessageBox.Show("Cập nhật tác giả thành công!");
            }

            // Đóng cửa sổ và báo là thao tác thành công
            this.DialogResult = true;
        }

        private void btnDong_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}