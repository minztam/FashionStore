using FashionStore.DataSeed;
using FashionStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FashionStore.Data
{
    public class FashionStoreContext : DbContext
    {
        public FashionStoreContext(DbContextOptions<FashionStoreContext> options) : base(options) { }

        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<HinhAnhSanPham> HinhAnhSanPhams { get; set; }
        public DbSet<SanPhamBienThe> SanPhamBienThes { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // ===============================
            // 1. SEED DATA DANH MỤC + SẢN PHẨM
            // ===============================
            modelBuilder.Entity<DanhMuc>().HasData(DanhMucSeedData.GetSeedData());
            modelBuilder.Entity<SanPham>().HasData(SanPhamSeedData.GetSeedData());
            modelBuilder.Entity<SanPhamBienThe>().HasData(SanPhamBienTheSeedData.GetSeedData());
            modelBuilder.Entity<HinhAnhSanPham>().HasData(HinhAnhSanPhamSeedData.GetSeedData());

            // ===============================
            // 2. QUAN HỆ VAI TRÒ – TÀI KHOẢN
            // ===============================
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(t => t.VaiTro)
                .WithMany(v => v.TaiKhoans)
                .HasForeignKey(t => t.Ma_VaiTro)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // 3. TÀI KHOẢN – NHÂN VIÊN (1–1)
            // ===============================
            modelBuilder.Entity<NhanVien>()
                .HasOne(n => n.TaiKhoan)
                .WithOne(t => t.NhanVien)
                .HasForeignKey<NhanVien>(n => n.Ma_TaiKhoan);

            // ===============================
            // 4. TÀI KHOẢN – KHÁCH HÀNG (1–1)
            // ===============================
            modelBuilder.Entity<KhachHang>()
                .HasOne(k => k.TaiKhoan)
                .WithOne(t => t.KhachHang)
                .HasForeignKey<KhachHang>(k => k.Ma_TaiKhoan);

            // ===============================
            // 5. KHÓA CHÍNH GHÉP: CHI TIẾT ĐƠN HÀNG
            // ===============================
            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(ct => new { ct.Ma_DonHang, ct.Ma_SanPham });

                entity.Property(ct => ct.Ma_BienThe)
                      .IsRequired(); // Bắt buộc NOT NULL

                entity.HasOne(ct => ct.DonHang)
                      .WithMany(dh => dh.ChiTietDonHangs)
                      .HasForeignKey(ct => ct.Ma_DonHang)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ct => ct.SanPham)
                      .WithMany()
                      .HasForeignKey(ct => ct.Ma_SanPham)
                      .OnDelete(DeleteBehavior.Restrict);

                // DÒNG QUAN TRỌNG NHẤT – ÉP BUỘC KHÔNG DÙNG SET NULL!!!
                entity.HasOne(ct => ct.BienThe)
                      .WithMany()
                      .HasForeignKey(ct => ct.Ma_BienThe)
                      .OnDelete(DeleteBehavior.Restrict)   // hoặc NoAction
                      .IsRequired();
            });

            // ===============================
            // 6. KHÓA CHÍNH GHÉP: CHI TIẾT GIỎ HÀNG
            // ===============================
            modelBuilder.Entity<ChiTietGioHang>(entity =>
            {
                entity.HasKey(ct => new { ct.Ma_GioHang, ct.Ma_SanPham, ct.Ma_BienThe });

                entity.HasOne(ct => ct.GioHang)
                      .WithMany(g => g.ChiTietGioHangs)
                      .HasForeignKey(ct => ct.Ma_GioHang)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ct => ct.SanPham)
                      .WithMany()
                      .HasForeignKey(ct => ct.Ma_SanPham)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ct => ct.BienThe)
                      .WithMany()
                      .HasForeignKey(ct => ct.Ma_BienThe)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===============================
            // 7. DANH MỤC CHA – CON
            // ===============================
            modelBuilder.Entity<DanhMuc>()
                .HasOne(dm => dm.DanhMucCha)
                .WithMany(dm => dm.DanhMucCon)
                .HasForeignKey(dm => dm.Ma_DanhMucCha)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // 8. SẢN PHẨM – DANH MỤC
            // ===============================
            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.DanhMuc)
                .WithMany(dm => dm.SanPhams)
                .HasForeignKey(sp => sp.Ma_DanhMuc)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // 9. HÌNH ẢNH SẢN PHẨM
            // ===============================
            modelBuilder.Entity<HinhAnhSanPham>()
                .HasOne(ha => ha.SanPham)
                .WithMany(sp => sp.HinhAnhSanPhams)
                .HasForeignKey(ha => ha.Ma_SanPham)
                .OnDelete(DeleteBehavior.Cascade);

            // ===============================
            // 10. SẢN PHẨM BIẾN THỂ – SẢN PHẨM
            // ===============================
            modelBuilder.Entity<SanPhamBienThe>()
                .HasOne(bt => bt.SanPham)
                .WithMany(sp => sp.BienThes)
                .HasForeignKey(bt => bt.Ma_SanPham)
                .OnDelete(DeleteBehavior.Cascade);

            // ===============================
            // 11. FORMAT CÁC TRƯỜNG DECIMAL
            // ===============================
            modelBuilder.Entity<SanPhamBienThe>()
                .Property(p => p.Gia_BienThe)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SanPhamBienThe>()
                .Property(p => p.Gia_Giam)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ChiTietDonHang>()
                .Property(p => p.DonGia)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DonHang>()
                .Property(p => p.Tong_Tien)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Voucher>()
                .Property(p => p.GiaTri_ToiThieu)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Voucher>()
                .Property(p => p.Giam_Tien)
                .HasColumnType("decimal(18,2)");

            // ===============================
            // 12. SEED ROLE
            // ===============================
            modelBuilder.Entity<VaiTro>().HasData(
                new VaiTro { Ma_VaiTro = "1111-1111-1111-1111", Ten_VaiTro = "Admin" },
                new VaiTro { Ma_VaiTro = "1111-2222-1111-2222", Ten_VaiTro = "Nhân viên bán hàng" },
                new VaiTro { Ma_VaiTro = "1111-3333-1111-3333", Ten_VaiTro = "Nhân viên kho" },
                new VaiTro { Ma_VaiTro = "2222-2222-2222-2222", Ten_VaiTro = "Khách hàng" }
            );

            // ===============================
            // 13. SEED PHƯƠNG THỨC THANH TOÁN
            // ===============================
            modelBuilder.Entity<PhuongThucThanhToan>(entity =>
            {
                entity.HasKey(e => e.Ma_PhuongThuc);
                //entity.Property(e => e.Ma_PhuongThuc)
                //      .ValueGeneratedNever()
                //      .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            });
            //modelBuilder.Entity<PhuongThucThanhToan>().HasData(
            //    new PhuongThucThanhToan { Ma_PhuongThuc = 1, Ten_PhuongThuc = "Thanh toán khi nhận hàng (COD)" },
            //    new PhuongThucThanhToan { Ma_PhuongThuc = 2, Ten_PhuongThuc = "Ví điện tử" },
            //    new PhuongThucThanhToan { Ma_PhuongThuc = 3, Ten_PhuongThuc = "Chuyển khoản ngân hàng" }
            //);
        }
    }
}
