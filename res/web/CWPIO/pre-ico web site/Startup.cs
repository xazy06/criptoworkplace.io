using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using pre_ico_web_site.Data;
using pre_ico_web_site.Eth;
using pre_ico_web_site.Models;
using pre_ico_web_site.Services;
using Slack.Webhooks;
using System;
using System.Security.Cryptography.X509Certificates;

namespace pre_ico_web_site
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLocalization(options => options.ResourcesPath = "Resources")
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("CWPConnection")), ServiceLifetime.Singleton, ServiceLifetime.Singleton)
                .AddAuthorization()
                .AddTransient<IEmailSender, EmailSender>()
                .AddSingleton<ISlackClient, SlackClient>(config =>
                {
                    var conf = config.GetService<IOptions<SlackSettings>>();
                    return new SlackClient(conf.Value.SubscribeChannelUrl);
                })
                .Configure<MailSettings>(Configuration.GetSection("MailSettings"))
                .Configure<SlackSettings>(Configuration.GetSection("SlackWebHooks"))
                .Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredUniqueChars = 6;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    options.User.RequireUniqueEmail = true;

                    // Confirmations
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                });

            services.AddMvc();

            services.AddDataProtection()
                .PersistKeysToSql()
                .SetDefaultKeyLifetime(TimeSpan.FromDays(7))
                .SetApplicationName("CWPIO");

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = Configuration["Authentication:Facebook:AppId"];
                    options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                })
                .AddTwitter(options =>
                {
                    options.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                    options.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                });

            services.Configure<EthSettings>(Configuration.GetSection("Ether"));

            services.AddSingleton(s =>
            {
                var settings = s.GetService<IOptions<EthSettings>>().Value;
                var password = settings.AppPrivateKey;

                var account = new Account(settings.AppPrivateKey);
                return new Web3(account, settings.NodeUrl ?? "http://localhost:8545");
            });

            services.AddSingleton<TokenSaleContract>();

            services.Configure<RateStoreSettings>(Configuration.GetSection("RateStore"));
            services.AddSingleton<IRateStore, RateStore>();

            services.AddMemoryCache();

            services.Configure<GoogleDriveSettings>(Configuration.GetSection("GDrive"));

            services.AddSingleton(s =>
            {
                var settings = s.GetRequiredService<IOptions<GoogleDriveSettings>>().Value;

                var serviceAccountEmail = settings.ServiceAccount;
                var certificate = new X509Certificate2(settings.P12CertPath, "notasecret", X509KeyStorageFlags.Exportable);

                ServiceAccountCredential credential = new ServiceAccountCredential(
                   new ServiceAccountCredential.Initializer(serviceAccountEmail)
                   {
                       Scopes = new[] { DriveService.Scope.Drive }
                   }.FromCertificate(certificate));

                return new DriveService(new BaseClientService.Initializer
                {
                    ApplicationName = "Discovery Sample",
                    HttpClientInitializer = credential
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestLocalization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            if (env.IsProduction())
            {
                var forwardingOptions = new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.All
                };
                forwardingOptions.KnownNetworks.Clear(); //its loopback by default
                forwardingOptions.KnownProxies.Clear();
                app.UseForwardedHeaders(forwardingOptions);

                app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}