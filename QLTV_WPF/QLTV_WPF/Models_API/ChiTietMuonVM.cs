using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace QLTV_WPF.ViewModels
{
    class ChiTietMuonVM : CBaseMVVM
    {
        private int _maPhieuHienTai;
        private DateTime? _hanTraCuaPhieu; // Lưu hạn trả để tính tiền phạt

        public ChiTietMuonVM(int maPhieu)
        {
            _maPhieuHienTai = maPhieu;
            MaPhieuMuon = maPhieu;

            LoadData();
            LayThongTinHanTra(); // Lấy hạn trả từ phiếu mượn gốc

            // Chỉ cho phép bấm "Thêm" khi đang mở một phiếu cụ thể
            cmdThemCT = new RelayCommand(p => Them(), p => true);
            cmdSuaCT = new RelayCommand(p => Sua(), p => Selected != null);
            cmdXoaCT = new RelayCommand(p => Xoa(), p => Selected != null);
            cmdLamMoiCT = new RelayCommand(p => LamMoi());
        }

        #region Properties (Khai báo biến)
        public RelayCommand cmdThemCT { get; set; }
        public RelayCommand cmdSuaCT { get; set; }
        public RelayCommand cmdXoaCT { get; set; }
        public RelayCommand cmdLamMoiCT { get; set; }

        private List<ChiTietMuon> _list;
        public List<ChiTietMuon> List { get => _list; set { _list = value; NotifyPropertyChanged("List"); } }

        private int _maPhieuMuon;
        private int? _maSachMuon;
        public int? MaSachMuon
        {
            get => _maSachMuon;
            set { _maSachMuon = value; NotifyPropertyChanged("MaSachMuon"); }
        }
        private string _lyDoPhat;
        private decimal? _tienPhat;

        public int MaPhieuMuon { get => _maPhieuMuon; set { _maPhieuMuon = value; NotifyPropertyChanged("MaPhieuMuon"); } }
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
                    MaSachMuon = value.MaSachMuon;
                    LyDoPhat = value.LyDoPhat;
                    TienPhat = value.TienPhat;

                    // Nếu là sách chưa trả, tự tính tiền phạt gợi ý
                    if (value.NgayTraThucTe == null) TinhTienPhatGoiY();
                }
                NotifyPropertyChanged("Selected");
            }
        }
        #endregion

        #region Logic Xử Lý (Methods)

        // 1. Lọc dữ liệu: Chỉ hiện sách của phiếu đang chọn
        void LoadData()
        {
            if (_maPhieuHienTai > 0)
                List = CXulyChiTietMuon.getByPhieuMuon(_maPhieuHienTai);
            else
                List = CXulyChiTietMuon.getds();
        }

        // 2. Lấy hạn trả của phiếu mượn này để tính phạt
        void LayThongTinHanTra()
        {
            if (_maPhieuHienTai <= 0) return;
            var phieu = CXuLyPhieuMuon.getds().FirstOrDefault(x => x.MaPhieuMuon == _maPhieuHienTai);
            _hanTraCuaPhieu = phieu?.NgayTra;
        }

        // 3. Tự động tính tiền phạt (ví dụ 5.000đ / ngày quá hạn)
        void TinhTienPhatGoiY()
        {
            if (_hanTraCuaPhieu == null || DateTime.Now <= _hanTraCuaPhieu)
            {
                TienPhat = 0; LyDoPhat = "Trả đúng hạn";
                return;
            }
            int soNgayTre = (DateTime.Now - _hanTraCuaPhieu.Value).Days;
            TienPhat = soNgayTre * 5000;
            LyDoPhat = $"Trễ {soNgayTre} ngày";
        }

        // 4. MƯỢN SÁCH (Thêm vào phiếu)
        void Them()
        {
            if (MaSachMuon == null || MaSachMuon <= 0)
            {
                MessageBox.Show("Vui lòng nhập mã cuốn sách vật lý!");
                return;
            }

            if (Selected != null) { MessageBox.Show("Vui lòng bấm Làm mới trước khi thêm sách mới!"); return; }

            var ct = new ChiTietMuon { MaPhieuMuon = _maPhieuHienTai, MaSachMuon = MaSachMuon.Value };

            // Bỏ nhánh else đi, nếu true thì load lại data, false thì CXulyChiTietMuon tự báo lỗi
            if (CXulyChiTietMuon.them(ct))
            {
                LoadData();
                LamMoi();
            }
        }

        // 5. TRẢ SÁCH (Sửa chi tiết)
        void Sua()
        {
            if (Selected == null) return;
            var ct = new ChiTietMuon
            {
                MaPhieuMuon = Selected.MaPhieuMuon,
                MaSachMuon = Selected.MaSachMuon,
                NgayTraThucTe = DateTime.Now,
                TienPhat = TienPhat,
                LyDoPhat = LyDoPhat,
                MaPhieuTra = 1 // Tạm thời gắn vào phiếu trả số 1
            };

            if (CXulyChiTietMuon.sua(ct))
            {
                MessageBox.Show("Đã thu hồi sách về kho thành công!");
                LoadData();
                LamMoi();
            }
        }

        void Xoa()
        {
            if (Selected == null) return;
            if (MessageBox.Show("Xóa sách khỏi phiếu?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (CXulyChiTietMuon.xoa(Selected.MaPhieuMuon, Selected.MaSachMuon))
                {
                    LoadData(); LamMoi();
                }
            }
        }

        void LamMoi()
        {
            MaSachMuon = 0; LyDoPhat = ""; TienPhat = 0; Selected = null; LoadData();
        }
        #endregion
    }
}