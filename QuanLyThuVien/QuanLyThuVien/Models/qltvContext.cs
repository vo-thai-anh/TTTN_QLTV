using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuanLyThuVien.Models
{
    public partial class qltvContext : DbContext
    {
        public qltvContext()
        {
        }

        public qltvContext(DbContextOptions<qltvContext> options)
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=EMTHAIXITIN;Initial Catalog=qltv;Integrated Security=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietMuon>(entity =>
            {
                entity.HasKey(e => new { e.Maphieumuon, e.Masachmuon });

                entity.ToTable("ChiTietMuon");

                entity.HasIndex(e => e.Maphieutra, "IX_ChiTietMuon_Maphieutra");

                entity.HasIndex(e => e.Masachmuon, "IX_ChiTietMuon_Masachmuon");

                entity.HasOne(d => d.MaphieumuonNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.Maphieumuon)
                    .HasConstraintName("FK_CT_PhieuMuon");

                entity.HasOne(d => d.MaphieutraNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.Maphieutra)
                    .HasConstraintName("FK_CT_PhieuTra");

                entity.HasOne(d => d.MasachmuonNavigation)
                    .WithMany(p => p.ChiTietMuons)
                    .HasForeignKey(d => d.Masachmuon)
                    .HasConstraintName("FK_CT_SachMuon");
            });

            modelBuilder.Entity<DocGia>(entity =>
            {
                entity.HasKey(e => e.MaDocGia);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Hoten).HasMaxLength(100);

                entity.Property(e => e.Sdt).HasMaxLength(20);
            });

            modelBuilder.Entity<LoaiSach>(entity =>
            {
                entity.HasKey(e => e.MaLoai);

                entity.ToTable("LoaiSach");

                entity.Property(e => e.Tenloai).HasMaxLength(100);
            });

            modelBuilder.Entity<NhaXuatBan>(entity =>
            {
                entity.HasKey(e => e.MaNxb);

                entity.ToTable("NhaXuatBan");

                entity.Property(e => e.MaNxb).HasColumnName("MaNXB");

                entity.Property(e => e.Sdt).HasMaxLength(20);

                entity.Property(e => e.Tennxb).HasMaxLength(150);
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNv);

                entity.ToTable("NhanVien");

                entity.Property(e => e.MaNv).HasColumnName("MaNV");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Hoten).HasMaxLength(100);
            });

            modelBuilder.Entity<PhieuMuon>(entity =>
            {
                entity.HasKey(e => e.MaPhieuMuon);

                entity.ToTable("PhieuMuon");

                entity.HasIndex(e => e.Madocgia, "IX_PhieuMuon_Madocgia");

                entity.HasIndex(e => e.Manhanvien, "IX_PhieuMuon_Manhanvien");

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
                entity.HasKey(e => e.MaPhieuTra);

                entity.ToTable("PhieuTra");

                entity.HasIndex(e => e.Manhanvien, "IX_PhieuTra_Manhanvien");

                entity.HasOne(d => d.ManhanvienNavigation)
                    .WithMany(p => p.PhieuTras)
                    .HasForeignKey(d => d.Manhanvien)
                    .HasConstraintName("FK_PhieuTra_NhanVien");
            });

            modelBuilder.Entity<Sach>(entity =>
            {
                entity.HasKey(e => e.MaSach);

                entity.ToTable("Sach");

                entity.HasIndex(e => e.Maloai, "IX_Sach_Maloai");

                entity.HasIndex(e => e.Manxb, "IX_Sach_Manxb");

                entity.Property(e => e.Tensach).HasMaxLength(200);

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
                entity.HasKey(e => e.MaSachMuon);

                entity.ToTable("SachMuon");

                entity.HasIndex(e => e.Masach, "IX_SachMuon_Masach");

                entity.HasOne(d => d.MasachNavigation)
                    .WithMany(p => p.SachMuons)
                    .HasForeignKey(d => d.Masach)
                    .HasConstraintName("FK_SachMuon_Sach");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
