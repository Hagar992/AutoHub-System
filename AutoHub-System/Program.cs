using AutoHub_System.Repositories;
using AutoHub_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace AutoHub_System
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==========================
            // FORCE DEVELOPMENT LOCALLY
            // ==========================
            builder.Environment.EnvironmentName = "Development";

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

            // Configure Cookie Authentication
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Login";
            });

            // Stripe Settings
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            // Generic Repositories & Services
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(AutoHub_System.Services.IService<>), typeof(AutoHub_System.Services.Service<>));

            // Specific Repositories
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<IDepositePolicyRepository, DepositePolicyRepository>();
            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Specific Services
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDepositePolicyService, DepositePolicyService>();
            builder.Services.AddScoped<IContactService, ContactService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            // Stripe key validation
            var stripeSecretKey = builder.Configuration["Stripe:SecretKey"];
            if (string.IsNullOrWhiteSpace(stripeSecretKey))
            {
                throw new InvalidOperationException("Stripe Secret Key is missing or empty in appsettings.json");
            }
            StripeConfiguration.ApiKey = stripeSecretKey;

            var app = builder.Build();

            // ==========================
            // SEED ROLES AND ADMIN USER
            // ==========================
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
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine("ADMIN SEED ERROR: " + error.Description);
                        }
                    }
                }
            }

            // ==========================
            // ERROR HANDLING
            // ==========================
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // SHOW FULL EXCEPTIONS
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // SAFE PAGE IN PRODUCTION
                app.UseHsts();
            }

            // Middleware pipeline
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            // Routing
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
