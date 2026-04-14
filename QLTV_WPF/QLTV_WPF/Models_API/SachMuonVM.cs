using QLTV_WPF.Models;
using QLTV_WPF.ViewModels;
using System.Collections.Generic;

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
            ListSachMuon = CXuLySachMuon.getdssachmuon();
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
            cmdlammoi = new RelayCommand(p => LamMoi());
            if (maSachDefault != null)
            {
                Masach = maSachDefault;
            }
        }

        public RelayCommand cmdthemsachmuon { get; set; }
        public RelayCommand cmdsuasachmuon { get; set; }
        public RelayCommand cmdxoasachmuon { get; set; }
        public RelayCommand cmdlammoi { get; set; }

        private List<CSachMuon> m_listSachMuon;
        public List<CSachMuon> ListSachMuon
        {
            get { return m_listSachMuon; }
            set { m_listSachMuon = value; NotifyPropertyChanged("ListSachMuon"); }
        }

        private List<Sach> m_listSach;
        public List<Sach> ListSach
        {
            get { return m_listSach; }
            set { m_listSach = value; NotifyPropertyChanged("ListSach"); }
        }

        private List<TrangThaiItem> m_listTrangThai;
        public List<TrangThaiItem> ListTrangThai
        {
            get { return m_listTrangThai; }
            set { m_listTrangThai = value; NotifyPropertyChanged("ListTrangThai"); }
        }

        // --- Các biến Binding dữ liệu ---
        private int? m_masach;
        public int? Masach
        {
            get { return m_masach; }
            set { m_masach = value; NotifyPropertyChanged("Masach"); }
        }

        private string m_tinhtrang;
        public string Tinhtrang
        {
            get { return m_tinhtrang; }
            set { m_tinhtrang = value; NotifyPropertyChanged("Tinhtrang"); }
        }

        private int? m_trangthai;
        public int? Trangthai
        {
            get { return m_trangthai; }
            set { m_trangthai = value; NotifyPropertyChanged("Trangthai"); }
        }

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

        // --- Hàm thực thi ---
        public void Them_Execute(object parameter)
        {
            // Nếu người dùng không chọn trạng thái, mặc định là 0 (Sẵn sàng)
            int tt = this.Trangthai ?? 0;
            CSachMuon moi = new CSachMuon { MaSach = this.Masach, TinhTrang = this.Tinhtrang, TrangThai = tt };

            if (CXuLySachMuon.themsachmuon(moi))
            {
                ListSachMuon = CXuLySachMuon.getdssachmuon();
                LamMoi();
                System.Windows.MessageBox.Show("Nhập sách vào kho thành công!");
            }
        }
        public bool Them_CanExecute(object parameter) => true;

        public void Sua_Execute(object parameter)
        {
            CSachMuon update = new CSachMuon { MaSachMuon = SelectedSachMuon.MaSachMuon, MaSach = this.Masach, TinhTrang = this.Tinhtrang, TrangThai = this.Trangthai };
            if (CXuLySachMuon.suasachmuon(update))
            {
                ListSachMuon = CXuLySachMuon.getdssachmuon();
                LamMoi();
                System.Windows.MessageBox.Show("Sửa thành công!");
            }
        }
        public bool Sua_CanExecute(object parameter) => SelectedSachMuon != null;

        public void Xoa_Execute(object parameter)
        {
            if (System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                if (CXuLySachMuon.xoasachmuon(SelectedSachMuon.MaSachMuon))
                {
                    ListSachMuon = CXuLySachMuon.getdssachmuon();
                    LamMoi();
                    System.Windows.MessageBox.Show("Xóa thành công!");
                }
            }
        }
        public bool Xoa_CanExecute(object parameter) => SelectedSachMuon != null;

        private void LamMoi()
        {
            Masach = null;
            Tinhtrang = string.Empty;
            Trangthai = null;
            SelectedSachMuon = null;
        }
    }
}