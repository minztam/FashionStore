namespace FashionStore.DTO
{
    public class PhuongThucThanhToanDTO
    {
        public int Ma_PhuongThuc { get; set; }
        public string Ten_PhuongThuc { get; set; } = string.Empty;
        public int MaPhuongThuc { get; internal set; }
    }
}
