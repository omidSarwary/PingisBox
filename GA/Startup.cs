using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA.Helper;
using GA.Hubs;
using GA.Models;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public TimeSpan HandshakeTimeout { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            services.AddDbContext<AppDbContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(
            //    options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //}
            )
            .AddCookie()
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://oec.com",
                    ValidIssuer = "http://oec.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1s5da4sd213&*^*sadjkhskjshksfd5s4fsd42^&%s7dasdfgsdfsdf4sd8f4sd8f465d%^^%$DS&"))
                };
            });
           
            services.AddTransient<IStudentRepository,StudentRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IBoxRepository, BoxRepository>();
            services.AddTransient<IMail, Mail>();
            services.AddTransient<INotificationsRepository , NotificationsRepository>();
            services.AddTransient<IChangeNotifierRepository, ChangeNotifierRepository>();
            services.AddSignalR(
                hubOptions =>
                {
                    hubOptions.EnableDetailedErrors = true;
                    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
                    HandshakeTimeout = TimeSpan.FromMinutes(1);
                });


            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "CookieName";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.SlidingExpiration = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseHangfireDashboard("/BackgroundTasks", new DashboardOptions
            {
                AppPath = "/HJKSAHadwadw",               
          
                Authorization = new[] { new HangfireAuthoriztionFilter() }
            });
            app.UseSignalR((configure) =>
            {
                var desiredTransports =
                    HttpTransportType.WebSockets |
                    HttpTransportType.LongPolling;

                configure.MapHub<BoxHub>("/BoxHub", (options) =>
                {
                    options.Transports = desiredTransports;
                });
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
