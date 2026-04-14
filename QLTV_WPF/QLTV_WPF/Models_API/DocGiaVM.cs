using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System.Collections.Generic;
using System.Windows;

namespace QLTV_WPF.ViewModels
{
    class DocGiaVM : CBaseMVVM
    {
        public DocGiaVM()
        {
            LoadData();
            // Giữ nguyên p => true để nút luôn sáng giống NhanVien
            cmdthem = new RelayCommand(p => Them(), p => true);
            cmdsua = new RelayCommand(p => Sua(), p => SelectedDocGia != null);
            cmdxoa = new RelayCommand(p => Xoa(), p => SelectedDocGia != null);
            cmdlammoi = new RelayCommand(p => LamMoi());
            cmdsearch = new RelayCommand(p => Search());
        }

        public RelayCommand cmdthem { get; set; }
        public RelayCommand cmdsua { get; set; }
        public RelayCommand cmdxoa { get; set; }
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdsearch { get; set; }

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
            }
        }

        void LoadData() => ListDocGia = CXuLyDocGia.getdsdg();

        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) { LoadData(); return; }
            ListDocGia = CXuLyDocGia.searchdg(Keyword);
        }

        // Kiểm tra bỏ trống (Dấu *)
        bool KiemTraNhapLieu()
        {
            if (string.IsNullOrWhiteSpace(HoTen))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường có dấu *!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        void Them()
        {
            if (!KiemTraNhapLieu()) return;

            // CHỐT CHẶN: Nếu đang chọn một dòng trong bảng (Selected khác null)
            // thì báo Thêm thất bại ngay, không gửi lên API để tránh trùng.
            if (SelectedDocGia != null)
            {
                MessageBox.Show("Thêm thất bại!", "Thông báo");
                return;
            }

            var dg = new DocGia { MaDocGia = 0, HoTen = HoTen, Sdt = Sdt, Email = Email, DiaChi = DiaChi };

            if (CXuLyDocGia.themdg(dg))
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
            if (SelectedDocGia == null) return;
            if (!KiemTraNhapLieu()) return;

            var dg = new DocGia { MaDocGia = SelectedDocGia.MaDocGia, HoTen = HoTen, Sdt = Sdt, Email = Email, DiaChi = DiaChi };

            if (CXuLyDocGia.suadg(dg))
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
            if (SelectedDocGia == null) return;
            if (MessageBox.Show("Xác nhận xóa độc giả này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                if (CXuLyDocGia.xoadg(SelectedDocGia.MaDocGia)) { LoadData(); LamMoi(); }
        }

        void LamMoi()
        {
            HoTen = Sdt = Email = DiaChi = Keyword = string.Empty;
            SelectedDocGia = null;
            LoadData();
        }
    }
}