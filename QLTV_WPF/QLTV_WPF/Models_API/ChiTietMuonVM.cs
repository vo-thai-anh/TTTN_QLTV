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
            MaPhieuMuon = maPhieu;

            LoadData();

            // TÊN COMMAND ĐÃ ĐƯỢC CHUẨN HÓA THEO ĐÚNG NÚT BẤM TRÊN UI
            cmdQuetThem = new RelayCommand(p => QuetThemSach(), p => true);
            cmdNhanTraSach = new RelayCommand(p => MoGiaoDienTraSach(), p => _maPhieuHienTai > 0); // Luôn sáng
            cmdXoaNham = new RelayCommand(p => XoaSachLoi(), p => Selected != null);
            cmdLamMoi = new RelayCommand(p => LamMoiData());
        }

        #region Properties (Biến giao diện)
        public RelayCommand cmdQuetThem { get; set; }
        public RelayCommand cmdNhanTraSach { get; set; }
        public RelayCommand cmdXoaNham { get; set; }
        public RelayCommand cmdLamMoi { get; set; }

        private List<ChiTietMuon> _list;
        public List<ChiTietMuon> List { get => _list; set { _list = value; NotifyPropertyChanged("List"); } }

        private int _maPhieuMuon;
        public int MaPhieuMuon { get => _maPhieuMuon; set { _maPhieuMuon = value; NotifyPropertyChanged("MaPhieuMuon"); } }

        private int? _maSachMuon;
        public int? MaSachMuon { get => _maSachMuon; set { _maSachMuon = value; NotifyPropertyChanged("MaSachMuon"); } }

        private string _lyDoPhat;
        public string LyDoPhat { get => _lyDoPhat; set { _lyDoPhat = value; NotifyPropertyChanged("LyDoPhat"); } }

        private decimal? _tienPhat;
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
                    MaSachMuon = value.MaSachMuon;
                    LyDoPhat = value.LyDoPhat;
                    TienPhat = value.TienPhat;
                    // Đã xóa phần tự tính tiền phạt vì chức năng đó đã chuyển sang form Phiếu Trả
                }
                NotifyPropertyChanged("Selected");
            }
        }
        #endregion

        #region Logic Xử Lý (Methods)

        void LoadData()
        {
            if (_maPhieuHienTai > 0)
                List = CXulyChiTietMuon.getByPhieuMuon(_maPhieuHienTai);
            else
                List = CXulyChiTietMuon.getds();
        }

        // Đã đổi tên từ Them() thành QuetThemSach()
        void QuetThemSach()
        {
            if (MaSachMuon == null || MaSachMuon <= 0)
            {
                MessageBox.Show("Vui lòng nhập mã cuốn sách vật lý!");
                return;
            }

            if (Selected != null) { MessageBox.Show("Vui lòng bấm Làm mới trước khi thêm sách mới!"); return; }

            var ct = new ChiTietMuon { MaPhieuMuon = _maPhieuHienTai, MaSachMuon = MaSachMuon.Value };

            if (CXulyChiTietMuon.them(ct))
            {
                LoadData();
                LamMoiData();
            }
        }

        // Đã đổi tên từ Sua() thành MoGiaoDienTraSach() và cập nhật logic chuyển trang
        void MoGiaoDienTraSach()
        {
            if (_maPhieuHienTai <= 0) return;

            // Chuyển thẳng sang trang All-in-One và ném mã phiếu mượn qua
            var windowTra = new UI.QL_PhieuTra(_maPhieuHienTai);
            windowTra.ShowDialog();

            LoadData(); // Cập nhật lại giỏ hàng sau khi trả sách xong
        }

        // Đã đổi tên từ Xoa() thành XoaSachLoi()
        void XoaSachLoi()
        {
            if (Selected == null) return;
            if (MessageBox.Show("Xóa sách khỏi phiếu?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXulyChiTietMuon.xoa(Selected.MaPhieuMuon, Selected.MaSachMuon))
                {
                    LoadData();
                    LamMoiData();
                }
            }
        }

        void LamMoiData()
        {
            MaSachMuon = 0; LyDoPhat = ""; TienPhat = 0; Selected = null; LoadData();
        }
        #endregion
    }
}