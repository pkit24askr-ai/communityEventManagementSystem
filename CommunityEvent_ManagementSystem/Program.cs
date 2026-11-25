using CommunityEvent_ManagementSystem.Interfaces;
using CommunityEvent_ManagementSystem.Components;
using CommunityEvent_ManagementSystem.Components.Account;
using CommunityEvent_ManagementSystem.Data;
using CommunityEvent_ManagementSystem.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvent_ManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            // Add Swagger for API documentation
            builder.Services.AddSwaggerGen();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            // Application Services
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IParticipantService, ParticipantService>();
            builder.Services.AddScoped<IVenueService, VenueService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            SeedRolesAndAdminAsync(app).GetAwaiter().GetResult();



            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapAdditionalIdentityEndpoints();

            app.MapControllers();

            app.Run();
        }
        private static async Task SeedRolesAndAdminAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Participant"))
            {
                await roleManager.CreateAsync(new IdentityRole("Participant"));
            }

            var adminEmail = "admin@community.com";
            var adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else
            {
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

    }
}