using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RanNetwork.Data;
using RanNetwork.Models;
using System.Text;

namespace RanNetwork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddMvc();
            builder.Services.AddSignalR();
            builder.Services.AddResponseCompression();

            //Add connetdatabase
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("MSSQL") ?? "Data Source=127.0.0.1;Initial Catalog=RanNetwork;Persist Security Info=True;TrustServerCertificate=True; User ID=sa;Password=Admin123@"));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                       .AddEntityFrameworkStores<ApplicationDbContext>()
                       .AddDefaultTokenProviders();

            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            builder.Services.AddMemoryCache();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = "RanNetwork"; // Tên cookie
                options.Cookie.Domain = ""; // Tên miền cookie áp dụng (nếu có)
                options.Cookie.Path = "/"; // Đường dẫn cookie áp dụng
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Chính sách bảo mật (SameAsRequest, Always, None)
                options.Cookie.HttpOnly = true; // Cookie chỉ được truy cập bằng HTTP (không bằng JavaScript)
                options.Cookie.SameSite = SameSiteMode.Strict; // Giới hạn cookie chỉ được gửi trong cùng nguồn (Strict, Lax, None)
                options.LoginPath = "/dang-nhap";
                options.ExpireTimeSpan = TimeSpan.FromDays(7); // Thời gian hết hạn của cookie
                options.SlidingExpiration = true; // Cho phép cập nhật thời gian hết hạn khi có hoạt động từ người dùng
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}