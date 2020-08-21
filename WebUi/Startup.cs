using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Domain.Repositories.Concrete;
using Hangfire;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using WebUi.Data;
using WebUi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebUi.Hubs;
using WebUi.Infrastructure;

namespace WebUi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EfDbContext>(options =>
                options.UseSqlServer(Constants.DefaultConnection));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Constants.DefaultConnection));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddSigningCredential(Certificate.Get())
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication()
                .AddCookie("Cookies")
                .AddIdentityServerJwt();

            services.AddSignalR();
            //services.AddHangfire(x => x.UseMemoryStorage());
            services.AddHangfire(x => x.UseSqlServerStorage(Constants.DefaultConnection));
            services.AddHangfireServer();

            ConfigureDi(services);

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private void ConfigureDi(IServiceCollection services)
        {
            services.AddScoped<ISettingRepository, EfSettingRepository>();

            //hangfire
            //services.AddScoped<IHangfireBuildAccount, HangfireBuildAccount>();

            //email
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<EmailSettings>(Configuration);

            services.AddTransient<IProfileService, ProfileService>();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            ILogger<Startup> logger)
        {
            AddHangfire(app);
            //logger.LogTrace("1");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();

            try
            {
                app.UseIdentityServer();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }

            app.UseAuthorization();

            CreateRoles(serviceProvider).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TalkActive>("/hubs");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminRoleExists = await roleManager.RoleExistsAsync("admin");

            if (!adminRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            var userRoleExists = await roleManager.RoleExistsAsync("user");

            if (!userRoleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            var userToMakeAdmin = await userManager.FindByNameAsync("admin@admin.net");
            if (userToMakeAdmin == null)
            {
                var user = new ApplicationUser { UserName = "admin@admin.net", Email = "admin@admin.net", EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, "Z2030r###");
                if (!result.Succeeded) return;
                await userManager.AddToRoleAsync(user, "admin");
            }
        }

        private static void AddHangfire(IApplicationBuilder app)
        {
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Queues = new[] { "critical", "default" },
                ServerName = "Hangfire:1"
            });
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
#if !DEBUG
            //RecurringJob.AddOrUpdate<IHangfireAmericanHomesParse>(z => z.Run(), Cron.Minutely);
#endif
            //var jobId = BackgroundJob.Enqueue<IHangfireAmericanHomesParse>(z => z.Run());
            //RecurringJob.AddOrUpdate<IHangfireFacebookLeadSendSms>(z => z.Run(), "*/2 * * * *");
            //RecurringJob.AddOrUpdate<IHangfireFacebookLeadDisposition>(z => z.Run(), "*/5 * * * *");
            //RecurringJob.AddOrUpdate<IHangfireFacebookLeadAlarm>(z => z.Run(), Cron.Minutely);
            //RecurringJob.AddOrUpdate<IHangfireRemoveMmsImages>(z => z.Run(), Cron.Daily);
        }
    }
}
