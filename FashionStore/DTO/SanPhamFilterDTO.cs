namespace FashionStore.DTO
{
    public class SanPhamFilterDTO
    {
        public string? TuKhoa { get; set; }          // tìm theo tên
        public string? MaDanhMuc { get; set; }       // lọc theo danh mục
        public string? MauSac { get; set; }          // lọc biến thể
        public string? KichThuoc { get; set; }       // lọc biến thể
        public decimal? GiaTu { get; set; }          // khoảng giá
        public decimal? GiaDen { get; set; }
        public bool? ConHang { get; set; }           // còn hàng hay không

        public string? SortBy { get; set; }          // giá_tang, giá_giam, moi_nhat

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
