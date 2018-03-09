﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CWPIO.Data;
using CWPIO.Models;
using CWPIO.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Slack.Webhooks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace CWPIO
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
            var supportedCultures = new[]
            {
                  new CultureInfo("en-US")
                , new CultureInfo("ru-RU")
                , new CultureInfo("zh-CN")
                //, new CultureInfo("ja-JP")
            };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                // Formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                options.SupportedUICultures = supportedCultures;

                var remove = options.RequestCultureProviders.OfType<AcceptLanguageHeaderRequestCultureProvider>().FirstOrDefault();
                if (remove != null)
                    options.RequestCultureProviders.Remove(remove);

                // Find the cookie provider with LINQ
                var cookieProvider = options.RequestCultureProviders.OfType<CookieRequestCultureProvider>().FirstOrDefault();
                if (cookieProvider != null)
                    // Set the new cookie name
                    options.RequestCultureProviders.Remove(cookieProvider);
                //cookieProvider.CookieName = "CWP.UserCulture";

                //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //    //Get culture from DB
                //    return new ProviderCultureResult("en");
                //}));
            });

            services
                .AddLocalization(options => options.ResourcesPath = "Resources")
                .AddEntityFrameworkNpgsql()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("CWPConnection")), ServiceLifetime.Singleton, ServiceLifetime.Singleton)
                .AddAuthorization(options =>
                    {
                        options.AddPolicy(
                            "CanAccessUsers",
                            policyBuilder => policyBuilder.RequireAuthenticatedUser().RequireClaim("IsAdmin", "True"));

                        options.AddPolicy(
                            "CanEditUsers",
                            policyBuilder => policyBuilder.RequireAuthenticatedUser().RequireClaim("IsAdmin", "True"));

                        options.AddPolicy(
                            "CanAccessBounty",
                            policyBuilder => policyBuilder
                                .RequireAuthenticatedUser()
                                .RequireClaim("IsExtendedProfileCompleted", "True")
                                .RequireClaim("IsEmailCompleted", "True"));
                    })
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
                        options.Password.RequireUppercase = true;
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

            services.AddMvc()
                .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddViewLocalization(); //IViewLocalizer


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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseRequestLocalization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
