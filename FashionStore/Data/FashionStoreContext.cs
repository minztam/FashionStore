using Microsoft.EntityFrameworkCore;
using FashionStore.Models;

namespace FashionStore.Data
{
    public class FashionStoreContext: DbContext
    {
        public FashionStoreContext(DbContextOptions<FashionStoreContext> options) : base(options){}

        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DanhMuc> DanhMuc { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<HinhAnhSanPham> HinhAnhSanPhams { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ VaiTro - TaiKhoan (1 - nhiều)
            modelBuilder.Entity<TaiKhoan>()
                .HasOne(t => t.VaiTro)
                .WithMany(v => v.TaiKhoans)
                .HasForeignKey(t => t.Ma_VaiTro);

            // Quan hệ TaiKhoan - NhanVien (1 - 1)
            modelBuilder.Entity<NhanVien>()
                .HasOne(n => n.TaiKhoan)
                .WithOne(t => t.NhanVien)
                .HasForeignKey<NhanVien>(n => n.Ma_TaiKhoan);

            // Quan hệ TaiKhoan - KhachHang (1 - 1) 
            modelBuilder.Entity<KhachHang>()
                .HasOne(k => k.TaiKhoan)
                .WithOne(t => t.KhachHang)
                .HasForeignKey<KhachHang>(k => k.Ma_TaiKhoan);

            // Khóa chính ghép ChiTietDonHang
            modelBuilder.Entity<ChiTietDonHang>()
                .HasKey(ct => new { ct.Ma_DonHang, ct.Ma_SanPham });

            // Khóa chính ghép ChiTietGioHang
            modelBuilder.Entity<ChiTietGioHang>()
                .HasKey(ct => new { ct.Ma_GioHang, ct.Ma_SanPham });

            //
            modelBuilder.Entity<DanhMuc>()
                .HasOne(dm => dm.DanhMucCha)
                .WithMany(dm => dm.DanhMucCon)
                .HasForeignKey(dm => dm.Ma_DanhMucCha)
                .OnDelete(DeleteBehavior.Restrict);

            // SanPham → DanhMuc
            modelBuilder.Entity<SanPham>()
                .HasOne(sp => sp.DanhMuc)
                .WithMany(dm => dm.SanPhams)
                .HasForeignKey(sp => sp.Ma_DanhMuc)
                .OnDelete(DeleteBehavior.Restrict);

            // HinhAnhSanPham → SanPham
            modelBuilder.Entity<HinhAnhSanPham>()
                .HasOne(ha => ha.SanPham)
                .WithMany(sp => sp.HinhAnhSanPhams)
                .HasForeignKey(ha => ha.Ma_SanPham)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.Property(e => e.Gia)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Gia_Giam)
                      .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.Property(e => e.DonGia)
                      .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.Property(e => e.Tong_Tien)
                      .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.Property(e => e.GiaTri_ToiThieu)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Giam_Tien)
                      .HasColumnType("decimal(18,2)");
            });
        }

    }
}
