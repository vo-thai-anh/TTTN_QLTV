using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System;
using System.Collections.Generic;
using System.Windows;

namespace QLTV_WPF.ViewModels
{
    class PhieuMuonVM : CBaseMVVM
    {
        public PhieuMuonVM()
        {
            LoadData();

            cmdthem = new RelayCommand(p => Them(), p => true);
            cmdsua = new RelayCommand(p => Sua(), p => Selected != null);
            cmdxoa = new RelayCommand(p => Xoa(), p => Selected != null);
            cmdlammoi = new RelayCommand(p => LamMoi());
            cmdsearch = new RelayCommand(p => Search());
        }

        public RelayCommand cmdthem { get; set; }
        public RelayCommand cmdsua { get; set; }
        public RelayCommand cmdxoa { get; set; }
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdsearch { get; set; }

        private List<PhieuMuon> _list;
        public List<PhieuMuon> List
        {
            get => _list;
            set { _list = value; NotifyPropertyChanged("List"); }
        }

        private int? _maDocGia, _maNhanVien;
        private DateTime? _ngayMuon = DateTime.Now;
        private DateTime? _ngayTra;
        private string _ghiChu, _keyword;

        public int? MaDocGia { get => _maDocGia; set { _maDocGia = value; NotifyPropertyChanged("MaDocGia"); } }
        public int? MaNhanVien { get => _maNhanVien; set { _maNhanVien = value; NotifyPropertyChanged("MaNhanVien"); } }
        public DateTime? NgayMuon { get => _ngayMuon; set { _ngayMuon = value; NotifyPropertyChanged("NgayMuon"); } }
        public DateTime? NgayTra { get => _ngayTra; set { _ngayTra = value; NotifyPropertyChanged("NgayTra"); } }
        public string GhiChu { get => _ghiChu; set { _ghiChu = value; NotifyPropertyChanged("GhiChu"); } }
        public string Keyword { get => _keyword; set { _keyword = value; NotifyPropertyChanged("Keyword"); } }

        private PhieuMuon _selected;
        public PhieuMuon Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (value != null)
                {
                    MaDocGia = value.MaDocGia;
                    MaNhanVien = value.MaNhanVien;
                    NgayMuon = value.NgayMuon;
                    NgayTra = value.NgayTra;
                    GhiChu = value.GhiChu;
                }
                NotifyPropertyChanged("Selected");
            }
        }

        void LoadData() => List = CXuLyPhieuMuon.getds();

        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) { LoadData(); return; }
            List = CXuLyPhieuMuon.search(Keyword);
        }

        bool KiemTra()
        {
            if (MaDocGia == null || MaNhanVien == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
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

            var pm = new PhieuMuon
            {
                MaPhieuMuon = 0,
                MaDocGia = MaDocGia,
                MaNhanVien = MaNhanVien,
                NgayMuon = NgayMuon,
                NgayTra = NgayTra,
                GhiChu = GhiChu
            };

            if (CXuLyPhieuMuon.them(pm))
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

            var pm = new PhieuMuon
            {
                MaPhieuMuon = Selected.MaPhieuMuon,
                MaDocGia = MaDocGia,
                MaNhanVien = MaNhanVien,
                NgayMuon = NgayMuon,
                NgayTra = NgayTra,
                GhiChu = GhiChu
            };

            if (CXuLyPhieuMuon.sua(pm))
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
                if (CXuLyPhieuMuon.xoa(Selected.MaPhieuMuon))
                {
                    LoadData();
                    LamMoi();
                }
            }
        }

        void LamMoi()
        {
            MaDocGia = MaNhanVien = null;
            NgayMuon = DateTime.Now;
            NgayTra = null;
            GhiChu = "";
            Keyword = "";
            Selected = null;
        }
    }
}