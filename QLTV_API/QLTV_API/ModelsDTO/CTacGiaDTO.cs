namespace QLTV_API.ModelsDTO
{
    public class CTacGiaDTO
    {
        public int MaTg { get; set; }
        public string TenTg { get; set; } = null!;
        public string? TieuSu { get; set; }
        
        public string? Butdanh { get; set; } 
        public int? Namsinh { get; set; }
    }
}