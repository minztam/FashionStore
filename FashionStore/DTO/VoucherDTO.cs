namespace FashionStore.DTO
{
    public class VoucherDTO
    {
        public string Ma_Voucher { get; set; } = string.Empty;
        public int? Giam_PhanTram { get; set; }
        public decimal? Giam_Tien { get; set; }
        public decimal? GiaTri_ToiThieu { get; set; }
        public int? So_LanDung { get; set; }
        public DateTime? Ngay_BatDau { get; set; }
        public DateTime? Ngay_KetThuc { get; set; }
        public bool Trang_Thai { get; set; }
    }
}
