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

            cmdthemtg = new RelayCommand(ThemTG_Execute, p => SelectedTacGia == null);

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
        public List<TacGia> ListTacGia { get => m_listTacGia; set { m_listTacGia = value; NotifyPropertyChanged("ListTacGia"); } }

        private string m_tentg;
        public string Tentg { get => m_tentg; set { m_tentg = value; NotifyPropertyChanged("Tentg"); } }

        private string m_butDanh;
        public string ButDanh { get => m_butDanh; set { m_butDanh = value; NotifyPropertyChanged("ButDanh"); } }

        private string m_strNamSinh;
        public string StrNamSinh { get => m_strNamSinh; set { m_strNamSinh = value; NotifyPropertyChanged("StrNamSinh"); } }

        private string m_tieusu;
        public string Tieusu { get => m_tieusu; set { m_tieusu = value; NotifyPropertyChanged("Tieusu"); } }

        private string m_tuKhoa;
        public string TuKhoa { get => m_tuKhoa; set { m_tuKhoa = value; NotifyPropertyChanged("TuKhoa"); } }

        private TacGia m_selectedTacGia;
        public TacGia SelectedTacGia
        {
            get => m_selectedTacGia;
            set
            {
                m_selectedTacGia = value;
                if (value != null)
                {
                    Tentg = value.TenTg;
                    Tieusu = value.TieuSu;
                    ButDanh = value.Butdanh;
                    StrNamSinh = value.Namsinh?.ToString();
                }
                NotifyPropertyChanged("SelectedTacGia");
            }
        }

        private bool KiemTraRong()
        {
            if (string.IsNullOrWhiteSpace(Tentg))
            {
                MessageBox.Show("Vui lòng nhập Tên tác giả!", "Cảnh báo");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ButDanh))
            {
                MessageBox.Show("Vui lòng nhập Bút danh!", "Cảnh báo");
                return false;
            }

            // KIỂM TRA NĂM SINH
            if (string.IsNullOrWhiteSpace(StrNamSinh))
            {
                MessageBox.Show("Vui lòng nhập Năm sinh!", "Cảnh báo");
                return false;
            }
            if (!int.TryParse(StrNamSinh, out int ns))
            {
                MessageBox.Show("Năm sinh không hợp lệ! Vui lòng chỉ nhập số (Ví dụ: 1990).", "Cảnh báo");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Tieusu))
            {
                MessageBox.Show("Vui lòng nhập Tiểu sử!", "Cảnh báo");
                return false;
            }
            return true;
        }

        private void ThemTG_Execute(object p)
        {
            if (!KiemTraRong()) return;

            var danhSachTG = CXuLyTacGia.getdstg();
            if (danhSachTG != null && danhSachTG.Any(x => x.Butdanh != null && x.Butdanh.ToLower() == ButDanh.ToLower()))
            {
                MessageBox.Show("Bút danh này đã tồn tại! Vui lòng chọn bút danh khác.", "Cảnh báo");
                return;
            }

            int namSinhInt = int.Parse(StrNamSinh); // Đã an toàn để ép kiểu
            TacGia moi = new TacGia { TenTg = Tentg, TieuSu = Tieusu, Butdanh = ButDanh, Namsinh = namSinhInt };
            if (CXuLyTacGia.themtg(moi))
            {
                ListTacGia = CXuLyTacGia.getdstg();
                LamMoi();
                MessageBox.Show("Thêm thành công!", "Thông báo");
            }
        }

        private void SuaTG_Execute(object p)
        {
            if (!KiemTraRong()) return;

            var danhSachTG = CXuLyTacGia.getdstg();
            if (danhSachTG != null && danhSachTG.Any(x => x.Butdanh != null && x.Butdanh.ToLower() == ButDanh.ToLower() && x.MaTg != SelectedTacGia.MaTg))
            {
                MessageBox.Show("Bút danh này đã tồn tại! Vui lòng chọn bút danh khác.", "Cảnh báo");
                return;
            }

            int namSinhInt = int.Parse(StrNamSinh);
            TacGia update = new TacGia { MaTg = SelectedTacGia.MaTg, TenTg = Tentg, TieuSu = Tieusu, Butdanh = ButDanh, Namsinh = namSinhInt };
            if (CXuLyTacGia.suatg(update))
            {
                ListTacGia = CXuLyTacGia.getdstg();
                LamMoi();
                MessageBox.Show("Sửa thành công!", "Thông báo");
            }
        }

        private void XoaTG_Execute(object p)
        {
            if (MessageBox.Show("Xóa tác giả này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXuLyTacGia.xoatg(SelectedTacGia.MaTg))
                {
                    ListTacGia = CXuLyTacGia.getdstg();
                    LamMoi();
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
            ButDanh = "";
            StrNamSinh = ""; 
            Tieusu = "";
            TuKhoa = "";
            SelectedTacGia = null; 
            ListTacGia = CXuLyTacGia.getdstg();
        }
    }
}