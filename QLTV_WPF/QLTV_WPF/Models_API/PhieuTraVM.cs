using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace QLTV_WPF.ViewModels
{
    // Class phụ để tạo cột Checkbox và xử lý trả từng cuốn
    class ItemThuHoi : CBaseMVVM
    {
        private bool _isChon;
        public bool IsChon { get => _isChon; set { _isChon = value; NotifyPropertyChanged("IsChon"); } }

        public ChiTietMuon ChiTietGoc { get; set; }

        private decimal? _tienPhat = 0;
        public decimal? TienPhat { get => _tienPhat; set { _tienPhat = value; NotifyPropertyChanged("TienPhat"); } }

        private string _lyDoPhat = "";
        public string LyDoPhat { get => _lyDoPhat; set { _lyDoPhat = value; NotifyPropertyChanged("LyDoPhat"); } }
    }

    public class PhieuTraHienThi
    {
        public int MaPhieuTra { get; set; }
        public string? TenNhanVien { get; set; }
        public DateTime? NgayTra { get; set; }
        public decimal? TongTienPhat { get; set; }
        public string? GhiChu { get; set; }
    }

    class PhieuTraVM : CBaseMVVM
    {
        public PhieuTraVM(int maPhieuMuonGoiY = 0)
        {
            ListNhanVien = CXuLyNhanVien.GetDsNhanVien();
            LoadData(); // Tải lịch sử ngay khi mở máy
            LamMoi(); // Thiết lập lại các trường nhập liệu

            cmdlammoi = new RelayCommand(p => LamMoi());
            cmdsearch = new RelayCommand(p => Search());
            cmdTimSachNo = new RelayCommand(p => TimSachNo());
            cmdChotTraSach = new RelayCommand(p => ChotTraSach(), p => ListSachNo != null && ListSachNo.Any(x => x.IsChon));
            if (maPhieuMuonGoiY > 0)
            {
                MaPhieuMuonTimKiem = maPhieuMuonGoiY;
                TimSachNo();
            }
        }

        #region PROPERTIES LỊCH SỬ (TAB 2)
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdsearch { get; set; }

        private List<PhieuTra> _list;
        public List<PhieuTra> List { get => _list; set { _list = value; NotifyPropertyChanged("List"); } }

        private List<PhieuTraHienThi> _listHienThi;
        public List<PhieuTraHienThi> ListHienThi { get => _listHienThi; set { _listHienThi = value; NotifyPropertyChanged("ListHienThi"); } }

        private string _keyword;
        public string Keyword { get => _keyword; set { _keyword = value; NotifyPropertyChanged("Keyword"); Search(); } }
        #endregion

        #region PROPERTIES NGHIỆP VỤ TRẢ SÁCH (TAB 1)
        public RelayCommand cmdTimSachNo { get; set; }
        public RelayCommand cmdChotTraSach { get; set; }

        private int _maPhieuMuonTimKiem;
        public int MaPhieuMuonTimKiem { get => _maPhieuMuonTimKiem; set { _maPhieuMuonTimKiem = value; NotifyPropertyChanged("MaPhieuMuonTimKiem"); } }

        private List<ItemThuHoi> _listSachNo;
        public List<ItemThuHoi> ListSachNo { get => _listSachNo; set { _listSachNo = value; NotifyPropertyChanged("ListSachNo"); } }

        private List<NhanVien> _listNhanVien;
        public List<NhanVien> ListNhanVien { get => _listNhanVien; set { _listNhanVien = value; NotifyPropertyChanged("ListNhanVien"); } }

        private int? _maNhanVien;
        public int? MaNhanVien { get => _maNhanVien; set { _maNhanVien = value; NotifyPropertyChanged("MaNhanVien"); } }

        private string _ghiChu;
        public string GhiChu { get => _ghiChu; set { _ghiChu = value; NotifyPropertyChanged("GhiChu"); } }
        #endregion

        void LoadData()
        {
            List = CXuLyPhieuTra.getds();
            BuildListHienThi();
        }

        void BuildListHienThi()
        {
            if (List == null) return;
            ListHienThi = List.Select(pt => new PhieuTraHienThi
            {
                MaPhieuTra = pt.MaPhieuTra,
                TenNhanVien = ListNhanVien?.FirstOrDefault(nv => nv.MaNv == pt.MaNhanVien)?.HoTen ?? pt.MaNhanVien?.ToString(),
                NgayTra = pt.NgayTra,
                TongTienPhat = pt.TongTienPhat,
                GhiChu = pt.GhiChu
            }).ToList();
        }

        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) { LoadData(); return; }
            var lower = Keyword.Trim().ToLower();
            ListHienThi = ListHienThi.Where(x => x.MaPhieuTra.ToString().Contains(lower) || (x.GhiChu != null && x.GhiChu.ToLower().Contains(lower))).ToList();
        }

        void TimSachNo()
        {
            if (MaPhieuMuonTimKiem <= 0) return;

            // 1. TÌM PHIẾU MƯỢN GỐC ĐỂ BIẾT HẠN TRẢ
            var phieuMuonGoc = CXuLyPhieuMuon.getds().FirstOrDefault(p => p.MaPhieuMuon == MaPhieuMuonTimKiem);
            if (phieuMuonGoc == null)
            {
                MessageBox.Show("Mã phiếu mượn không tồn tại!", "Lỗi");
                return;
            }

            var tatCaChiTiet = CXulyChiTietMuon.getByPhieuMuon(MaPhieuMuonTimKiem);
            var danhSachChuaTra = tatCaChiTiet.Where(x => x.NgayTraThucTe == null).ToList();

            if (danhSachChuaTra.Count == 0)
            {
                MessageBox.Show("Phiếu mượn này không có sách nào đang nợ!", "Thông báo");
                ListSachNo = null;
                return;
            }

            // 2. DUYỆT TỪNG CUỐN VÀ TỰ TÍNH TIỀN PHẠT
            var listTam = new List<ItemThuHoi>();
            foreach (var ct in danhSachChuaTra)
            {
                decimal tienPhatGoiY = 0;
                string lyDoGoiY = "";

                // Kiểm tra xem có hạn trả chưa, và ngày hôm nay đã vượt quá hạn trả chưa (chỉ tính theo ngày, bỏ qua giờ giấc)
                if (phieuMuonGoc.NgayTra.HasValue && DateTime.Now.Date > phieuMuonGoc.NgayTra.Value.Date)
                {
                    // Tính số ngày trễ
                    int soNgayTre = (DateTime.Now.Date - phieuMuonGoc.NgayTra.Value.Date).Days;

                    // Giả sử quy định phạt 5.000đ / ngày trễ (Bạn có thể đổi số 5000 này theo ý muốn)
                    tienPhatGoiY = soNgayTre * 5000;
                    lyDoGoiY = $"Trễ hạn {soNgayTre} ngày";
                }

                listTam.Add(new ItemThuHoi
                {
                    IsChon = false, // Để thủ thư tự tick
                    ChiTietGoc = ct,
                    TienPhat = tienPhatGoiY,
                    LyDoPhat = lyDoGoiY
                });
            }

            // Đẩy ra giao diện
            ListSachNo = listTam;
        }

        void ChotTraSach()
        {
            if (MaNhanVien == null) { MessageBox.Show("Vui lòng chọn Nhân viên thu hồi!"); return; }
            var dsSelected = ListSachNo.Where(x => x.IsChon).ToList();
            decimal tongPhat = dsSelected.Sum(x => x.TienPhat ?? 0);

            // 1. Tạo Phiếu Trả
            var phieuMoi = CXuLyPhieuTra.themVoiKetQua(new PhieuTra { MaNhanVien = MaNhanVien, NgayTra = DateTime.Now, TongTienPhat = tongPhat, GhiChu = GhiChu });
            if (phieuMoi != null)
            {
                // 2. Cập nhật từng cuốn sách
                foreach (var item in dsSelected)
                {
                    item.ChiTietGoc.NgayTraThucTe = DateTime.Now;
                    item.ChiTietGoc.TienPhat = item.TienPhat;
                    item.ChiTietGoc.LyDoPhat = item.LyDoPhat;
                    item.ChiTietGoc.MaPhieuTra = phieuMoi.MaPhieuTra;
                    CXulyChiTietMuon.sua(item.ChiTietGoc);
                }
                MessageBox.Show("Thu hồi sách và lập phiếu trả thành công!");
                LamMoi(); LoadData();
            }
        }

        void LamMoi() {
            MaNhanVien = CSessionManager.MaNV;
            GhiChu = ""; MaPhieuMuonTimKiem = 0; 
            ListSachNo = null; Keyword = ""; 
            LoadData(); }
    }
}