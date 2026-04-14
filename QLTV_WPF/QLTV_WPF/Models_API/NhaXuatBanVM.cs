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
            cmdthemnxb = new RelayCommand(ThemNXB_Execute, p => true);
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

        private void ThemNXB_Execute(object p)
        {
            if (string.IsNullOrWhiteSpace(Tennxb))
            {
                MessageBox.Show("Vui lòng nhập tên nhà xuất bản!", "Cảnh báo");
                return;
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
            NhaXuatBan update = new NhaXuatBan { MaNxb = SelectedNXB.MaNxb, TenNxb = Tennxb, DiaChi = Diachi, Sdt = Sdt };
            if (CXuLyNhaXuatBan.suanxb(update))
            {
                LamMoi();
                MessageBox.Show("Sửa thành công!", "Thông báo");
            }
        }

        private void XoaNXB_Execute(object p)
        {
            if (MessageBox.Show("Xóa nhà xuất bản này?", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXuLyNhaXuatBan.xoanxb(SelectedNXB.MaNxb))
                {
                    LamMoi();
                }
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
            Manxb = null; // Đảm bảo ô ID được xóa trắng
            Tennxb = "";
            Diachi = "";
            Sdt = "";
            TuKhoa = "";
            SelectedNXB = null;
            ListNhaXuatBan = CXuLyNhaXuatBan.getdsnxb();
        }
    }
}
