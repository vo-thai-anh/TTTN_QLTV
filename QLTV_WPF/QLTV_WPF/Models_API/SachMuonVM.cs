using QLTV_WPF.Models;
using QLTV_WPF.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QLTV_WPF.Models_API
{
    // Class phụ hỗ trợ ComboBox Trạng thái
    public class TrangThaiItem
    {
        public int MaTT { get; set; }
        public string TenTT { get; set; }
    }

    class SachMuonVM : CBaseMVVM
    {
        public SachMuonVM(int? maSachDefault = null)
        {
            ListSach = CXuLySach.getdssach();
            ListTrangThai = new List<TrangThaiItem>
            {
                new TrangThaiItem { MaTT = 0, TenTT = "0 - Sẵn sàng" },
                new TrangThaiItem { MaTT = 1, TenTT = "1 - Đang mượn" },
                new TrangThaiItem { MaTT = 2, TenTT = "2 - Bảo trì" },
                new TrangThaiItem { MaTT = 3, TenTT = "3 - Mất" }
            };

            cmdthemsachmuon = new RelayCommand(Them_Execute, Them_CanExecute);
            cmdsuasachmuon = new RelayCommand(Sua_Execute, Sua_CanExecute);
            cmdxoasachmuon = new RelayCommand(Xoa_Execute, Xoa_CanExecute);

            // Bấm nút Làm mới trên UI sẽ xóa sạch sẽ để xem toàn bộ sách
            cmdlammoi = new RelayCommand(p => LamMoiHoanToan());

            if (maSachDefault != null)
            {
                Masach = maSachDefault; // Sẽ tự động trigger LoadData()
            }
            else
            {
                LoadData();
            }
        }

        public RelayCommand cmdthemsachmuon { get; set; }
        public RelayCommand cmdsuasachmuon { get; set; }
        public RelayCommand cmdxoasachmuon { get; set; }
        public RelayCommand cmdlammoi { get; set; }

        private List<CSachMuon> m_listSachMuon;
        public List<CSachMuon> ListSachMuon { get => m_listSachMuon; set { m_listSachMuon = value; NotifyPropertyChanged("ListSachMuon"); } }

        private List<Sach> m_listSach;
        public List<Sach> ListSach { get => m_listSach; set { m_listSach = value; NotifyPropertyChanged("ListSach"); } }

        private List<TrangThaiItem> m_listTrangThai;
        public List<TrangThaiItem> ListTrangThai { get => m_listTrangThai; set { m_listTrangThai = value; NotifyPropertyChanged("ListTrangThai"); } }

        // --- MẸO TRÁNH LỖI VÒNG LẶP DATAGRID ---
        private int? m_masach;
        public int? Masach
        {
            get { return m_masach; }
            set
            {
                // Chỉ chạy lệnh lọc khi Đầu sách THỰC SỰ bị thay đổi
                if (m_masach != value)
                {
                    m_masach = value;
                    NotifyPropertyChanged("Masach");
                    LoadData(); // Tự động lọc theo ComboBox
                }
            }
        }

        private string m_tinhtrang;
        public string Tinhtrang { get => m_tinhtrang; set { m_tinhtrang = value; NotifyPropertyChanged("Tinhtrang"); } }

        private int? m_trangthai;
        public int? Trangthai { get => m_trangthai; set { m_trangthai = value; NotifyPropertyChanged("Trangthai"); } }

        private CSachMuon m_selectedSachMuon;
        public CSachMuon SelectedSachMuon
        {
            get { return m_selectedSachMuon; }
            set
            {
                m_selectedSachMuon = value;
                if (value != null)
                {
                    Masach = value.MaSach;
                    Tinhtrang = value.TinhTrang;
                    Trangthai = value.TrangThai;
                }
                NotifyPropertyChanged("SelectedSachMuon");
            }
        }

        // --- HÀM TẢI & LỌC DỮ LIỆU ---
        private void LoadData()
        {
            var toanBo = CXuLySachMuon.getdssachmuon();
            if (m_masach != null && m_masach > 0)
            {
                // Chỉ lấy những cuốn sách vật lý thuộc Đầu sách đang chọn
                ListSachMuon = toanBo?.Where(x => x.MaSach == m_masach).ToList();
            }
            else
            {
                // Nếu không chọn Đầu sách nào, hiện tất cả
                ListSachMuon = toanBo;
            }
        }

        // --- HÀM THỰC THI ---
        public void Them_Execute(object parameter)
        {
            int tt = this.Trangthai ?? 0;
            CSachMuon moi = new CSachMuon { MaSach = this.Masach, TinhTrang = this.Tinhtrang, TrangThai = tt };

            if (CXuLySachMuon.themsachmuon(moi))
            {
                MessageBox.Show("Nhập sách vào kho thành công!");
                ClearForm(); // Chỉ xóa ô nhập liệu
                LoadData();  // Load lại danh sách NHƯNG GIỮ NGUYÊN BỘ LỌC
            }
        }
        public bool Them_CanExecute(object parameter) => Masach != null;

        public void Sua_Execute(object parameter)
        {
            if (SelectedSachMuon.TrangThai == 1)
            {
                MessageBox.Show("Cuốn sách này đang được độc giả mượn!\nBạn không thể sửa thông tin lúc này.", "Cảnh báo bảo mật", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CSachMuon update = new CSachMuon { MaSachMuon = SelectedSachMuon.MaSachMuon, MaSach = this.Masach, TinhTrang = this.Tinhtrang, TrangThai = this.Trangthai };
            if (CXuLySachMuon.suasachmuon(update))
            {
                MessageBox.Show("Sửa thành công!");
                ClearForm();
                LoadData(); // Giữ nguyên bộ lọc
            }
        }
        public bool Sua_CanExecute(object parameter) => SelectedSachMuon != null;

        public void Xoa_Execute(object parameter)
        {
            if (SelectedSachMuon.TrangThai == 1)
            {
                MessageBox.Show("Sách này đang nằm trong tay độc giả, không thể xóa khỏi hệ thống!", "Lỗi thao tác", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXuLySachMuon.xoasachmuon(SelectedSachMuon.MaSachMuon))
                {
                    MessageBox.Show("Xóa thành công!");
                    ClearForm();
                    LoadData(); // Giữ nguyên bộ lọc
                }
            }
        }
        public bool Xoa_CanExecute(object parameter) => SelectedSachMuon != null;

        // Chỉ xóa chữ ở ô Tình trạng và trạng thái để nhập cuốn tiếp theo
        private void ClearForm()
        {
            Tinhtrang = string.Empty;
            Trangthai = null;
            SelectedSachMuon = null;
        }

        // Bấm nút Làm Mới trên giao diện -> Reset toàn bộ để xem tất cả sách
        private void LamMoiHoanToan()
        {
            Masach = null;
            ClearForm();
            LoadData();
        }
    }
}