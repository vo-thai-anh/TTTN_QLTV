using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLTV_WPF.Migrations
{
    public partial class QLTV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocGia",
                columns: table => new
                {
                    MaDocGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    SDT = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DocGia__F165F945F812173C", x => x.MaDocGia);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSach",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LoaiSach__730A5759901A478D", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChucVu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaiKhoan = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    MatKhau = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    SDT = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanVien__2725D70A9CF9D9D5", x => x.MaNV);
                });

            migrationBuilder.CreateTable(
                name: "NhaXuatBan",
                columns: table => new
                {
                    MaNXB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNXB = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SDT = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhaXuatB__3A19482CAE502F4B", x => x.MaNXB);
                });

            migrationBuilder.CreateTable(
                name: "TacGia",
                columns: table => new
                {
                    MaTG = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTG = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TieuSu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TacGia__27250074E9A0C9AC", x => x.MaTG);
                });

            migrationBuilder.CreateTable(
                name: "PhieuMuon",
                columns: table => new
                {
                    MaPhieuMuon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDocGia = table.Column<int>(type: "int", nullable: true),
                    MaNhanVien = table.Column<int>(type: "int", nullable: true),
                    NgayMuon = table.Column<DateTime>(type: "date", nullable: true),
                    NgayTra = table.Column<DateTime>(type: "date", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuMuo__C4C82222E6645F44", x => x.MaPhieuMuon);
                    table.ForeignKey(
                        name: "FK__PhieuMuon__MaDoc__48CFD27E",
                        column: x => x.MaDocGia,
                        principalTable: "DocGia",
                        principalColumn: "MaDocGia");
                    table.ForeignKey(
                        name: "FK__PhieuMuon__MaNha__49C3F6B7",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "PhieuTra",
                columns: table => new
                {
                    MaPhieuTra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNhanVien = table.Column<int>(type: "int", nullable: true),
                    NgayTra = table.Column<DateTime>(type: "date", nullable: true),
                    TongTienPhat = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0))"),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PhieuTra__1D880A463EE4B7AD", x => x.MaPhieuTra);
                    table.ForeignKey(
                        name: "FK__PhieuTra__MaNhan__4D94879B",
                        column: x => x.MaNhanVien,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "Sach",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSach = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NamXB = table.Column<int>(type: "int", nullable: true),
                    SoTrang = table.Column<int>(type: "int", nullable: true),
                    TomTat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    MaLoai = table.Column<int>(type: "int", nullable: true),
                    MaNXB = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sach__B235742D12AD972D", x => x.MaSach);
                    table.ForeignKey(
                        name: "FK__Sach__MaLoai__4222D4EF",
                        column: x => x.MaLoai,
                        principalTable: "LoaiSach",
                        principalColumn: "MaLoai");
                    table.ForeignKey(
                        name: "FK__Sach__MaNXB__4316F928",
                        column: x => x.MaNXB,
                        principalTable: "NhaXuatBan",
                        principalColumn: "MaNXB");
                });

            migrationBuilder.CreateTable(
                name: "Sach_TacGia",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    MaTG = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sach_Tac__E047242AC5D92F86", x => new { x.MaSach, x.MaTG });
                    table.ForeignKey(
                        name: "FK__Sach_TacG__MaSac__5629CD9C",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach");
                    table.ForeignKey(
                        name: "FK__Sach_TacGi__MaTG__571DF1D5",
                        column: x => x.MaTG,
                        principalTable: "TacGia",
                        principalColumn: "MaTG");
                });

            migrationBuilder.CreateTable(
                name: "SachMuon",
                columns: table => new
                {
                    MaSachMuon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSach = table.Column<int>(type: "int", nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SachMuon__59A235086C6C957D", x => x.MaSachMuon);
                    table.ForeignKey(
                        name: "FK__SachMuon__MaSach__45F365D3",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietMuon",
                columns: table => new
                {
                    MaPhieuMuon = table.Column<int>(type: "int", nullable: false),
                    MaSachMuon = table.Column<int>(type: "int", nullable: false),
                    NgayTraThucTe = table.Column<DateTime>(type: "date", nullable: true),
                    TienPhat = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0))"),
                    LyDoPhat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaPhieuTra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietM__5152017268A420CE", x => new { x.MaPhieuMuon, x.MaSachMuon });
                    table.ForeignKey(
                        name: "FK__ChiTietMu__MaPhi__5165187F",
                        column: x => x.MaPhieuMuon,
                        principalTable: "PhieuMuon",
                        principalColumn: "MaPhieuMuon");
                    table.ForeignKey(
                        name: "FK__ChiTietMu__MaPhi__534D60F1",
                        column: x => x.MaPhieuTra,
                        principalTable: "PhieuTra",
                        principalColumn: "MaPhieuTra");
                    table.ForeignKey(
                        name: "FK__ChiTietMu__MaSac__52593CB8",
                        column: x => x.MaSachMuon,
                        principalTable: "SachMuon",
                        principalColumn: "MaSachMuon");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMuon_MaPhieuTra",
                table: "ChiTietMuon",
                column: "MaPhieuTra");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMuon_MaSachMuon",
                table: "ChiTietMuon",
                column: "MaSachMuon");

            migrationBuilder.CreateIndex(
                name: "UQ__NhanVien__D5B8C7F0B10D9FAE",
                table: "NhanVien",
                column: "TaiKhoan",
                unique: true,
                filter: "[TaiKhoan] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuMuon_MaDocGia",
                table: "PhieuMuon",
                column: "MaDocGia");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuMuon_MaNhanVien",
                table: "PhieuMuon",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuTra_MaNhanVien",
                table: "PhieuTra",
                column: "MaNhanVien");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_MaLoai",
                table: "Sach",
                column: "MaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_MaNXB",
                table: "Sach",
                column: "MaNXB");

            migrationBuilder.CreateIndex(
                name: "IX_Sach_TacGia_MaTG",
                table: "Sach_TacGia",
                column: "MaTG");

            migrationBuilder.CreateIndex(
                name: "IX_SachMuon_MaSach",
                table: "SachMuon",
                column: "MaSach");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietMuon");

            migrationBuilder.DropTable(
                name: "Sach_TacGia");

            migrationBuilder.DropTable(
                name: "PhieuMuon");

            migrationBuilder.DropTable(
                name: "PhieuTra");

            migrationBuilder.DropTable(
                name: "SachMuon");

            migrationBuilder.DropTable(
                name: "TacGia");

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
