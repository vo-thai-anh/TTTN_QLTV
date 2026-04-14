using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System.Collections.Generic;
using System.Windows;

namespace QLTV_WPF.ViewModels
{
    class NhanVienVM : CBaseMVVM
    {
        public NhanVienVM()
        {
            LoadData();
            cmdThem = new RelayCommand(p => Them(), p => true);
            cmdSua = new RelayCommand(p => Sua(), p => SelectedNhanVien != null);
            cmdXoa = new RelayCommand(p => Xoa(), p => SelectedNhanVien != null);
            cmdLamMoi = new RelayCommand(p => LamMoi());
            cmdSearch = new RelayCommand(p => Search());
        }

        public RelayCommand cmdThem { get; set; }
        public RelayCommand cmdSua { get; set; }
        public RelayCommand cmdXoa { get; set; }
        public RelayCommand cmdLamMoi { get; set; }
        public RelayCommand cmdSearch { get; set; }

        private List<NhanVien> _listNhanVien;
        public List<NhanVien> ListNhanVien
        {
            get => _listNhanVien;
            set { _listNhanVien = value; NotifyPropertyChanged("ListNhanVien"); }
        }

        private string _hoTen, _sdt, _email, _chucVu, _matKhau, _taiKhoan, _keyword;
        public string HoTen { get => _hoTen; set { _hoTen = value; NotifyPropertyChanged("HoTen"); } }
        public string Sdt { get => _sdt; set { _sdt = value; NotifyPropertyChanged("Sdt"); } }
        public string Email { get => _email; set { _email = value; NotifyPropertyChanged("Email"); } }
        public string ChucVu { get => _chucVu; set { _chucVu = value; NotifyPropertyChanged("ChucVu"); } }
        public string MatKhau { get => _matKhau; set { _matKhau = value; NotifyPropertyChanged("MatKhau"); } }
        public string TaiKhoan { get => _taiKhoan; set { _taiKhoan = value; NotifyPropertyChanged("TaiKhoan"); } }
        public string Keyword { get => _keyword; set { _keyword = value; NotifyPropertyChanged("Keyword"); } }

        private NhanVien _selectedNhanVien;
        public NhanVien SelectedNhanVien
        {
            get => _selectedNhanVien;
            set
            {
                _selectedNhanVien = value;
                if (value != null)
                {
                    HoTen = value.HoTen;
                    Sdt = value.Sdt;
                    Email = value.Email;
                    ChucVu = value.ChucVu;
                    MatKhau = value.MatKhau;
                    TaiKhoan = value.TaiKhoan;
                }
                NotifyPropertyChanged("SelectedNhanVien");
            }
        }

        void LoadData() => ListNhanVien = CXuLyNhanVien.GetDsNhanVien();

        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) { LoadData(); return; }
            ListNhanVien = CXuLyNhanVien.SearchNhanVien(Keyword);
        }

        // HÀM KIỂM TRA CHUNG CHO CẢ THÊM VÀ SỬA
        bool KiemTraNhapLieu()
        {
            if (string.IsNullOrWhiteSpace(HoTen) || string.IsNullOrWhiteSpace(TaiKhoan) ||
                string.IsNullOrWhiteSpace(MatKhau) || string.IsNullOrWhiteSpace(ChucVu))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường có dấu *!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        void Them()
        {
            if (!KiemTraNhapLieu()) return;

            var nv = new NhanVien { MaNv = 0, HoTen = HoTen, Sdt = Sdt, Email = Email, ChucVu = ChucVu, MatKhau = MatKhau, TaiKhoan = TaiKhoan };

            if (CXuLyNhanVien.ThemNhanVien(nv))
            {
                MessageBox.Show("Thêm thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!", "Thông báo");
            }
        }

        void Sua()
        {
            if (SelectedNhanVien == null) return;
            if (!KiemTraNhapLieu()) return; // Kiểm tra trống khi sửa

            var nv = new NhanVien { MaNv = SelectedNhanVien.MaNv, HoTen = HoTen, Sdt = Sdt, Email = Email, ChucVu = ChucVu, MatKhau = MatKhau, TaiKhoan = TaiKhoan };

            if (CXuLyNhanVien.SuaNhanVien(nv))
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Thông báo");
            }
        }

        void Xoa()
        {
            if (SelectedNhanVien == null) return;
            if (MessageBox.Show("Xác nhận xóa nhân viên này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                if (CXuLyNhanVien.XoaNhanVien(SelectedNhanVien.MaNv)) { LoadData(); LamMoi(); }
        }

        void LamMoi()
        {
            HoTen = Sdt = Email = ChucVu = MatKhau = TaiKhoan = Keyword = string.Empty;
            SelectedNhanVien = null;
            LoadData();
        }
    }
}