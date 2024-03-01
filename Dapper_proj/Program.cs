using static System.Net.Mime.MediaTypeNames;
using System;
using Dapper_proj.Models;

namespace Dapper_proj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = @"
                Data Source=host.docker.internal,1433;
                Initial Catalog=KDA_tennisists;
                User Id=sa;
                Password=Qwerqwer1;
                Connect Timeout=30;
                Encrypt=False;
                Trust Server Certificate=False;
                Application Intent=ReadWrite;
            ";

            builder.Services.AddTransient<IMyRepository, MyRepository>(provider => new MyRepository(connectionString));

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
