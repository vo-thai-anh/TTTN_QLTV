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
            ChucVu = "Nhân viên";

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

        void LoadData()
        {
            var ds = CXuLyNhanVien.GetDsNhanVien();

            if (ds != null)
            {
                // ql->nv->a-z
                ListNhanVien = ds.OrderBy(x => x.ChucVu == "Quản lý" ? 0 : 1)
                                 .ThenBy(x => x.HoTen)
                                 .ToList();
            }
        }
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

            // Gán cứng chức vụ khi tạo mới
            var nv = new NhanVien
            {
                MaNv = 0,
                HoTen = HoTen,
                Sdt = Sdt,
                Email = Email,
                ChucVu = "Nhân viên", // Luôn là nhân viên
                MatKhau = MatKhau,
                TaiKhoan = TaiKhoan
            };

            if (CXuLyNhanVien.ThemNhanVien(nv))
            {
                MessageBox.Show("Thêm nhân viên mới thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
        }

        void Sua()
        {
            // 1. Kiểm tra xem đã chọn nhân viên chưa
            if (SelectedNhanVien == null) return;



            // 3. Kiểm tra các ràng buộc nhập liệu (giữ lại hàm cũ của ông)
            if (!KiemTraNhapLieu(false)) return;

            // 4. Cảnh báo khi thay đổi thông tin nhạy cảm (Tài khoản/Mật khẩu)
            bool isDoiTaiKhoan = TaiKhoan != SelectedNhanVien.TaiKhoan;
            bool isDoiMatKhau = !string.IsNullOrWhiteSpace(MatKhau);

            if (isDoiTaiKhoan || isDoiMatKhau)
            {
                string noiDungCanhBao = "Bạn đang thay đổi thông tin đăng nhập quan trọng:";
                if (isDoiTaiKhoan) noiDungCanhBao += "\n- Tên đăng nhập (Username)";
                if (isDoiMatKhau) noiDungCanhBao += "\n- Mật khẩu mới (Password)";
                noiDungCanhBao += "\n\nViệc này có thể khiến nhân viên không thể đăng nhập bằng thông tin cũ. Bạn có chắc chắn?";

                var result = MessageBox.Show(noiDungCanhBao, "Xác nhận thay đổi",
                                             MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No) return;
            }

            // 5. Chuẩn bị dữ liệu gửi đi
            var nv = new NhanVien
            {
                MaNv = SelectedNhanVien.MaNv,
                HoTen = HoTen,
                Sdt = Sdt,
                Email = Email,
                // Ép chức vụ về "Nhân viên" để đảm bảo không ai leo quyền được
                ChucVu = ChucVu,
                MatKhau = MatKhau,
                TaiKhoan = TaiKhoan
            };

            // 6. Thực hiện gọi API qua lớp xử lý
            if (CXuLyNhanVien.SuaNhanVien(nv))
            {
                MessageBox.Show("Cập nhật thông tin nhân viên thành công!", "Thông báo");
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
            // 1. Kiểm tra xem đã chọn nhân viên từ danh sách chưa
            if (SelectedNhanVien == null) return;

            // 2. CHẶN XÓA QUẢN LÝ: Quét chức vụ của người đang được chọn
            if (string.Equals(SelectedNhanVien.ChucVu, "Quản lý", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(SelectedNhanVien.ChucVu, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Đây là tài khoản Quản lý hệ thống. Bạn không có quyền xóa tài khoản này!",
                                "Lỗi bảo mật", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            // 3. Nếu là nhân viên bình thường, hiện bảng xác nhận như cũ
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên: {SelectedNhanVien.HoTen}?\nLưu ý: Hành động này không thể hoàn tác.",
                                         "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // 4. Gọi API xóa
                bool success = CXuLyNhanVien.XoaNhanVien(SelectedNhanVien.MaNv);

                if (success)
                {
                    MessageBox.Show("Đã xóa nhân viên thành công!", "Thông báo");
                    LoadData(); // Load lại danh sách mới
                    LamMoi();   // Xóa sạch form nhập liệu
                }
                else
                {
                    // Giữ lại logic thông báo lỗi ràng buộc dữ liệu (FK)
                    MessageBox.Show("Xóa thất bại! Nhân viên này đang xử lý Phiếu mượn.",
                                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        void LamMoi()
        {
            HoTen = Sdt = Email = MatKhau = TaiKhoan = Keyword = string.Empty;
            ChucVu = "Nhân viên"; // Luôn reset về Nhân viên sau khi thêm hoặc làm mới
            SelectedNhanVien = null;
            LoadData();
        }
    }
}