using QLTV_WPF.Models;
using QLTV_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QLTV_WPF.Models_API
{
    class NhaXuatBanVM : CBaseMVVM
    {
        public NhaXuatBanVM()
        {
            ListNhaXuatBan = CXuLyNhaXuatBan.getdsnxb();

            // CHỈ CHO PHÉP THÊM KHI ĐANG KHÔNG CHỌN NXB NÀO
            cmdthemnxb = new RelayCommand(ThemNXB_Execute, p => SelectedNXB == null);

            cmdsuanxb = new RelayCommand(SuaNXB_Execute, p => SelectedNXB != null);
            cmdxoanxb = new RelayCommand(XoaNXB_Execute, p => SelectedNXB != null);
            cmdlammoi = new RelayCommand(p => LamMoi(), p => true);

            cmdTimKiem = new RelayCommand(p => TimKiem_Execute(), p => true);
        }

        public RelayCommand cmdthemnxb { get; set; }
        public RelayCommand cmdsuanxb { get; set; }
        public RelayCommand cmdxoanxb { get; set; }
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdTimKiem { get; set; }

        private List<NhaXuatBan> m_listNXB;
        public List<NhaXuatBan> ListNhaXuatBan { get => m_listNXB; set { m_listNXB = value; NotifyPropertyChanged("ListNhaXuatBan"); } }


        private int? m_manxb;
        public int? Manxb { get => m_manxb; set { m_manxb = value; NotifyPropertyChanged("Manxb"); } }

        private string m_tennxb;
        public string Tennxb { get => m_tennxb; set { m_tennxb = value; NotifyPropertyChanged("Tennxb"); } }

        private string m_diachi;
        public string Diachi { get => m_diachi; set { m_diachi = value; NotifyPropertyChanged("Diachi"); } }

        private string m_sdt;
        public string Sdt { get => m_sdt; set { m_sdt = value; NotifyPropertyChanged("Sdt"); } }

        private string m_tuKhoa;
        public string TuKhoa { get => m_tuKhoa; set { m_tuKhoa = value; NotifyPropertyChanged("TuKhoa"); } }

        private NhaXuatBan m_selectedNXB;
        public NhaXuatBan SelectedNXB
        {
            get => m_selectedNXB;
            set
            {
                m_selectedNXB = value;
                if (value != null)
                {
                    Manxb = value.MaNxb;
                    Tennxb = value.TenNxb;
                    Diachi = value.DiaChi;
                    Sdt = value.Sdt;
                }
                NotifyPropertyChanged("SelectedNXB");
            }
        }

        // HÀM KIỂM TRA RỖNG VÀ KIỂU DỮ LIỆU
        private bool KiemTraRong()
        {
            if (string.IsNullOrWhiteSpace(Tennxb))
            {
                MessageBox.Show("Vui lòng nhập Tên nhà xuất bản!", "Cảnh báo");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Diachi))
            {
                MessageBox.Show("Vui lòng nhập Địa chỉ!", "Cảnh báo");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Sdt))
            {
                MessageBox.Show("Vui lòng nhập Số điện thoại!", "Cảnh báo");
                return false;
            }

            if (!Sdt.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng chỉ nhập số.", "Cảnh báo");
                return false;
            }

            return true;
        }

        private void ThemNXB_Execute(object p)
        {
            if (!KiemTraRong()) return;

            var danhSach = CXuLyNhaXuatBan.getdsnxb();
            if (danhSach != null)
            {
                
                if (danhSach.Any(x => x.DiaChi != null && x.DiaChi.ToLower() == Diachi.ToLower()))
                {
                    MessageBox.Show("Địa chỉ này đã được sử dụng cho một Nhà xuất bản khác!", "Cảnh báo");
                    return;
                }
                if (danhSach.Any(x => x.Sdt != null && x.Sdt == Sdt))
                {
                    MessageBox.Show("Số điện thoại này đã được đăng ký!", "Cảnh báo");
                    return;
                }
            }

            NhaXuatBan moi = new NhaXuatBan { TenNxb = Tennxb, DiaChi = Diachi, Sdt = Sdt };
            if (CXuLyNhaXuatBan.themnxb(moi))
            {
                LamMoi();
                MessageBox.Show("Thêm thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Vui lòng kiểm tra kết nối.", "Lỗi");
            }
        }

        private void SuaNXB_Execute(object p)
        {
            if (!KiemTraRong()) return;

            var danhSach = CXuLyNhaXuatBan.getdsnxb();
            if (danhSach != null)
            {
               
                int maHienTai = SelectedNXB.MaNxb;

                
                if (danhSach.Any(x => x.DiaChi != null && x.DiaChi.ToLower() == Diachi.ToLower() && x.MaNxb != maHienTai))
                {
                    MessageBox.Show("Địa chỉ này đã được sử dụng cho một Nhà xuất bản khác!", "Cảnh báo");
                    return;
                }
                if (danhSach.Any(x => x.Sdt != null && x.Sdt == Sdt && x.MaNxb != maHienTai))
                {
                    MessageBox.Show("Số điện thoại này đã được đăng ký!", "Cảnh báo");
                    return;
                }
            }

            NhaXuatBan update = new NhaXuatBan { MaNxb = SelectedNXB.MaNxb, TenNxb = Tennxb, DiaChi = Diachi, Sdt = Sdt };
            if (CXuLyNhaXuatBan.suanxb(update))
            {
                LamMoi();
                MessageBox.Show("Sửa thành công!", "Thông báo");
            }
        }

        private void XoaNXB_Execute(object p)
        {
            if (MessageBox.Show("Xóa nhà xuất bản này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXuLyNhaXuatBan.xoanxb(SelectedNXB.MaNxb))
                {
                    LamMoi();
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Xóa thất bại! Vui lòng kiểm tra kết nối.", "Lỗi");
            }
        }

        private void TimKiem_Execute()
        {
            var tatCa = CXuLyNhaXuatBan.getdsnxb();
            if (string.IsNullOrWhiteSpace(TuKhoa))
            {
                ListNhaXuatBan = tatCa;
            }
            else
            {
                ListNhaXuatBan = tatCa.Where(x => x.TenNxb.ToLower().Contains(TuKhoa.ToLower())).ToList();
            }
        }

        private void LamMoi()
        {
            Manxb = null;
            Tennxb = "";
            Diachi = "";
            Sdt = "";
            TuKhoa = "";
            SelectedNXB = null; // Trả về null để nút Thêm sáng lên lại
            ListNhaXuatBan = CXuLyNhaXuatBan.getdsnxb();
        }
    }
}