using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CORS_proj_server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var builder1 = new CorsPolicyBuilder();

            builder.Services.AddRazorPages();
            builder.Services.AddCors();
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseCors(builder => {
                builder.WithOrigins("https://localhost:7291")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("VS_version")
                .WithExposedHeaders("Angular_IP");
                builder1 = builder;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var response = context.Response;
                    response.Headers.ContentLanguage = "ru-RU";
                    response.Headers.ContentType = "text/plain; charset=utf-8";
                    context.Response.Headers.Add("VS_version", "17.5.1 - 28.02.2023");
                    context.Response.Headers.Add("Angular_IP", "http://localhost/my-app-angular/index.html");
                    await context.Response.WriteAsync("Приложение CORS_1 отвечает на запрос.");
                });

                endpoints.MapGet("/CorsC", async context =>
                {
                    builder1.WithExposedHeaders("IdentityData");               
                    var response = context.Response;
                    response.Headers.ContentLanguage = "ru-RU";
                    response.Headers.ContentType = "text/plain; charset=utf-8";
                    var id_data = context.Request.Cookies["User_Identity"];

                    if (id_data == null || id_data == "") 
                        id_data = "XXXX";

                    context.Response.Headers.Add("IdentityData", id_data);
                    await context.Response.WriteAsync($"Здравствуйте, {id_data}!");
                });
            });


            app.MapRazorPages();
            app.Run();
        }
    }
}
