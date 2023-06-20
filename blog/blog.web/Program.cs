using blog.web.Data;
using blog.web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace blog.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            // inject DbContext 
            builder.Services.AddDbContext<BloggieDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("BlDbConnectionString")));

            // inject DbContext for auth 
            builder.Services.AddDbContext<AuthDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("BlAuthDBConnectionString")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>();



            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}