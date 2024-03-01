namespace SignalR_proj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.ClientTimeoutInterval = System.TimeSpan.FromMinutes(2);
                hubOptions.KeepAliveInterval = System.TimeSpan.FromMinutes(2);
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<SignalR_proj.Hubs.KDA_Hub_1>("/Chat");
            });

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
