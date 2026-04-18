using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace QLTV_WPF.ViewModels
{
    class DocGiaVM : CBaseMVVM
    {
        public DocGiaVM()
        {
            LoadData();

            // Logic khóa nút: Chọn dòng thì khóa Thêm, Làm mới thì khóa Sửa/Xóa
            cmdThem = new RelayCommand(p => Them(), p => SelectedDocGia == null);
            cmdSua = new RelayCommand(p => Sua(), p => SelectedDocGia != null);
            cmdXoa = new RelayCommand(p => Xoa(), p => SelectedDocGia != null);
            cmdLamMoi = new RelayCommand(p => LamMoi());
            cmdSearch = new RelayCommand(p => Search());
        }

        #region Properties & Commands
        public RelayCommand cmdThem { get; set; }
        public RelayCommand cmdSua { get; set; }
        public RelayCommand cmdXoa { get; set; }
        public RelayCommand cmdLamMoi { get; set; }
        public RelayCommand cmdSearch { get; set; }

        private List<DocGia> _listDocGia;
        public List<DocGia> ListDocGia
        {
            get => _listDocGia;
            set { _listDocGia = value; NotifyPropertyChanged("ListDocGia"); }
        }

        private string _hoTen, _sdt, _email, _diaChi, _keyword;
        public string HoTen { get => _hoTen; set { _hoTen = value; NotifyPropertyChanged("HoTen"); } }
        public string Sdt { get => _sdt; set { _sdt = value; NotifyPropertyChanged("Sdt"); } }
        public string Email { get => _email; set { _email = value; NotifyPropertyChanged("Email"); } }
        public string DiaChi { get => _diaChi; set { _diaChi = value; NotifyPropertyChanged("DiaChi"); } }
        public string Keyword { get => _keyword; set { _keyword = value; NotifyPropertyChanged("Keyword"); } }

        private DocGia _selectedDocGia;
        public DocGia SelectedDocGia
        {
            get => _selectedDocGia;
            set
            {
                _selectedDocGia = value;
                if (value != null)
                {
                    HoTen = value.HoTen;
                    Sdt = value.Sdt;
                    Email = value.Email;
                    DiaChi = value.DiaChi;
                }
                NotifyPropertyChanged("SelectedDocGia");
                // Ép giao diện cập nhật trạng thái các nút bấm ngay lập tức
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion

        #region Methods
        void LoadData() => ListDocGia = CXuLyDocGia.getdsdg();

        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) { LoadData(); return; }
            ListDocGia = CXuLyDocGia.searchdg(Keyword.Trim());
        }

        // HÀM KIỂM TRA NHẬP LIỆU CHUẨN
        bool KiemTraNhapLieu()
        {
            if (string.IsNullOrWhiteSpace(HoTen) || string.IsNullOrWhiteSpace(Sdt))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường có dấu *.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Regex Họ tên: Không số, không ký tự đặc biệt, hỗ trợ tiếng Việt
            string patternTen = @"^[a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ\s]+$";
            if (!Regex.IsMatch(HoTen.Trim(), patternTen))
            {
                MessageBox.Show("Họ tên không được chứa số hoặc ký tự!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Regex SĐT: Đúng 10 chữ số
            if (!Regex.IsMatch(Sdt.Trim(), @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại phải nhập đúng 10 số!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        void Them()
        {
            if (!KiemTraNhapLieu()) return;

            var dg = new DocGia
            {
                MaDocGia = 0,
                HoTen = HoTen.Trim(),
                Sdt = Sdt.Trim(),
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
                DiaChi = DiaChi?.Trim()
            };

            if (CXuLyDocGia.themdg(dg))
            {
                MessageBox.Show("Thêm độc giả mới thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!", "Lỗi");
            }
        }

        void Sua()
        {
            if (SelectedDocGia == null || !KiemTraNhapLieu()) return;

            var dg = new DocGia
            {
                MaDocGia = SelectedDocGia.MaDocGia,
                HoTen = HoTen.Trim(),
                Sdt = Sdt.Trim(),
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
                DiaChi = DiaChi?.Trim()
            };

            if (CXuLyDocGia.suadg(dg))
            {
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo");
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
            if (SelectedDocGia == null) return;

            var result = MessageBox.Show($"Xác nhận xóa độc giả: {SelectedDocGia.HoTen}?",
                                         "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (CXuLyDocGia.xoadg(SelectedDocGia.MaDocGia))
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                    LoadData();
                    LamMoi();
                }
                else
                {
                    MessageBox.Show("Không thể xóa độc giả này!",
                                    "Xóa thất bại", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        void LamMoi()
        {
            HoTen = Sdt = Email = DiaChi = Keyword = string.Empty;
            SelectedDocGia = null;
            LoadData();
        }
        #endregion
    }
}