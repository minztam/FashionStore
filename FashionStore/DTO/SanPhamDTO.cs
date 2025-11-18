namespace FashionStore.DTO
{
    public class SanPhamDTO
    {
        public string Ma_SanPham { get; set; } = string.Empty;
        public string Ten_SanPham { get; set; } = string.Empty;
        public string Ma_DanhMuc { get; set; } = string.Empty;
        public string? Mo_Ta { get; set; }
        public bool Trang_Thai { get; set; } = true;
        // Danh sách biến thể sản phẩm (mới, thay thế cho Gia, So_Luong, Mau_Sac, Kich_Thuoc)
        public List<SanPhamBienTheDTO>? BienThes { get; set; }
        // Danh sách hình ảnh (giữ nguyên)
        public List<HinhAnhDTO>? HinhAnhs { get; set; }
    }
}
