using QLTV_WPF.Models;
using QLTV_WPF.Models_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace QLTV_WPF.ViewModels
{
    // DTO để hiển thị tên đầy đủ trong DataGrid
    public class PhieuMuonHienThi
    {
        public int MaPhieuMuon { get; set; }
        public string TenNhanVien { get; set; }
        public string TenDocGia { get; set; }
        public DateTime? NgayMuon { get; set; }
        public DateTime? NgayTra { get; set; }
        public string GhiChu { get; set; }
    }
    class PhieuMuonVM : CBaseMVVM
    {
        public PhieuMuonVM()
        {
            ListDocGia = CXuLyDocGia.getdsdg();
            ListNhanVien = CXuLyNhanVien.GetDsNhanVien();
            LoadData();
            LamMoi();
            var viewDocGia = CollectionViewSource.GetDefaultView(ListDocGia);
            viewDocGia.Filter = (item) =>
            {
                if (string.IsNullOrWhiteSpace(TuKhoaDocGia)) return true;

                var dg = item as DocGia;
                string keyword = TuKhoaDocGia.ToLower();

                // 🌟 NÂNG CẤP DÒNG NÀY: Kiểm tra cả Họ tên HOẶC Số điện thoại
                return (dg.HoTen != null && dg.HoTen.ToLower().Contains(keyword)) ||
                       (dg.Sdt != null && dg.Sdt.Contains(keyword));
            };

            cmdthem = new RelayCommand(p => Them(), p => true);
            cmdsua = new RelayCommand(p => Sua(), p => Selected != null);
            cmdxoa = new RelayCommand(p => Xoa(), p => Selected != null);
            cmdlammoi = new RelayCommand(p => LamMoi());
            cmdsearch = new RelayCommand(p => Search());
            cmdMoChiTiet = new RelayCommand(p => MoChiTiet_Execute(), p => SelectedHienThi != null);
            cmdTraSachNhanh = new RelayCommand(p => TraSachNhanh_Execute(), p => SelectedHienThi != null);
        }

        public RelayCommand cmdthem { get; set; }
        public RelayCommand cmdsua { get; set; }
        public RelayCommand cmdxoa { get; set; }
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdsearch { get; set; }
        public RelayCommand cmdMoChiTiet { get; set; }
        public RelayCommand cmdTraSachNhanh { get; set; }

        private List<PhieuMuon> _list;
        public List<PhieuMuon> List
        {
            get => _list;
            set { _list = value; NotifyPropertyChanged("List"); }
        }

        private List<PhieuMuonHienThi> _listHienThi;
        public List<PhieuMuonHienThi> ListHienThi
        {
            get => _listHienThi;
            set { _listHienThi = value; NotifyPropertyChanged("ListHienThi"); }
        }

        private PhieuMuonHienThi _selectedHienThi;
        public PhieuMuonHienThi SelectedHienThi
        {
            get => _selectedHienThi;
            set
            {
                _selectedHienThi = value;
                NotifyPropertyChanged("SelectedHienThi");
                if (value != null)
                {
                    // Tìm PhieuMuon gốc để dùng cho Sửa/Xóa
                    Selected = List?.FirstOrDefault(pm => pm.MaPhieuMuon == value.MaPhieuMuon);
                }
                else
                {
                    Selected = null;
                }
            }
        }

        private List<DocGia> _listDocGia;
        public List<DocGia> ListDocGia
        {
            get => _listDocGia;
            set { _listDocGia = value; NotifyPropertyChanged("ListDocGia"); }
        }

        private List<NhanVien> _listNhanVien;
        public List<NhanVien> ListNhanVien
        {
            get => _listNhanVien;
            set { _listNhanVien = value; NotifyPropertyChanged("ListNhanVien"); }
        }

        private string _tuKhoaDocGia;
        public string TuKhoaDocGia
        {
            get => _tuKhoaDocGia;
            set
            {
                _tuKhoaDocGia = value;
                NotifyPropertyChanged("TuKhoaDocGia");

                // Kích hoạt bộ lọc danh sách ngay khi gõ chữ
                if (ListDocGia != null)
                {
                    CollectionViewSource.GetDefaultView(ListDocGia).Refresh();
                }
            }
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
        public string Keyword { get => _keyword; set { _keyword = value; NotifyPropertyChanged("Keyword"); Search(); } }

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

        void LoadData()
        {
            List = CXuLyPhieuMuon.getds();
            BuildListHienThi();
        }

        void BuildListHienThi()
        {
            if (List == null) return;
            ListHienThi = List.Select(pm => new PhieuMuonHienThi
            {
                MaPhieuMuon = pm.MaPhieuMuon,
                TenNhanVien = ListNhanVien?.FirstOrDefault(nv => nv.MaNv == pm.MaNhanVien)?.HoTen ?? pm.MaNhanVien?.ToString(),
                TenDocGia = ListDocGia?.FirstOrDefault(dg => dg.MaDocGia == pm.MaDocGia)?.HoTen ?? pm.MaDocGia?.ToString(),
                NgayMuon = pm.NgayMuon,
                NgayTra = pm.NgayTra,
                GhiChu = pm.GhiChu
            }).ToList();
        }

        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) { LoadData(); return; }

            // Tìm client-side theo: Mã phiếu, Tên NV, Tên độc giả, Ghi chú
            var lower = Keyword.Trim().ToLower();
            ListHienThi = (List ?? new List<PhieuMuon>())
                .Where(pm =>
                    pm.MaPhieuMuon.ToString().Contains(lower) ||
                    (pm.GhiChu != null && pm.GhiChu.ToLower().Contains(lower)) ||
                    (ListNhanVien?.FirstOrDefault(nv => nv.MaNv == pm.MaNhanVien)?.HoTen?.ToLower().Contains(lower) == true) ||
                    (ListDocGia?.FirstOrDefault(dg => dg.MaDocGia == pm.MaDocGia)?.HoTen?.ToLower().Contains(lower) == true)
                )
                .Select(pm => new PhieuMuonHienThi
                {
                    MaPhieuMuon = pm.MaPhieuMuon,
                    TenNhanVien = ListNhanVien?.FirstOrDefault(nv => nv.MaNv == pm.MaNhanVien)?.HoTen ?? pm.MaNhanVien?.ToString(),
                    TenDocGia = ListDocGia?.FirstOrDefault(dg => dg.MaDocGia == pm.MaDocGia)?.HoTen ?? pm.MaDocGia?.ToString(),
                    NgayMuon = pm.NgayMuon,
                    NgayTra = pm.NgayTra,
                    GhiChu = pm.GhiChu
                }).ToList();
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

        // Trong PhieuMuonVM.cs
        void Them()
        {
            if (!KiemTra()) return;

            var pm = new PhieuMuon
            {
                MaPhieuMuon = 0,
                MaDocGia = MaDocGia,
                MaNhanVien = MaNhanVien,
                NgayMuon = NgayMuon,
                NgayTra = NgayTra,
                GhiChu = GhiChu
            };

            // Gọi hàm API mới để lấy về object có chứa MaPhieuMuon vừa sinh ra
            var phieuVuaTao = CXuLyPhieuMuon.themVoiKetQua(pm);

            if (phieuVuaTao != null)
            {
                MessageBox.Show($"Lập phiếu mượn #{phieuVuaTao.MaPhieuMuon} thành công! Mời bạn quét mã sách.");
                LoadData();
                LamMoi();

                // TỰ ĐỘNG MỞ CỬA SỔ CHI TIẾT
                var windowDetail = new UI.QL_ChiTietMuon(phieuVuaTao.MaPhieuMuon);
                windowDetail.ShowDialog();
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
            MaNhanVien = CSessionManager.MaNV;
            NgayMuon = DateTime.Now;
            NgayTra = null;
            GhiChu = "";
            Keyword = "";
            Selected = null;
            SelectedHienThi = null;
        }
        private void MoChiTiet_Execute()
        {
            if (SelectedHienThi == null) return;


            var window = new UI.QL_ChiTietMuon(SelectedHienThi.MaPhieuMuon);
            window.ShowDialog();
            LoadData();
        }
        private void TraSachNhanh_Execute()
        {
            if (SelectedHienThi == null) return;

            // Chỉ cần gọi thẳng cửa sổ All-in-One và ném mã Phiếu mượn sang
            var windowTra = new UI.QL_PhieuTra(SelectedHienThi.MaPhieuMuon);
            windowTra.ShowDialog();

            LoadData();
        }
    }
}