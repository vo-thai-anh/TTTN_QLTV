using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuanLyThuVien_WPF.Models
{
    public partial class qlthuvienContext : DbContext
    {
        public qlthuvienContext()
        {
        }

        public qlthuvienContext(DbContextOptions<qlthuvienContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Docgia> Docgias { get; set; }
        public virtual DbSet<Loaisach> Loaisaches { get; set; }
        public virtual DbSet<Nhaxuatban> Nhaxuatbans { get; set; }
        public virtual DbSet<Nhanvien> Nhanviens { get; set; }
        public virtual DbSet<PhieuMuon> PhieuMuons { get; set; }
        public virtual DbSet<PhieuTra> PhieuTras { get; set; }
        public virtual DbSet<Sach> Saches { get; set; }
        public virtual DbSet<SachMuon> SachMuons { get; set; }
        public virtual DbSet<ChiTietMuon> ChiTietMuons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning Nên đưa connection string vào appsettings.json
                optionsBuilder.UseSqlServer("Data Source=EMTHAIXITIN;Initial Catalog=qltv;Integrated Security=True;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Docgia>(entity =>
            {
                entity.HasKey(e => e.Madocgia);

                entity.ToTable("DocGia");

                entity.Property(e => e.Madocgia).HasColumnName("MaDocGia");

                entity.Property(e => e.Hoten)
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(100);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(20);
            });


            modelBuilder.Entity<Loaisach>(entity =>
            {
                entity.HasKey(e => e.Maloai);

                entity.ToTable("LoaiSach");

                entity.Property(e => e.Maloai).HasColumnName("MaLoai");

                entity.Property(e => e.Tenloai)
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<Nhaxuatban>(entity =>
            {
                entity.HasKey(e => e.Manxb);

                entity.ToTable("NhaXuatBan");

                entity.Property(e => e.Manxb).HasColumnName("MaNXB");

                entity.Property(e => e.Tennxb)
                    .HasMaxLength(150);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(20);
            });


            modelBuilder.Entity<Nhanvien>(entity =>
            {
                entity.HasKey(e => e.Manv);

                entity.ToTable("NhanVien");

                entity.Property(e => e.Manv).HasColumnName("MaNV");

                entity.Property(e => e.Hoten)
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(100);
            });


            modelBuilder.Entity<Sach>(entity =>
            {
                entity.HasKey(e => e.Masach);

                entity.ToTable("Sach");

                entity.Property(e => e.Masach).HasColumnName("MaSach");

                entity.Property(e => e.Tensach)
                    .HasMaxLength(200);

                entity.HasOne(d => d.MaloaiNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.Maloai)
                    .HasConstraintName("FK_Sach_LoaiSach");

                entity.HasOne(d => d.ManxbNavigation)
                    .WithMany(p => p.Saches)
                    .HasForeignKey(d => d.Manxb)
                    .HasConstraintName("FK_Sach_NhaXuatBan");
            });


            modelBuilder.Entity<SachMuon>(entity =>
            {
                entity.HasKey(e => e.Masachmuon);

                entity.ToTable("SachMuon");

                entity.Property(e => e.Masachmuon)
                    .HasColumnName("MaSachMuon");

                entity.HasOne(d => d.MasachNavigation)
                    .WithMany(p => p.SachMuons)
                    .HasForeignKey(d => d.Masach)
                    .HasConstraintName("FK_SachMuon_Sach");
            });


            modelBuilder.Entity<PhieuMuon>(entity =>
            {
                entity.HasKey(e => e.Maphieumuon);

                entity.ToTable("PhieuMuon");

                entity.Property(e => e.Maphieumuon)
                    .HasColumnName("MaPhieuMuon");

                entity.HasOne(d => d.MadocgiaNavigation)
                    .WithMany(p => p.PhieuMuons)
                    .HasForeignKey(d => d.Madocgia)
                    .HasConstraintName("FK_PhieuMuon_DocGia");

                entity.HasOne(d => d.ManhanvienNavigation)
                    .WithMany(p => p.PhieuMuons)
                    .HasForeignKey(d => d.Manhanvien)
                    .HasConstraintName("FK_PhieuMuon_NhanVien");
            });


            modelBuilder.Entity<PhieuTra>(entity =>
            {
                entity.HasKey(e => e.Maphieutra);

                entity.ToTable("PhieuTra");

                entity.Property(e => e.Maphieutra)
                    .HasColumnName("MaPhieuTra");

                entity.HasOne(d => d.ManhanvienNavigation)
                    .WithMany(p => p.PhieuTras)
                    .HasForeignKey(d => d.Manhanvien)
                    .HasConstraintName("FK_PhieuTra_NhanVien");
            });


            modelBuilder.Entity<ChiTietMuon>(entity =>
            {
                entity.HasKey(e => new { e.Maphieumuon, e.Masachmuon });

                entity.ToTable("ChiTietMuon");

                entity.HasOne(d => d.MaphieumuonNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.Maphieumuon)
                    .HasConstraintName("FK_CT_PhieuMuon");

                entity.HasOne(d => d.MasachmuonNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.Masachmuon)
                    .HasConstraintName("FK_CT_SachMuon");

                entity.HasOne(d => d.MaphieutraNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.Maphieutra)
                    .HasConstraintName("FK_CT_PhieuTra");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
