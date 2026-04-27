using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_WPF.Models
{
    public static class CSessionManager
    {
        public static string Token { get; set; }

        public static string Role { get; set; }

        public static string Username { get; set; }

        public static int MaNV { get; set; }


        public static void Logout()
        {
            Token = null;
            Role = null;
            Username = null;
            MaNV = 0;
        }


        public static bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(Token);
        }


        public static bool IsManager()
        {
            return Role == "Quản lý";
        }


        public static bool IsStaff()
        {
            return Role == "Nhân viên";
        }
    }
}
