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
            if (string.IsNullOrWhiteSpace(Tenloai))
                return false;

            if (ListloaiSach != null && ListloaiSach.Any(x => x.TenLoai.Trim().ToLower() == Tenloai.Trim().ToLower()))
            {
                return false;
            }

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

                ListloaiSach = CXuLyLoaiSach.getdsls();

                SelectedLoaiSach = null;

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
            if (SelectedLoaiSach == null) return false;

            if (string.IsNullOrWhiteSpace(Tenloai)) return false;


            if (ListloaiSach != null)
            {
                bool biTrung = ListloaiSach.Any(x =>
                    x.MaLoai != SelectedLoaiSach.MaLoai &&
                    x.TenLoai.Trim().ToLower() == Tenloai.Trim().ToLower());

                if (biTrung) return false;
            }

            return true;
        }
        public void XoaLS_Execute(object parameter)
        {
            if (SelectedLoaiSach.Saches != null && SelectedLoaiSach.Saches.Count > 0)
            {
                System.Windows.MessageBox.Show("Không thể xóa! Loại sách này đang chứa " + SelectedLoaiSach.Saches.Count + " cuốn sách.",
                    "Cảnh báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                return;
            }
            var xacNhan = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa loại sách này?", "Xác nhận",
                System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (xacNhan == System.Windows.MessageBoxResult.Yes)
            {
                bool thanhCong = CXuLyLoaiSach.xoals(SelectedLoaiSach.MaLoai);
                if (thanhCong)
                {
                    RefreshData("Xóa thành công!");
                }
                else
                {
                    System.Windows.MessageBox.Show("Xóa thất bại! Có lỗi hệ thống hoặc dữ liệu liên quan không thể xóa.", "Lỗi");
                }
            }
        }
        public bool XoaLS_CanExecute(object parameter)
        {
            if (SelectedLoaiSach == null) return false;

            if (SelectedLoaiSach.Saches != null && SelectedLoaiSach.Saches.Any())
                return false;

            return true;
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
