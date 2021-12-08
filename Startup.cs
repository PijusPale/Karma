using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Karma.Aspects;
using Karma.Helpers;
using Karma.Repositories;
using Karma.Services;
using Karma.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Karma.Database;

namespace Karma
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddControllersWithViews()
                .AddJsonOptions(opts => {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });

            var jwtSettingsSection = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if(!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/ChatHub")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSignalR();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            /* builder.RegisterGeneric(typeof(Lazy<>))
                .As(typeof(LazyInstance<>))
                .InstancePerLifetimeScope(); */

            var options = new DbContextOptionsBuilder<BaseDbContext>()
                .UseSqlite(Configuration.GetConnectionString("DefaultConnection")).Options;

            builder.RegisterType<BaseDbContext>()
                .WithParameter("options", options)
                .InstancePerLifetimeScope();

            builder.RegisterType<DbListingRepository>()
                .As<IListingRepository>()
                .InstancePerDependency();

            builder.RegisterType<DbUserRepository>()
                .As<IUserRepository>()
                .InstancePerDependency();

            builder.Register(m => new MessageRepository("temp"))
                .As<IMessageRepository>()
                .InstancePerDependency();
            
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(MethodInterceptor))
                .InstancePerDependency();

            builder.Register(m => new MessageService(new MessageRepository("temp")))
                .As<IMessageService>()
                .InstancePerDependency();

            builder.Register(l => new ListingNotification(Path.Combine("data", "NotificationData.json")))
                .As<IListingNotification>()
                .SingleInstance();
            
            builder.RegisterType<IdBasedUserIdProvider>()
                .As<IUserIdProvider>()
                .SingleInstance();

            builder.RegisterType<MethodInterceptor>()
                .SingleInstance();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            /* app.UseMiddleware<StatisticsMiddleware>(); */
 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/ChatHub");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
