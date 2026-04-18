using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QLTV_API.Models
{
    public partial class QuanLyThuVienContext : DbContext
    {
        public QuanLyThuVienContext()
        {
        }

        public QuanLyThuVienContext(DbContextOptions<QuanLyThuVienContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietMuon> ChiTietMuons { get; set; } = null!;
        public virtual DbSet<DocGia> DocGia { get; set; } = null!;
        public virtual DbSet<LoaiSach> LoaiSaches { get; set; } = null!;
        public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; } = null!;
        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;
        public virtual DbSet<PhieuMuon> PhieuMuons { get; set; } = null!;
        public virtual DbSet<PhieuTra> PhieuTras { get; set; } = null!;
        public virtual DbSet<Sach> Saches { get; set; } = null!;
        public virtual DbSet<SachMuon> SachMuons { get; set; } = null!;
        public virtual DbSet<TacGia> TacGia { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-TH3M50VI;Initial Catalog=QuanLyThuVien;Integrated Security=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietMuon>(entity =>
            {
                entity.HasKey(e => new { e.MaPhieuMuon, e.MaSachMuon })
                    .HasName("PK__ChiTietM__51520172196B36F2");

                entity.ToTable("ChiTietMuon");

                entity.Property(e => e.LyDoPhat).HasMaxLength(255);
                entity.Property(e => e.NgayTraThucTe).HasColumnType("date");
                entity.Property(e => e.TienPhat)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.MaPhieuMuonNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.MaPhieuMuon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietMu__MaPhi__6477ECF3");

                entity.HasOne(d => d.MaPhieuTraNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.MaPhieuTra)
                    .HasConstraintName("FK__ChiTietMu__MaPhi__66603565");

                entity.HasOne(d => d.MaSachMuonNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.MaSachMuon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietMu__MaSac__656C112C");
            });

            modelBuilder.Entity<DocGia>(entity =>
            {
                entity.HasKey(e => e.MaDocGia)
                    .HasName("PK__DocGia__F165F94524CB6D42");

                entity.Property(e => e.DiaChi).HasMaxLength(255);
                entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.HoTen).HasMaxLength(100);
                entity.Property(e => e.Sdt).HasMaxLength(20).IsUnicode(false).HasColumnName("SDT");
            });

            modelBuilder.Entity<LoaiSach>(entity =>
            {
                entity.HasKey(e => e.MaLoai).HasName("PK__LoaiSach__730A5759A08E0880");
                entity.ToTable("LoaiSach");
                entity.Property(e => e.MoTa).HasMaxLength(255);
                entity.Property(e => e.TenLoai).HasMaxLength(100);
            });

            modelBuilder.Entity<NhaXuatBan>(entity =>
            {
                entity.HasKey(e => e.MaNxb).HasName("PK__NhaXuatB__3A19482CBB21E584");
                entity.ToTable("NhaXuatBan");
                entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
                entity.Property(e => e.DiaChi).HasMaxLength(255);
                entity.Property(e => e.Sdt).HasMaxLength(20).IsUnicode(false).HasColumnName("SDT");
                entity.Property(e => e.TenNxb).HasMaxLength(150).HasColumnName("TenNXB");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70AF4414AC1");
                entity.ToTable("NhanVien");
                entity.HasIndex(e => e.TaiKhoan, "UQ__NhanVien__D5B8C7F0C12CF1F6").IsUnique();
                entity.Property(e => e.MaNv).HasColumnName("MaNV");
                entity.Property(e => e.ChucVu).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.HoTen).HasMaxLength(100);
                entity.Property(e => e.MatKhau).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Sdt).HasMaxLength(20).IsUnicode(false).HasColumnName("SDT");
                entity.Property(e => e.TaiKhoan).HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<PhieuMuon>(entity =>
            {
                entity.HasKey(e => e.MaPhieuMuon).HasName("PK__PhieuMuo__C4C822220698818F");
                entity.ToTable("PhieuMuon");
                entity.Property(e => e.GhiChu).HasMaxLength(255);
                entity.Property(e => e.NgayMuon).HasColumnType("date");
                entity.Property(e => e.NgayTra).HasColumnType("date");

                entity.HasOne(d => d.MaDocGiaNavigation)
                    .WithMany(p => p.PhieuMuons)
                    .HasForeignKey(d => d.MaDocGia)
                    .HasConstraintName("FK__PhieuMuon__MaDoc__5BE2A6F2");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.PhieuMuons)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK__PhieuMuon__MaNha__5CD6CB2B");
            });

            modelBuilder.Entity<PhieuTra>(entity =>
            {
                entity.HasKey(e => e.MaPhieuTra).HasName("PK__PhieuTra__1D880A4651208B9D");
                entity.ToTable("PhieuTra");
                entity.Property(e => e.GhiChu).HasMaxLength(255);
                entity.Property(e => e.NgayTra).HasColumnType("date");
                entity.Property(e => e.TongTienPhat).HasColumnType("decimal(18, 2)").HasDefaultValueSql("((0))");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.PhieuTras)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK__PhieuTra__MaNhan__60A75C0F");
            });

            modelBuilder.Entity<Sach>(entity =>
            {
                entity.HasKey(e => e.MaSach).HasName("PK__Sach__B235742D1F178B85");
                entity.ToTable("Sach");
                entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
                entity.Property(e => e.NamXb).HasColumnName("NamXB");
                entity.Property(e => e.TenSach).HasMaxLength(200);

                entity.HasOne(d => d.MaLoaiNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.MaLoai)
                    .HasConstraintName("FK__Sach__MaLoai__5535A963");

                entity.HasOne(d => d.MaNxbNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.MaNxb)
                    .HasConstraintName("FK__Sach__MaNXB__5629CD9C");

                entity.HasMany(d => d.MaTgs)
                    .WithMany(p => p.MaSaches)
                    .UsingEntity<Dictionary<string, object>>(
                        "SachTacGia",
                        l => l.HasOne<TacGia>().WithMany().HasForeignKey("MaTg").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Sach_TacGi__MaTG__571DF1D5"),
                        r => r.HasOne<Sach>().WithMany().HasForeignKey("MaSach").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Sach_TacG__MaSac__5629CD9C"),
                        j =>
                        {
                            j.HasKey("MaSach", "MaTg").HasName("PK__Sach_Tac__E047242AEBB2FED5");
                            j.ToTable("Sach_TacGia");
                            j.IndexerProperty<int>("MaTg").HasColumnName("MaTG");
                        });
            });

            modelBuilder.Entity<SachMuon>(entity =>
            {
                entity.HasKey(e => e.MaSachMuon).HasName("PK__SachMuon__59A235084F23C039");
                entity.ToTable("SachMuon");
                entity.Property(e => e.TinhTrang).HasMaxLength(100);

                entity.HasOne(d => d.MaSachNavigation)
                    .WithMany(p => p.SachMuons)
                    .HasForeignKey(d => d.MaSach)
                    .HasConstraintName("FK__SachMuon__MaSach__59063A47");
            });

            modelBuilder.Entity<TacGia>(entity =>
            {
                entity.HasKey(e => e.MaTg).HasName("PK__TacGia__27250074FECED078");
                entity.Property(e => e.MaTg).HasColumnName("MaTG");
                entity.Property(e => e.TenTg).HasMaxLength(100).HasColumnName("TenTG");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}