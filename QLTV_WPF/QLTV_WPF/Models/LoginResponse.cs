using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_WPF.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string role { get; set; }
        public string username { get; set; }
        public int manv { get; set; }

    }
}
