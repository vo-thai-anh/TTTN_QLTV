using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_WPF.Models
{
    public class CTacGiaChon : TacGia
    {
        private bool m_isSelected;
        public bool IsSelected
        {
            get => m_isSelected;
            set { m_isSelected = value; } 
        }
    }
}
