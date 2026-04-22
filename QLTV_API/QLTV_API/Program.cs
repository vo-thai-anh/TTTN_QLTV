using Microsoft.EntityFrameworkCore;
using QLTV_API.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. CÁC DỊCH VỤ CƠ BẢN
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- PHẦN CODE NHIỆM VỤ 4 (CỦA AN) ---
// Cấu hình Database
builder.Services.AddDbContext<QuanLyThuVienContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Controllers và xử lý lỗi vòng lặp JSON
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// ----------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();