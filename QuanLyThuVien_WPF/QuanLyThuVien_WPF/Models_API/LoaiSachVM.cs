using QuanLyThuVien_WPF.Models;
using QuanLyThuVien_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien_WPF.Models_API
{
    class LoaiSachVM:CBaseMVVM
    {
        private List<Loaisach> m_listloaiSach;
        public List<Loaisach> ListloaiSach
        {
            get { return m_listloaiSach; }
            set
            {
                m_listloaiSach = value;
                NotifyPropertyChanged("ListloaiSach");
            }
        }
        public LoaiSachVM()
        {
            ListloaiSach = CXuLyLoaiSach.getdsls();
        }
    }
}
