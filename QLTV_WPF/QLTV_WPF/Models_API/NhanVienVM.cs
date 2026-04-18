using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace QLTV_WPF.ViewModels
{
    class NhanVienVM : CBaseMVVM
    {
        public NhanVienVM()
        {
            LoadData();

            cmdThem = new RelayCommand(p => Them(), p => SelectedNhanVien == null);
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
                    TaiKhoan = value.TaiKhoan;
                    // KHI CHỌN NHÂN VIÊN ĐỂ SỬA, ĐỂ TRỐNG Ô MẬT KHẨU
                    MatKhau = string.Empty;
                }
                NotifyPropertyChanged("SelectedNhanVien");
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        void LoadData() => ListNhanVien = CXuLyNhanVien.GetDsNhanVien();

        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) { LoadData(); return; }
            ListNhanVien = CXuLyNhanVien.SearchNhanVien(Keyword);
        }

        bool KiemTraNhapLieu(bool isThem)
        {
            if (string.IsNullOrWhiteSpace(HoTen) || string.IsNullOrWhiteSpace(TaiKhoan) || string.IsNullOrWhiteSpace(ChucVu))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường có dấu *.", "Thông báo");
                return false;
            }

            //Mật khẩu: Chỉ bắt buộc khi Thêm mới
            if (isThem && string.IsNullOrWhiteSpace(MatKhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cho nhân viên mới!", "Thông báo");
                return false;
            }

            // Họ tên: Không chứa ký tự lạ
            string patternTen = @"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ\s]+$";
            if (!Regex.IsMatch(HoTen, patternTen))
            {
                MessageBox.Show("Họ tên không được chứa số hoặc ký tự đặc biệt!", "Thông báo");
                return false;
            }

            //  SĐT: Đúng 10 số
            if (!Regex.IsMatch(Sdt ?? "", @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại phải đúng 10 số!", "Thông báo");
                return false;
            }

            return true;
        }

        void Them()
        {
            if (!KiemTraNhapLieu(true)) return;

            var nv = new NhanVien { MaNv = 0, HoTen = HoTen, Sdt = Sdt, Email = Email, ChucVu = ChucVu, MatKhau = MatKhau, TaiKhoan = TaiKhoan };

            if (CXuLyNhanVien.ThemNhanVien(nv))
            {
                MessageBox.Show("Thêm thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!");
            }
        }

        void Sua()
        {
            if (SelectedNhanVien == null) return;
            if (!KiemTraNhapLieu(false)) return;

            // Kiểm tra xem Admin có thay đổi Tài khoản hoặc Mật khẩu không
            bool isDoiTaiKhoan = TaiKhoan != SelectedNhanVien.TaiKhoan;
            bool isDoiMatKhau = !string.IsNullOrWhiteSpace(MatKhau); 

            if (isDoiTaiKhoan || isDoiMatKhau)
            {
                string noiDungCanhBao = "Bạn đang thay đổi thông tin đăng nhập:";
                if (isDoiTaiKhoan) noiDungCanhBao += "\n- Tên đăng nhập";
                if (isDoiMatKhau) noiDungCanhBao += "\n- Mật khẩu mới";
                noiDungCanhBao += "\n\n Bạn có chắc chắn muốn thay đổi?";

                var result = MessageBox.Show(noiDungCanhBao, "Xác nhận thay đổi",
                                             MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;
            }
            var nv = new NhanVien
            {
                MaNv = SelectedNhanVien.MaNv,
                HoTen = HoTen,
                Sdt = Sdt,
                Email = Email,
                ChucVu = ChucVu,
                MatKhau = MatKhau,
                TaiKhoan = TaiKhoan
            };

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

            var result = MessageBox.Show($"Xác nhận xóa nhân viên: {SelectedNhanVien.HoTen}?",
                                         "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool success = CXuLyNhanVien.XoaNhanVien(SelectedNhanVien.MaNv);

                if (success)
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                    LoadData();
                    LamMoi();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! Nhân viên này đang cho mượn mượn sách.",
                                    "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        void LamMoi()
        {
            HoTen = Sdt = Email = ChucVu = MatKhau = TaiKhoan = Keyword = string.Empty;
            SelectedNhanVien = null;
            LoadData();
        }
    }
}