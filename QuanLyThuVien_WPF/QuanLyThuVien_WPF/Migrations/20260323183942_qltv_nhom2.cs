using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyThuVien_WPF.Migrations
{
    public partial class qltv_nhom2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocGia",
                columns: table => new
                {
                    MaDocGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hoten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Diachi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocGia", x => x.MaDocGia);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSach",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tenloai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mota = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSach", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hoten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Chucvu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Taikhoan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matkhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "NhaXuatBan",
                columns: table => new
                {
                    MaNXB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tennxb = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Diachi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaXuatBan", x => x.MaNXB);
                });

            migrationBuilder.CreateTable(
                name: "PhieuMuon",
                columns: table => new
                {
                    MaPhieuMuon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Madocgia = table.Column<int>(type: "int", nullable: true),
                    Manhanvien = table.Column<int>(type: "int", nullable: true),
                    Ngaymuon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ngaytra = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ghichu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuMuon", x => x.MaPhieuMuon);
                    table.ForeignKey(
                        name: "FK_PhieuMuon_DocGia",
                        column: x => x.Madocgia,
                        principalTable: "DocGia",
                        principalColumn: "MaDocGia");
                    table.ForeignKey(
                        name: "FK_PhieuMuon_NhanVien",
                        column: x => x.Manhanvien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "PhieuTra",
                columns: table => new
                {
                    MaPhieuTra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Manhanvien = table.Column<int>(type: "int", nullable: true),
                    Ngaytra = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ghichu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuTra", x => x.MaPhieuTra);
                    table.ForeignKey(
                        name: "FK_PhieuTra_NhanVien",
                        column: x => x.Manhanvien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "Sach",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tensach = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Namxb = table.Column<int>(type: "int", nullable: true),
                    Sotrang = table.Column<int>(type: "int", nullable: true),
                    Tomtat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soluong = table.Column<int>(type: "int", nullable: true),
                    Maloai = table.Column<int>(type: "int", nullable: true),
                    Manxb = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sach", x => x.MaSach);
                    table.ForeignKey(
                        name: "FK_Sach_LoaiSach",
                        column: x => x.Maloai,
                        principalTable: "LoaiSach",
                        principalColumn: "MaLoai");
                    table.ForeignKey(
                        name: "FK_Sach_NhaXuatBan",
                        column: x => x.Manxb,
                        principalTable: "NhaXuatBan",
                        principalColumn: "MaNXB");
                });

            migrationBuilder.CreateTable(
                name: "SachMuon",
                columns: table => new
                {
                    MaSachMuon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Masach = table.Column<int>(type: "int", nullable: true),
                    Tinhtrang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SachMuon", x => x.MaSachMuon);
                    table.ForeignKey(
                        name: "FK_SachMuon_Sach",
                        column: x => x.Masach,
                        principalTable: "Sach",
                        principalColumn: "MaSach");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietMuon",
                columns: table => new
                {
                    Maphieumuon = table.Column<int>(type: "int", nullable: false),
                    Masachmuon = table.Column<int>(type: "int", nullable: false),
                    Ngaytrathucte = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Maphieutra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietMuon", x => new { x.Maphieumuon, x.Masachmuon });
                    table.ForeignKey(
                        name: "FK_CT_PhieuMuon",
                        column: x => x.Maphieumuon,
                        principalTable: "PhieuMuon",
                        principalColumn: "MaPhieuMuon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CT_PhieuTra",
                        column: x => x.Maphieutra,
                        principalTable: "PhieuTra",
                        principalColumn: "MaPhieuTra");
                    table.ForeignKey(
                        name: "FK_CT_SachMuon",
                        column: x => x.Masachmuon,
                        principalTable: "SachMuon",
                        principalColumn: "MaSachMuon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMuon_Maphieutra",
                table: "ChiTietMuon",
                column: "Maphieutra");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMuon_Masachmuon",
                table: "ChiTietMuon",
                column: "Masachmuon");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuMuon_Madocgia",
                table: "PhieuMuon",
                column: "Madocgia");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuMuon_Manhanvien",
                table: "PhieuMuon",
                column: "Manhanvien");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuTra_Manhanvien",
                table: "PhieuTra",
                column: "Manhanvien");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_Maloai",
                table: "Sach",
                column: "Maloai");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_Manxb",
                table: "Sach",
                column: "Manxb");

            migrationBuilder.CreateIndex(
                name: "IX_SachMuon_Masach",
                table: "SachMuon",
                column: "Masach");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietMuon");

            migrationBuilder.DropTable(
                name: "PhieuMuon");

            migrationBuilder.DropTable(
                name: "PhieuTra");

            migrationBuilder.DropTable(
                name: "SachMuon");

            migrationBuilder.DropTable(
                name: "DocGia");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "Sach");

            migrationBuilder.DropTable(
                name: "LoaiSach");

            migrationBuilder.DropTable(
                name: "NhaXuatBan");
        }
    }
}
