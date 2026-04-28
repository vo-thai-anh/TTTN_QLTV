using QLTV_WPF.Models;
using QLTV_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLTV_WPF.Models_API
{
    class SachVM : CBaseMVVM
    {
        // 🔐 ROLE
        public string UserRole { get; set; } = "Nhân viên"; // mặc định

        public bool IsQuanLy => UserRole == "Quản lý";

        // ================= CONSTRUCTOR =================
        public SachVM()
        {
            ListSach = CXuLySach.getdssach();
            ListLoaiSach = CXuLyLoaiSach.getdsls();
            ListNXB = CXuLyNhaXuatBan.getdsnxb();

            cmdthemsach = new RelayCommand(ThemSach_Execute, ThemSach_CanExecute);
            cmdsuasach = new RelayCommand(SuaSach_Execute, SuaSach_CanExecute);
            cmdxoasach = new RelayCommand(XoaSach_Execute, XoaSach_CanExecute);
            cmdlammoi = new RelayCommand(p => LamMoi());
            cmdMoKhoSach = new RelayCommand(MoKhoSach_Execute, MoKhoSach_CanExecute);
            cmdsearch = new RelayCommand(p => Search());

            cmdQuanLyTacGia = new RelayCommand(p =>
            {
                if (!IsQuanLy) return; // 🔐 CHẶN NHÂN VIÊN

                if (SelectedSach != null)
                {
                    var popup = new UI.QL_Sach_TacGia(SelectedSach.MaSach, SelectedSach.TenSach);
                    popup.ShowDialog();
                    ListSach = CXuLySach.getdssach();
                }
                else
                {
                    var popup = new UI.QL_Sach_TacGia(null, Tensach, m_dsTacGiaTam);
                    if (popup.ShowDialog() == true)
                    {
                        m_dsTacGiaTam = popup.SelectedIds;
                    }
                }
            }, p => IsQuanLy); // 🔐 chỉ quản lý
        }

        // ================= COMMAND =================
        public RelayCommand cmdthemsach { get; set; }
        public RelayCommand cmdsuasach { get; set; }
        public RelayCommand cmdxoasach { get; set; }
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdMoKhoSach { get; set; }
        public RelayCommand cmdQuanLyTacGia { get; set; }
        public RelayCommand cmdsearch { get; set; }

        // ================= DATA =================
        private List<Sach> m_listSach;
        public List<Sach> ListSach
        {
            get => m_listSach;
            set { m_listSach = value; NotifyPropertyChanged("ListSach"); }
        }

        private List<int> m_dsTacGiaTam = new List<int>();

        private List<LoaiSach> m_listLoaiSach;
        public List<LoaiSach> ListLoaiSach
        {
            get => m_listLoaiSach;
            set { m_listLoaiSach = value; NotifyPropertyChanged("ListLoaiSach"); }
        }

        private List<NhaXuatBan> m_listNXB;
        public List<NhaXuatBan> ListNXB
        {
            get => m_listNXB;
            set { m_listNXB = value; NotifyPropertyChanged("ListNXB"); }
        }

        // ================= FORM =================
        public string Tensach { get; set; }
        public int? Namxb { get; set; }
        public int? Sotrang { get; set; }
        public string Tomtat { get; set; }
        public int? Soluong { get; set; }
        public int? Maloai { get; set; }
        public int? Manxb { get; set; }

        private string _keyword;
        public string Keyword
        {
            get => _keyword;
            set
            {
                _keyword = value;
                NotifyPropertyChanged("Keyword");
                Search();
            }
        }

        private Sach m_selectedSach;
        public Sach SelectedSach
        {
            get => m_selectedSach;
            set
            {
                m_selectedSach = value;

                if (value != null)
                {
                    Tensach = value.TenSach;
                    Namxb = value.NamXb;
                    Sotrang = value.SoTrang;
                    Tomtat = value.TomTat;
                    Soluong = value.SoLuong;
                    Maloai = value.MaLoai;
                    Manxb = value.MaNxb;
                }

                NotifyPropertyChanged("SelectedSach");
            }
        }

        // ================= THÊM =================
        public void ThemSach_Execute(object parameter)
        {
            if (!IsQuanLy) return; 

            if (string.IsNullOrWhiteSpace(Tensach))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập Tên sách!");
                return;
            }

            CSach moi = new CSach
            {
                TenSach = Tensach,
                NamXb = Namxb,
                SoTrang = Sotrang,
                TomTat = Tomtat,
                SoLuong = Soluong,
                MaLoai = Maloai,
                MaNxb = Manxb,
                MaTGIds = m_dsTacGiaTam
            };

            if (CXuLySach.themsach(moi))
            {
                ListSach = CXuLySach.getdssach();
                LamMoi();
                m_dsTacGiaTam.Clear();
            }
        }

        public bool ThemSach_CanExecute(object parameter)
        {
            return IsQuanLy;
        }

        // ================= SỬA =================
        public void SuaSach_Execute(object parameter)
        {
            if (!IsQuanLy) return;

            if (SelectedSach == null) return;

            CSach update = new CSach
            {
                MaSach = SelectedSach.MaSach,
                TenSach = Tensach,
                NamXb = Namxb,
                SoTrang = Sotrang,
                TomTat = Tomtat,
                SoLuong = Soluong,
                MaLoai = Maloai,
                MaNxb = Manxb
            };

            if (CXuLySach.suasach(update))
            {
                ListSach = CXuLySach.getdssach();
                LamMoi();
            }
        }

        public bool SuaSach_CanExecute(object parameter)
        {
            return SelectedSach != null && IsQuanLy;
        }

        // ================= XÓA =================
        public void XoaSach_Execute(object parameter)
        {
            if (!IsQuanLy) return;

            if (SelectedSach == null) return;

            if (CXuLySach.xoasach(SelectedSach.MaSach))
            {
                ListSach = CXuLySach.getdssach();
                LamMoi();
            }
        }

        public bool XoaSach_CanExecute(object parameter)
        {
            return SelectedSach != null && IsQuanLy;
        }

        // ================= KHÁC =================
        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword))
            {
                ListSach = CXuLySach.getdssach();
                return;
            }

            string lower = Keyword.ToLower();

            ListSach = CXuLySach.getdssach().Where(x =>
                (x.TenSach ?? "").ToLower().Contains(lower) ||
                (x.TenTacGia ?? "").ToLower().Contains(lower) ||
                (x.TenLoai ?? "").ToLower().Contains(lower)
            ).ToList();
        }

        private void LamMoi()
        {
            Tensach = "";
            Namxb = null;
            Sotrang = null;
            Tomtat = "";
            Soluong = null;
            Maloai = null;
            Manxb = null;
            SelectedSach = null;
        }

        public void MoKhoSach_Execute(object parameter)
        {
            if (SelectedSach == null) return;

            new UI.QL_SachMuon(SelectedSach.MaSach).ShowDialog();
        }

        public bool MoKhoSach_CanExecute(object parameter)
        {
            return SelectedSach != null;
        }
    }
}