namespace QuanLyThuVien.Models
{
    public class TacGiac
    {
        public TacGiac()
        {
            Saches = new HashSet<Sach>();
        }

        public int MaTG { get; set; }
        public string TenTG { get; set; } = null!;
        public string TieuSu { get; set; } = null!;

        public virtual ICollection<Sach> Saches { get; set; }
    }
}
