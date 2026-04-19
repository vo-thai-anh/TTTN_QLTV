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
    // DTO để hiển thị tên nhân viên trong DataGrid thay vì mã số
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
        public PhieuTraVM()
        {
            // 1. Load danh sách nhân viên cho ComboBox
            ListNhanVien = CXuLyNhanVien.GetDsNhanVien();

            // 2. Load dữ liệu phiếu trả
            LoadData();

            // 3. Khởi tạo commands
            cmdthem    = new RelayCommand(p => Them(), p => true);
            cmdsua     = new RelayCommand(p => Sua(),  p => SelectedPhieuTra != null);
            cmdxoa     = new RelayCommand(p => Xoa(),  p => SelectedPhieuTra != null);
            cmdlammoi  = new RelayCommand(p => LamMoi());
            cmdsearch  = new RelayCommand(p => Search());
        }

        // ── Commands ──────────────────────────────────────────────────────────
        public RelayCommand cmdthem   { get; set; }
        public RelayCommand cmdsua    { get; set; }
        public RelayCommand cmdxoa    { get; set; }
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdsearch { get; set; }

        // ── Danh sách gốc (để Sửa/Xóa) ───────────────────────────────────────
        private List<PhieuTra> _list;
        public List<PhieuTra> List
        {
            get => _list;
            set { _list = value; NotifyPropertyChanged("List"); }
        }

        // ── Danh sách hiển thị (DataGrid) ────────────────────────────────────
        private List<PhieuTraHienThi> _listHienThi;
        public List<PhieuTraHienThi> ListHienThi
        {
            get => _listHienThi;
            set { _listHienThi = value; NotifyPropertyChanged("ListHienThi"); }
        }

        // ── Danh sách nhân viên cho ComboBox ─────────────────────────────────
        private List<NhanVien> _listNhanVien;
        public List<NhanVien> ListNhanVien
        {
            get => _listNhanVien;
            set { _listNhanVien = value; NotifyPropertyChanged("ListNhanVien"); }
        }

        // ── Item được chọn trong DataGrid (DTO hiển thị) ─────────────────────
        private PhieuTraHienThi _selectedHienThi;
        public PhieuTraHienThi SelectedHienThi
        {
            get => _selectedHienThi;
            set
            {
                _selectedHienThi = value;
                NotifyPropertyChanged("SelectedHienThi");

                // Đồng bộ về PhieuTra gốc để dùng cho Sửa/Xóa
                if (value != null)
                    SelectedPhieuTra = List?.FirstOrDefault(pt => pt.MaPhieuTra == value.MaPhieuTra);
                else
                    SelectedPhieuTra = null;
            }
        }

        // ── PhieuTra gốc được chọn (dùng cho Sửa/Xóa & điền form) ───────────
        private PhieuTra _selectedPhieuTra;
        public PhieuTra SelectedPhieuTra
        {
            get => _selectedPhieuTra;
            set
            {
                _selectedPhieuTra = value;
                NotifyPropertyChanged("SelectedPhieuTra");

                // Tự điền form khi chọn hàng
                if (value != null)
                {
                    MaNhanVien    = value.MaNhanVien;
                    NgayTra       = value.NgayTra;
                    TongTienPhat  = value.TongTienPhat;
                    GhiChu        = value.GhiChu;
                }
            }
        }

        // ── Các trường nhập liệu ──────────────────────────────────────────────
        private int?      _maNhanVien;
        private DateTime? _ngayTra    = DateTime.Now;
        private decimal?  _tongTienPhat;
        private string    _ghiChu;
        private string    _keyword;

        public int?      MaNhanVien   { get => _maNhanVien;   set { _maNhanVien   = value; NotifyPropertyChanged("MaNhanVien");   } }
        public DateTime? NgayTra      { get => _ngayTra;      set { _ngayTra      = value; NotifyPropertyChanged("NgayTra");      } }
        public decimal?  TongTienPhat { get => _tongTienPhat; set { _tongTienPhat = value; NotifyPropertyChanged("TongTienPhat"); } }
        public string    GhiChu       { get => _ghiChu;       set { _ghiChu       = value; NotifyPropertyChanged("GhiChu");       } }
        public string    Keyword      { get => _keyword;      set { _keyword      = value; NotifyPropertyChanged("Keyword"); Search(); } }

        // ── Load & Build ──────────────────────────────────────────────────────
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
                MaPhieuTra   = pt.MaPhieuTra,
                TenNhanVien  = ListNhanVien?.FirstOrDefault(nv => nv.MaNv == pt.MaNhanVien)?.HoTen
                               ?? pt.MaNhanVien?.ToString(),
                NgayTra      = pt.NgayTra,
                TongTienPhat = pt.TongTienPhat,
                GhiChu       = pt.GhiChu
            }).ToList();
        }

        void Search()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) { LoadData(); return; }

            // Tìm theo: Mã phiếu trả, Tên nhân viên, hoặc Ghi chú
            var lower = Keyword.Trim().ToLower();
            ListHienThi = (List ?? new List<PhieuTra>())
                .Where(pt =>
                    pt.MaPhieuTra.ToString().Contains(lower) ||
                    (pt.GhiChu != null && pt.GhiChu.ToLower().Contains(lower)) ||
                    (ListNhanVien?.FirstOrDefault(nv => nv.MaNv == pt.MaNhanVien)?.HoTen?.ToLower().Contains(lower) == true)
                )
                .Select(pt => new PhieuTraHienThi
                {
                    MaPhieuTra   = pt.MaPhieuTra,
                    TenNhanVien  = ListNhanVien?.FirstOrDefault(nv => nv.MaNv == pt.MaNhanVien)?.HoTen ?? pt.MaNhanVien?.ToString(),
                    NgayTra      = pt.NgayTra,
                    TongTienPhat = pt.TongTienPhat,
                    GhiChu       = pt.GhiChu
                }).ToList();
        }

        bool KiemTra()
        {
            if (MaNhanVien == null)
            {
                MessageBox.Show("Vui lòng chọn Nhân viên!", "Thông báo");
                return false;
            }
            if (NgayTra == null)
            {
                MessageBox.Show("Vui lòng nhập Ngày trả!", "Thông báo");
                return false;
            }
            return true;
        }

        void Them()
        {
            if (!KiemTra()) return;

            // Khi thêm mới, không được có record đang chọn
            if (SelectedPhieuTra != null)
            {
                MessageBox.Show("Đang ở chế độ sửa. Nhấn 'Làm mới' trước khi thêm!", "Thông báo");
                return;
            }

            var pt = new PhieuTra
            {
                MaPhieuTra   = 0,
                MaNhanVien   = MaNhanVien,
                NgayTra      = NgayTra,
                TongTienPhat = TongTienPhat,
                GhiChu       = GhiChu
            };

            if (CXuLyPhieuTra.them(pt))
            {
                MessageBox.Show("Thêm phiếu trả thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Kiểm tra lại kết nối hoặc dữ liệu.", "Thông báo");
            }
        }

        void Sua()
        {
            if (SelectedPhieuTra == null) return;
            if (!KiemTra()) return;

            var pt = new PhieuTra
            {
                MaPhieuTra   = SelectedPhieuTra.MaPhieuTra,
                MaNhanVien   = MaNhanVien,
                NgayTra      = NgayTra,
                TongTienPhat = TongTienPhat,
                GhiChu       = GhiChu
            };

            if (CXuLyPhieuTra.sua(pt))
            {
                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                LoadData();
                LamMoi();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Thông báo");
            }
        }

        void Xoa()
        {
            if (SelectedPhieuTra == null) return;

            var result = MessageBox.Show(
                $"Xác nhận xóa phiếu trả #{SelectedPhieuTra.MaPhieuTra}?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (CXuLyPhieuTra.xoa(SelectedPhieuTra.MaPhieuTra))
                {
                    MessageBox.Show("Xóa thành công!", "Thông báo");
                    LoadData();
                    LamMoi();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! Phiếu trả này có thể đang được sử dụng.", "Thông báo");
                }
            }
        }

        void LamMoi()
        {
            MaNhanVien        = null;
            NgayTra           = DateTime.Now;
            TongTienPhat      = null;
            GhiChu            = string.Empty;
            Keyword           = string.Empty;
            // Xóa selection KHÔNG trigger setter SelectedPhieuTra để tránh vòng lặp
            _selectedPhieuTra = null;
            NotifyPropertyChanged("SelectedPhieuTra");
            _selectedHienThi  = null;
            NotifyPropertyChanged("SelectedHienThi");
        }
    }
}