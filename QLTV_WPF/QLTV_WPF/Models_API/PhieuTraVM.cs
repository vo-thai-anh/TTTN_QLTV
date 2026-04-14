using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System;
using System.Collections.Generic;
using System.Windows;

namespace QLTV_WPF.ViewModels
{
    class PhieuTraVM : CBaseMVVM
    {
        public PhieuTraVM()
        {
            LoadData();

            cmdthem = new RelayCommand(p => Them(), p => true);
            cmdsua = new RelayCommand(p => Sua(), p => Selected != null);
            cmdxoa = new RelayCommand(p => Xoa(), p => Selected != null);
            cmdlammoi = new RelayCommand(p => LamMoi());
        }

        public RelayCommand cmdthem { get; set; }
        public RelayCommand cmdsua { get; set; }
        public RelayCommand cmdxoa { get; set; }
        public RelayCommand cmdlammoi { get; set; }

        private List<PhieuTra> _list;
        public List<PhieuTra> List
        {
            get => _list;
            set { _list = value; NotifyPropertyChanged("List"); }
        }

        private int? _maNhanVien;
        private DateTime? _ngayTra = DateTime.Now;
        private decimal? _tongTienPhat;
        private string _ghiChu;

        public int? MaNhanVien { get => _maNhanVien; set { _maNhanVien = value; NotifyPropertyChanged("MaNhanVien"); } }
        public DateTime? NgayTra { get => _ngayTra; set { _ngayTra = value; NotifyPropertyChanged("NgayTra"); } }
        public decimal? TongTienPhat { get => _tongTienPhat; set { _tongTienPhat = value; NotifyPropertyChanged("TongTienPhat"); } }
        public string GhiChu { get => _ghiChu; set { _ghiChu = value; NotifyPropertyChanged("GhiChu"); } }

        private PhieuTra _selected;
        public PhieuTra Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (value != null)
                {
                    MaNhanVien = value.MaNhanVien;
                    NgayTra = value.NgayTra;
                    TongTienPhat = value.TongTienPhat;
                    GhiChu = value.GhiChu;
                }
                NotifyPropertyChanged("Selected");
            }
        }

        void LoadData() => List = CXuLyPhieuTra.getds();

        bool KiemTra()
        {
            if (MaNhanVien == null)
            {
                MessageBox.Show("Vui lòng nhập Mã nhân viên!", "Thông báo");
                return false;
            }
            return true;
        }

        void Them()
        {
            if (!KiemTra()) return;

            if (Selected != null)
            {
                MessageBox.Show("Thêm thất bại!", "Thông báo");
                return;
            }

            var pt = new PhieuTra
            {
                MaPhieuTra = 0,
                MaNhanVien = MaNhanVien,
                NgayTra = NgayTra,
                TongTienPhat = TongTienPhat,
                GhiChu = GhiChu
            };

            if (CXuLyPhieuTra.them(pt))
            {
                MessageBox.Show("Thêm thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else MessageBox.Show("Thêm thất bại!", "Thông báo");
        }

        void Sua()
        {
            if (Selected == null) return;
            if (!KiemTra()) return;

            var pt = new PhieuTra
            {
                MaPhieuTra = Selected.MaPhieuTra,
                MaNhanVien = MaNhanVien,
                NgayTra = NgayTra,
                TongTienPhat = TongTienPhat,
                GhiChu = GhiChu
            };

            if (CXuLyPhieuTra.sua(pt))
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else MessageBox.Show("Cập nhật thất bại!", "Thông báo");
        }

        void Xoa()
        {
            if (Selected == null) return;

            if (MessageBox.Show("Xác nhận xóa?", "Xác nhận",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXuLyPhieuTra.xoa(Selected.MaPhieuTra))
                {
                    LoadData();
                    LamMoi();
                }
            }
        }

        void LamMoi()
        {
            MaNhanVien = null;
            NgayTra = DateTime.Now;
            TongTienPhat = null;
            GhiChu = "";
            Selected = null;
        }
    }
}