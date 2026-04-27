using QLTV_WPF.Models;
using QLTV_WPF.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QLTV_WPF.Models_API
{
    class TacGiaVM : CBaseMVVM
    {
        public TacGiaVM()
        {
            ListTacGia = CXuLyTacGia.getdstg();

            cmdthemtg = new RelayCommand(ThemTG_Execute, p => true);
            cmdsuatg = new RelayCommand(SuaTG_Execute, p => SelectedTacGia != null);
            cmdxoatg = new RelayCommand(XoaTG_Execute, p => SelectedTacGia != null);
            cmdlammoi = new RelayCommand(p => LamMoi(), p => true);
            cmdTimKiem = new RelayCommand(p => TimKiem_Execute(), p => true);
        }

        public RelayCommand cmdthemtg { get; set; }
        public RelayCommand cmdsuatg { get; set; }
        public RelayCommand cmdxoatg { get; set; }
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdTimKiem { get; set; }

        private List<TacGia> m_listTacGia;
        public List<TacGia> ListTacGia
        {
            get => m_listTacGia;
            set { m_listTacGia = value; NotifyPropertyChanged("ListTacGia"); }
        }

        private string m_tentg;
        public string Tentg { get => m_tentg; set { m_tentg = value; NotifyPropertyChanged("Tentg"); } }

        private string m_tieusu;
        public string Tieusu { get => m_tieusu; set { m_tieusu = value; NotifyPropertyChanged("Tieusu"); } }

        private string m_tuKhoa;
        public string TuKhoa
        {
            get => m_tuKhoa;
            set { m_tuKhoa = value; NotifyPropertyChanged("TuKhoa"); }
        }

        private TacGia m_selectedTacGia;
        public TacGia SelectedTacGia
        {
            get => m_selectedTacGia;
            set
            {
                m_selectedTacGia = value;
                if (value != null) { Tentg = value.TenTg; Tieusu = value.TieuSu; }
                NotifyPropertyChanged("SelectedTacGia");
            }
        }

        private void ThemTG_Execute(object p)
        {
            if (string.IsNullOrWhiteSpace(Tentg))
            {
                MessageBox.Show("Vui lòng nhập tên tác giả!", "Cảnh báo");
                return;
            }

            TacGia moi = new TacGia { TenTg = Tentg, TieuSu = Tieusu };
            if (CXuLyTacGia.themtg(moi))
            {
                ListTacGia = CXuLyTacGia.getdstg();
                LamMoi();
                MessageBox.Show("Thêm thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Vui lòng kiểm tra lại.", "Lỗi");
            }
        }

        private void SuaTG_Execute(object p)
        {
            TacGia update = new TacGia { MaTg = SelectedTacGia.MaTg, TenTg = Tentg, TieuSu = Tieusu };
            if (CXuLyTacGia.suatg(update))
            {
                ListTacGia = CXuLyTacGia.getdstg();
                LamMoi();
                MessageBox.Show("Sửa thành công!", "Thông báo");
            }
            else
                {
                    MessageBox.Show("Sửa thất bại! Vui lòng kiểm tra lại.", "Lỗi");
            }
        }

        private void XoaTG_Execute(object p)
        {
            if (MessageBox.Show("Xóa tác giả này?", "Cảnh báo", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXuLyTacGia.xoatg(SelectedTacGia.MaTg))
                {
                    ListTacGia = CXuLyTacGia.getdstg();
                    LamMoi();
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! Vui lòng kiểm tra lại.", "Lỗi");
                }
            }
        }

        private void TimKiem_Execute()
        {
            var tatCaTacGia = CXuLyTacGia.getdstg();
            if (string.IsNullOrWhiteSpace(TuKhoa))
            {
                ListTacGia = tatCaTacGia;
            }
            else
            {
                ListTacGia = tatCaTacGia.Where(x => x.TenTg != null && x.TenTg.ToLower().Contains(TuKhoa.ToLower())).ToList();
            }
        }
        private void LamMoi()
        {
            Tentg = "";
            Tieusu = "";
            TuKhoa = "";
            SelectedTacGia = null;
            ListTacGia = CXuLyTacGia.getdstg();
        }
    }
}