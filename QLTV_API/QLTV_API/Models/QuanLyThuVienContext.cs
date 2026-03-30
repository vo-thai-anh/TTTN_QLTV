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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=EMTHAIXITIN;Initial Catalog=QuanLyThuVien;Integrated Security=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietMuon>(entity =>
            {
                entity.HasKey(e => new { e.MaPhieuMuon, e.MaSachMuon })
                    .HasName("PK__ChiTietM__5152017268A420CE");

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
                    .HasConstraintName("FK__ChiTietMu__MaPhi__5165187F");

                entity.HasOne(d => d.MaPhieuTraNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.MaPhieuTra)
                    .HasConstraintName("FK__ChiTietMu__MaPhi__534D60F1");

                entity.HasOne(d => d.MaSachMuonNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.MaSachMuon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietMu__MaSac__52593CB8");
            });

            modelBuilder.Entity<DocGia>(entity =>
            {
                entity.HasKey(e => e.MaDocGia)
                    .HasName("PK__DocGia__F165F945F812173C");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SDT");
            });

            modelBuilder.Entity<LoaiSach>(entity =>
            {
                entity.HasKey(e => e.MaLoai)
                    .HasName("PK__LoaiSach__730A5759901A478D");

                entity.ToTable("LoaiSach");

                entity.Property(e => e.MoTa).HasMaxLength(255);

                entity.Property(e => e.TenLoai).HasMaxLength(100);
            });

            modelBuilder.Entity<NhaXuatBan>(entity =>
            {
                entity.HasKey(e => e.MaNxb)
                    .HasName("PK__NhaXuatB__3A19482CAE502F4B");

                entity.ToTable("NhaXuatBan");

                entity.Property(e => e.MaNxb).HasColumnName("MaNXB");

                entity.Property(e => e.DiaChi).HasMaxLength(255);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SDT");

                entity.Property(e => e.TenNxb)
                    .HasMaxLength(150)
                    .HasColumnName("TenNXB");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNv)
                    .HasName("PK__NhanVien__2725D70A9CF9D9D5");

                entity.ToTable("NhanVien");

                entity.HasIndex(e => e.TaiKhoan, "UQ__NhanVien__D5B8C7F0B10D9FAE")
                    .IsUnique();

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.ChucVu).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SDT");

                entity.Property(e => e.TaiKhoan)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PhieuMuon>(entity =>
            {
                entity.HasKey(e => e.MaPhieuMuon)
                    .HasName("PK__PhieuMuo__C4C82222E6645F44");

                entity.ToTable("PhieuMuon");

                entity.Property(e => e.GhiChu).HasMaxLength(255);

                entity.Property(e => e.NgayMuon).HasColumnType("date");

                entity.Property(e => e.NgayTra).HasColumnType("date");

                entity.HasOne(d => d.MaDocGiaNavigation)
                    .WithMany(p => p.PhieuMuons)
                    .HasForeignKey(d => d.MaDocGia)
                    .HasConstraintName("FK__PhieuMuon__MaDoc__48CFD27E");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.PhieuMuons)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK__PhieuMuon__MaNha__49C3F6B7");
            });

            modelBuilder.Entity<PhieuTra>(entity =>
            {
                entity.HasKey(e => e.MaPhieuTra)
                    .HasName("PK__PhieuTra__1D880A463EE4B7AD");

                entity.ToTable("PhieuTra");

                entity.Property(e => e.GhiChu).HasMaxLength(255);

                entity.Property(e => e.NgayTra).HasColumnType("date");

                entity.Property(e => e.TongTienPhat)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.PhieuTras)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK__PhieuTra__MaNhan__4D94879B");
            });

            modelBuilder.Entity<Sach>(entity =>
            {
                entity.HasKey(e => e.MaSach)
                    .HasName("PK__Sach__B235742D12AD972D");

                entity.ToTable("Sach");

                entity.Property(e => e.MaNxb).HasColumnName("MaNXB");

                entity.Property(e => e.NamXb).HasColumnName("NamXB");

                entity.Property(e => e.TenSach).HasMaxLength(200);

                entity.HasOne(d => d.MaLoaiNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.MaLoai)
                    .HasConstraintName("FK__Sach__MaLoai__4222D4EF");

                entity.HasOne(d => d.MaNxbNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.MaNxb)
                    .HasConstraintName("FK__Sach__MaNXB__4316F928");

                entity.HasMany(d => d.MaTgs)
                    .WithMany(p => p.MaSaches)
                    .UsingEntity<Dictionary<string, object>>(
                        "SachTacGium",
                        l => l.HasOne<TacGia>().WithMany().HasForeignKey("MaTg").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Sach_TacGi__MaTG__571DF1D5"),
                        r => r.HasOne<Sach>().WithMany().HasForeignKey("MaSach").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__Sach_TacG__MaSac__5629CD9C"),
                        j =>
                        {
                            j.HasKey("MaSach", "MaTg").HasName("PK__Sach_Tac__E047242AC5D92F86");

                            j.ToTable("Sach_TacGia");

                            j.IndexerProperty<int>("MaTg").HasColumnName("MaTG");
                        });
            });

            modelBuilder.Entity<SachMuon>(entity =>
            {
                entity.HasKey(e => e.MaSachMuon)
                    .HasName("PK__SachMuon__59A235086C6C957D");

                entity.ToTable("SachMuon");

                entity.Property(e => e.TinhTrang).HasMaxLength(100);

                entity.HasOne(d => d.MaSachNavigation)
                    .WithMany(p => p.SachMuons)
                    .HasForeignKey(d => d.MaSach)
                    .HasConstraintName("FK__SachMuon__MaSach__45F365D3");
            });

            modelBuilder.Entity<TacGia>(entity =>
            {
                entity.HasKey(e => e.MaTg)
                    .HasName("PK__TacGia__27250074E9A0C9AC");

                entity.Property(e => e.MaTg).HasColumnName("MaTG");

                entity.Property(e => e.TenTg)
                    .HasMaxLength(100)
                    .HasColumnName("TenTG");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
