using QLTV_WPF.Models;
using QLTV_WPF.ViewModels; // Thư mục chứa CBaseMVVM và RelayCommand của bạn
using System;
using System.Collections.Generic;

namespace QLTV_WPF.Models_API
{
    class SachVM : CBaseMVVM
    {
        public SachVM()
        {
            //Lấy danh sách Sách từ API
            ListSach = CXuLySach.getdssach();
            //Lấy danh sách Loại sách từ API
            ListLoaiSach = CXuLyLoaiSach.getdsls();

            ListNXB = CXuLyNhaXuatBan.getdsnxb();

            cmdthemsach = new RelayCommand(ThemSach_Execute, ThemSach_CanExecute);
            cmdsuasach = new RelayCommand(SuaSach_Execute, SuaSach_CanExecute);
            cmdxoasach = new RelayCommand(XoaSach_Execute, XoaSach_CanExecute);
            cmdlammoi = new RelayCommand(p => LamMoi());
            cmdMoKhoSach = new RelayCommand(MoKhoSach_Execute, MoKhoSach_CanExecute);
            cmdsearch = new RelayCommand(p => Search());

            cmdQuanLyTacGia = new RelayCommand(p => {
                if (SelectedSach != null)
                {
                    // Chế độ 1: Đang chọn sách cũ để sửa
                    var popup = new UI.QL_Sach_TacGia(SelectedSach.MaSach, SelectedSach.TenSach);
                    popup.ShowDialog();
                    ListSach = CXuLySach.getdssach(); // Cập nhật lại UI bảng chính
                }
                else
                {
                    // Chế độ 2: Đang nhập sách mới
                    var popup = new UI.QL_Sach_TacGia(null, Tensach, m_dsTacGiaTam);
                    if (popup.ShowDialog() == true)
                    {
                        m_dsTacGiaTam = popup.SelectedIds; // Cất danh sách vừa chọn vào biến tạm
                    }
                }
            }, p => true); // ĐỔI THÀNH TRUE ĐỂ NÚT Tác giả LUÔN SÁNG

        }

        // Khai báo Commands
        public RelayCommand cmdthemsach { get; set; }
        public RelayCommand cmdsuasach { get; set; }
        public RelayCommand cmdxoasach { get; set; }
        public RelayCommand cmdlammoi { get; set; }
        public RelayCommand cmdMoKhoSach { get; set; }
        public RelayCommand cmdQuanLyTacGia { get; set; }
        public RelayCommand cmdsearch { get; set; }

        // Danh sách sách hiển thị lên DataGrid
        private List<Sach> m_listSach;
        public List<Sach> ListSach
        {
            get { return m_listSach; }
            set
            {
                m_listSach = value;
                NotifyPropertyChanged("ListSach");
            }
        }
        // Biến tạm để lưu danh sách mã tác giả khi thêm mới sách (chưa có mã sách để lấy từ DB)
        private List<int> m_dsTacGiaTam = new List<int>();


        private List<LoaiSach> m_listLoaiSach;
        // Danh sách loại sách hiển thị lên comboBox Loại sách
        public List<LoaiSach> ListLoaiSach
        {
            get { return m_listLoaiSach; }
            set { m_listLoaiSach = value; NotifyPropertyChanged("ListLoaiSach"); }
        }
        private List<NhaXuatBan> m_listNXB;
        public List<NhaXuatBan> ListNXB
        {
            get { return m_listNXB; }
            set { m_listNXB = value; NotifyPropertyChanged("ListNXB"); }
        }
        // Các thuộc tính Binding lên TextBox
        private string m_tensach;
        public string Tensach
        {
            get { return m_tensach; }
            set { m_tensach = value; NotifyPropertyChanged("Tensach"); }
        }

        private int? m_namxb;
        public int? Namxb
        {
            get { return m_namxb; }
            set { m_namxb = value; NotifyPropertyChanged("Namxb"); }
        }

        private int? m_sotrang;
        public int? Sotrang
        {
            get { return m_sotrang; }
            set { m_sotrang = value; NotifyPropertyChanged("Sotrang"); }
        }

        private string m_tomtat;
        public string Tomtat
        {
            get { return m_tomtat; }
            set { m_tomtat = value; NotifyPropertyChanged("Tomtat"); }
        }

        private int? m_soluong;
        public int? Soluong
        {
            get { return m_soluong; }
            set { m_soluong = value; NotifyPropertyChanged("Soluong"); }
        }

        private int? m_maloai;
        public int? Maloai
        {
            get { return m_maloai; }
            set { m_maloai = value; NotifyPropertyChanged("Maloai"); }
        }

        private int? m_manxb;
        public int? Manxb
        {
            get { return m_manxb; }
            set { m_manxb = value; NotifyPropertyChanged("Manxb"); }
        }
        private string _keyword;
        public string Keyword
        {
            get => _keyword;
            set
            {
                _keyword = value;
                NotifyPropertyChanged("Keyword");
                Search(); // Tự động tìm kiếm mỗi khi gõ phím
            }
        }

        // Chọn một dòng trên DataGrid
        private Sach m_selectedSach;
        public Sach SelectedSach
        {
            get { return m_selectedSach; }
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

        // Thêm Sách
        public void ThemSach_Execute(object parameter)
        {
            // 1. Bắt buộc phải có Tên sách (Hàm IsNullOrWhiteSpace kiểm tra cả trường hợp người dùng gõ toàn dấu cách)
            if (string.IsNullOrWhiteSpace(Tensach))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập Tên sách!", "Cảnh báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return; // Lệnh return này sẽ bắt hàm dừng lại ngay lập tức, không chạy phần code thêm phía dưới nữa
            }

            // 2. Tùy chọn: Bắt buộc chọn Loại sách
            if (Maloai == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn Loại sách!", "Cảnh báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            // 3. Tùy chọn: Bắt buộc chọn Nhà xuất bản
            if (Manxb == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn Nhà xuất bản!", "Cảnh báo", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }
            CSach moi = new CSach
            {
                TenSach = this.Tensach,
                NamXb = this.Namxb,
                SoTrang = this.Sotrang,
                TomTat = this.Tomtat,
                SoLuong = this.Soluong,
                MaLoai = this.Maloai,
                MaNxb = this.Manxb,

                MaTGIds = m_dsTacGiaTam
            };

            bool thanhCong = CXuLySach.themsach(moi);

            if (thanhCong)
            {
                ListSach = CXuLySach.getdssach();
                LamMoi();
                // Reset mảng tạm về rỗng để chuẩn bị cho lần thêm sách tiếp theo
                m_dsTacGiaTam.Clear();
                System.Windows.MessageBox.Show("Thêm sách thành công!");
            }
            else
            {
                System.Windows.MessageBox.Show("Thêm thất bại. Vui lòng kiểm tra lại kết nối API.");
            }
        }
        public bool ThemSach_CanExecute(object parameter)
        {
            return true; // Có thể bổ sung check string.IsNullOrEmpty(Tensach) ở đây
        }

        void Search()
        {
            // 1. Nếu ô tìm kiếm trống -> Tải lại toàn bộ danh sách từ Database
            if (string.IsNullOrWhiteSpace(Keyword))
            {
                ListSach = CXuLySach.getdssach(); // Lấy lại toàn bộ sách gốc
                return;
            }

            string lower = Keyword.Trim().ToLower();

            // 2. Bước A: Lấy lại danh sách đầy đủ nhất từ API trước khi lọc
            var danhSachGoc = CXuLySach.getdssach();

            // 3. Bước B: Tiến hành lọc trên danh sách gốc
            ListSach = danhSachGoc.Where(x =>
                (x.TenSach != null && x.TenSach.ToLower().Contains(lower)) ||
                (x.TenTacGia != null && x.TenTacGia.ToLower().Contains(lower)) ||
                (x.TenLoai != null && x.TenLoai.ToLower().Contains(lower)) // Lọc theo thể loại
            ).ToList();
        }

        // Sửa Sách
        public void SuaSach_Execute(object parameter)
        {
            CSach update = new CSach
            {
                MaSach = SelectedSach.MaSach,
                TenSach = this.Tensach,
                NamXb = this.Namxb,
                SoTrang = this.Sotrang,
                TomTat = this.Tomtat,
                SoLuong = this.Soluong,
                MaLoai = this.Maloai,
                MaNxb = this.Manxb
            };

            bool thanhCong = CXuLySach.suasach(update);

            if (thanhCong)
            {
                System.Windows.MessageBox.Show("Sửa sách thành công!", "Thông báo");
                ListSach = CXuLySach.getdssach();
                LamMoi();
            }
            else
            {
                System.Windows.MessageBox.Show("Sửa thất bại. Vui lòng thử lại.", "Lỗi");
            }
        }
        public bool SuaSach_CanExecute(object parameter)
        {
            return SelectedSach != null;
        }

        // Xóa Sách
        public void XoaSach_Execute(object parameter)
        {
            var xacNhan = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa sách này không?",
                                                         "Xác nhận xóa",
                                                         System.Windows.MessageBoxButton.YesNo,
                                                         System.Windows.MessageBoxImage.Warning);

            if (xacNhan == System.Windows.MessageBoxResult.Yes)
            {
                bool thanhCong = CXuLySach.xoasach(SelectedSach.MaSach);

                if (thanhCong)
                {
                    ListSach = CXuLySach.getdssach();
                    LamMoi();
                    System.Windows.MessageBox.Show("Xóa sách thành công!", "Thông báo");
                }
                else
                {
                    System.Windows.MessageBox.Show("Xóa thất bại. Có thể sách đang được tham chiếu ở bảng mượn trả.", "Lỗi");
                }
            }
        }
        public bool XoaSach_CanExecute(object parameter)
        {
            return SelectedSach != null;
        }

        // Clear dữ liệu Form
        private void LamMoi()
        {
            Tensach = string.Empty;
            Namxb = null;
            Sotrang = null;
            Tomtat = string.Empty;
            Soluong = null;
            Maloai = null;
            Manxb = null;
            SelectedSach = null;
        }
        public void MoKhoSach_Execute(object parameter)
        {
            // Khởi tạo cửa sổ Sách Mượn và truyền vào Mã sách đang chọn trên DataGrid
            UI.QL_SachMuon windowSachMuon = new UI.QL_SachMuon(SelectedSach.MaSach);
            windowSachMuon.ShowDialog(); // Mở lên dưới dạng cửa sổ con (Popup)
        }
        public bool MoKhoSach_CanExecute(object parameter)
        {
            // Chỉ cho phép bấm nút này khi người dùng ĐÃ CHỌN một dòng sách trên DataGrid
            return SelectedSach != null;
        }

    }
}