using QLTV_WPF.Models;
using QLTV_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QLTV_WPF.Models_API
{
    class LoaiSachVM : CBaseMVVM
    {
        public LoaiSachVM()
        {
            ListloaiSach = CXuLyLoaiSach.getdsls();
            cmdthemls = new RelayCommand(ThemLS_Execute, ThemLS_CanExecute);
            cmdsuals = new RelayCommand(SuaLS_Execute,SuaLS_CanExecute);
            cmdxoals = new RelayCommand(XoaLS_Execute,XoaLS_CanExecute);
            cmdlammoi = new RelayCommand(p => LamMoi());
        }

        public RelayCommand cmdthemls { get; set; }
        public RelayCommand cmdsuals { get; set; }
        public RelayCommand cmdxoals { get; set; }
        public RelayCommand cmdlammoi { get; set; }

        private List<LoaiSach> m_listloaiSach;
        public List<LoaiSach> ListloaiSach
        {
            get { return m_listloaiSach; }
            set
            {
                m_listloaiSach = value;
                NotifyPropertyChanged("ListloaiSach");
            }
        }
        private string m_tenloai;
        public string Tenloai
        {
            get { return m_tenloai; }
            set { m_tenloai = value; NotifyPropertyChanged("Tenloai"); }
        }
        private string m_mota;
        public string Mota
        {
            get { return m_mota; }
            set { m_mota = value; NotifyPropertyChanged("Mota"); }
        }

        private LoaiSach m_selectedLoaiSach;
        public LoaiSach SelectedLoaiSach
        {
            get { return m_selectedLoaiSach; }
            set
            {
                m_selectedLoaiSach = value;
                if (value != null)
                {
                    Tenloai = value.TenLoai;
                    Mota = value.MoTa;
                }
                NotifyPropertyChanged("SelectedLoaiSach");
            }
        }
        public void ThemLS_Execute(object parametera)
        {
            Cloaisach moi = new Cloaisach
            {
                TenLoai = this.Tenloai,
                MoTa = this.Mota
            };

            bool thanhCong = CXuLyLoaiSach.themls(moi);

            if (thanhCong)
            {
                ListloaiSach = CXuLyLoaiSach.getdsls();

                Tenloai = string.Empty;
                Mota = string.Empty;

                System.Windows.MessageBox.Show("Thêm loại sách thành công!");
            }
            else
            {
                System.Windows.MessageBox.Show("Thêm thất bại. Vui lòng kiểm tra lại kết nối API.");
            }
        }
        public bool ThemLS_CanExecute(object parameter)
        {
            return true;
        }
        public void SuaLS_Execute(object parameter)
        {
            Cloaisach update = new Cloaisach
            {
                MaLoai = SelectedLoaiSach.MaLoai,
                TenLoai = this.Tenloai,
                MoTa = this.Mota
            };

            bool thanhCong = CXuLyLoaiSach.suals(update);

            if (thanhCong)
            {
                System.Windows.MessageBox.Show("Sửa loại sách thành công!", "Thông báo");

                // 1. Tải lại DataGrid
                ListloaiSach = CXuLyLoaiSach.getdsls();

                // 2. Ép DataGrid bỏ chọn dòng cũ
                SelectedLoaiSach = null;

                // 3. Làm rỗng TextBox (Bây giờ dây Binding đã nối lại, nó sẽ xóa chữ trên màn hình)
                Tenloai = string.Empty;
                Mota = string.Empty;
            }
            else
            {
                System.Windows.MessageBox.Show("Sửa thất bại. Vui lòng thử lại.", "Lỗi");
            }
        }
        public bool SuaLS_CanExecute(object parameter)
        {
            return SelectedLoaiSach != null;
        }
        public void XoaLS_Execute(object parameter)
        {
            // Hỏi lại người dùng cho chắc chắn trước khi xóa
            var xacNhan = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa loại sách này không?",
                                                         "Xác nhận xóa",
                                                         System.Windows.MessageBoxButton.YesNo,
                                                         System.Windows.MessageBoxImage.Warning);

            if (xacNhan == System.Windows.MessageBoxResult.Yes)
            {
                // Gọi API để xóa dựa vào Mã loại đang chọn
                bool thanhCong = CXuLyLoaiSach.xoals(SelectedLoaiSach.MaLoai);

                if (thanhCong)
                {
                    // Tải lại danh sách và xóa trắng form nhập liệu
                    ListloaiSach = CXuLyLoaiSach.getdsls();
                    Tenloai = string.Empty;
                    Mota = string.Empty;

                    System.Windows.MessageBox.Show("Xóa loại sách thành công!", "Thông báo");
                }
                else
                {
                    System.Windows.MessageBox.Show("Xóa thất bại. Có thể loại sách này đang chứa sách.", "Lỗi");
                }
            }
        }
        public bool XoaLS_CanExecute(object parameter)
        {
            return SelectedLoaiSach != null;
        }
        private void LamMoi()
        {
            Tenloai = string.Empty;
            Mota = string.Empty;
            SelectedLoaiSach = null;
        }
        private void RefreshData(string msg)
        {
            ListloaiSach = CXuLyLoaiSach.getdsls();
            LamMoi();
            System.Windows.MessageBox.Show(msg);
        }
    }
}
