using QLTV_WPF.Models;
using QLTV_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_WPF.Models_API
{
    class LoaiSachVM:CBaseMVVM
    {
        private List<LoaiSach> m_listloaiSach;
        public List<LoaiSach> ListloaiSach
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
