using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Web.Api.Core;
using Web.Api.Core.Interfaces;
using Web.Api.Settings;

namespace Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {

            services.AddOptions();

            IConfigurationSection sec = Configuration.GetSection("Endpoints");
            services.Configure<Endpoints>(sec);

            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton<IAlbumClient, AlbumClient>();
            services.AddSingleton<IPhotoClient, PhotoClient>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<ICachePhotoClient, CachePhotoClient>();



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            ServiceProvider sp = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
