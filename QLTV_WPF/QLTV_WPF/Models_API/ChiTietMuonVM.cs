using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System;
using System.Collections.Generic;
using System.Windows;

namespace QLTV_WPF.ViewModels
{

    class ChiTietMuonVM : CBaseMVVM
    {
        private int _maPhieuHienTai;
        public ChiTietMuonVM(int maPhieu)
        {
           _maPhieuHienTai = maPhieu;
             MaPhieuMuon = maPhieu; // Gán sẵn mã phiếu để khi quét mã sách là Thêm vào đúng phiếu này
    
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

        private List<ChiTietMuon> _list;
        public List<ChiTietMuon> List
        {
            get => _list;
            set { _list = value; NotifyPropertyChanged("List"); }
        }

        // Input
        private int _maPhieuMuon, _maSachMuon;
        private string _lyDoPhat;
        private decimal? _tienPhat;

        public int MaPhieuMuon { get => _maPhieuMuon; set { _maPhieuMuon = value; NotifyPropertyChanged("MaPhieuMuon"); } }
        public int MaSachMuon { get => _maSachMuon; set { _maSachMuon = value; NotifyPropertyChanged("MaSachMuon"); } }
        public string LyDoPhat { get => _lyDoPhat; set { _lyDoPhat = value; NotifyPropertyChanged("LyDoPhat"); } }
        public decimal? TienPhat { get => _tienPhat; set { _tienPhat = value; NotifyPropertyChanged("TienPhat"); } }

        private ChiTietMuon _selected;
        public ChiTietMuon Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (value != null)
                {
                    MaPhieuMuon = value.MaPhieuMuon;
                    MaSachMuon = value.MaSachMuon;
                    LyDoPhat = value.LyDoPhat;
                    TienPhat = value.TienPhat;
                }
                NotifyPropertyChanged("Selected");
            }
        }

        void LoadData()
        {
            List = CXulyChiTietMuon.getds();
        }

        // THÊM (mượn sách)
        void Them()
        {
            if (Selected != null)
            {
                MessageBox.Show("Thêm thất bại!", "Thông báo");
                return;
            }

            var ct = new ChiTietMuon
            {
                MaPhieuMuon = MaPhieuMuon,
                MaSachMuon = MaSachMuon
            };

            if (CXulyChiTietMuon.them(ct))
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

        // SỬA = TRẢ SÁCH
        void Sua()
        {
            if (Selected == null) return;

            var ct = new ChiTietMuon
            {
                MaPhieuMuon = MaPhieuMuon,
                MaSachMuon = MaSachMuon,
                NgayTraThucTe = DateTime.Now,
                TienPhat = TienPhat,
                LyDoPhat = LyDoPhat,
                MaPhieuTra = 1 // tạm
            };

            if (CXulyChiTietMuon.sua(ct))
            {
                MessageBox.Show("Trả sách thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else
            {
                MessageBox.Show("Thất bại!", "Thông báo");
            }
        }

        void Xoa()
        {
            if (Selected == null) return;

            if (MessageBox.Show("Xóa sách khỏi phiếu?", "Xác nhận",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXulyChiTietMuon.xoa(Selected.MaPhieuMuon, Selected.MaSachMuon))
                {
                    LoadData();
                    LamMoi();
                }
            }
        }

        void LamMoi()
        {
            MaPhieuMuon = 0;
            MaSachMuon = 0;
            LyDoPhat = "";
            TienPhat = 0;
            Selected = null;
            LoadData();
        }
    }
}