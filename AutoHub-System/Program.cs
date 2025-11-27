using AutoHub_System.Repositories;
using Microsoft.EntityFrameworkCore;
namespace AutoHub_System
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Register DbContext + Connection String
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineConnection"))
            );

            // Register Identity
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Register generic repositories and services
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

            // Register specific repositories - ADD THIS
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<IDepositePolicyRepository, DepositePolicyRepository>();
            
            // Register specific services
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDepositePolicyService, DepositePolicyService>();

            var app = builder.Build();

            // Seed roles and default admin
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string[] roles = { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Seed default admin
                var adminEmail = "Yahya@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var admin = new User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        Name = "System Admin",
                        Address = "Headquarters",
                        ProfilePicture = "https://res.cloudinary.com/dmsmksagp/image/upload/v1764107340/profiles/pzk48nhlmgrp4vpnhpyx.jpg",
                        DateRegistered = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(admin, "Yahya951753*");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}