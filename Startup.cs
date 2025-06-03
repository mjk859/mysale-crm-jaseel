using DocumentFormat.OpenXml.Bibliography;
using Irony.Parsing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MySaleApp.Admin.UI.Context;
using MySaleApp.Admin.UI.Services;
using System.Net;
using static System.Net.WebRequestMethods;

namespace MySaleApp.Admin.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MySaleAppContext>(options =>
              options.UseSqlServer(Configuration["Data:MySaleAppConnection:ConnectionString"]));

            // Configure Data Protection to persist keys (CRITICAL for production)
            var dataProtectionBuilder = services.AddDataProtection()
                .SetApplicationName("MySaleApp"); // Must be same across all instances

            if (Environment.IsDevelopment())
            {
                // For development - keys in local folder
                var keysPath = Path.Combine(Environment.ContentRootPath, "App_Data", "DataProtectionKeys");
                Directory.CreateDirectory(keysPath);
                dataProtectionBuilder.PersistKeysToFileSystem(new DirectoryInfo(keysPath));
            }
            else
            {
                // Create directory if it doesn't exist
                var keysPath = "/tmp/dataprotection-keys";
                if (!Directory.Exists(keysPath))
                {
                    Directory.CreateDirectory(keysPath);
                }

                // App Engine doesn't support DPAPI, use different approach
                dataProtectionBuilder
                    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
                    .SetDefaultKeyLifetime(TimeSpan.FromDays(90)); // Keys last 90 days
            }


            // Add authorization services
            services.AddAuthorization();
            

            var redisHost = Configuration["RedisConnection:Host"];
            var redisPort = Configuration["RedisConnection:Port"];
            var redisUser = Configuration["RedisConnection:User"];
            var redisPassword = Configuration["RedisConnection:Password"];

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                {
                    EndPoints = { $"{redisHost}:{redisPort}" },
                    Password = redisPassword,
                    Ssl = false, // Disable TLS as not available in free plan
                    AbortOnConnectFail = false,
                    ConnectRetry = 5,
                    ConnectTimeout = 5000,
                };
                options.InstanceName = "MySaleAppSession:";

            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(8);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddSingleton<ITicketStore, RedisTicketStore>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/Login/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.Cookie.MaxAge = TimeSpan.FromHours(8);
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.SlidingExpiration = false;


                });

            services.PostConfigure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                var provider = services.BuildServiceProvider(); // Only once
                var ticketStore = provider.GetRequiredService<ITicketStore>();
                options.SessionStore = ticketStore;
            });

            services.AddControllersWithViews();

            

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Handle App Engine's load balancer headers
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                                 Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
            }


            app.UseStaticFiles();
            app.UseRouting();

            // Add session middleware if you added session services above
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePages(context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == 401)
                {
                    response.Redirect("/Login");
                }
                else if (response.StatusCode == 403)
                {
                    response.Redirect("/Login/AccessDenied");
                }
                return Task.CompletedTask;
            });



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}