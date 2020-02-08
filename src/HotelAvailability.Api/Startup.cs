/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Application startup
/// </summary>

namespace HotelAvailability.Api
{
    #region import namespaces

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.Swagger;
    using HotelAvailability.Api.Helpers;
    using HotelAvailability.Core.Repositories;
    using HotelAvailability.Core.Repositories.Interfaces;
    using System.Diagnostics;

    #endregion

    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
            .SetBasePath(hostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add cors
            services.AddCors();

            // Add framework services.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // The port to use for https redirection in production
            if (!_hostingEnvironment.IsDevelopment() && !string.IsNullOrWhiteSpace(Configuration["HttpsRedirectionPort"]))
            {
                services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = int.Parse(Configuration["HttpsRedirectionPort"]);
                });
            }

            // Add swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Hotel Availibality Api", Version = "v1" });
            });
            
            // Add repositories
            services.AddTransient<IHotelAvailability>(c => new HotelAvailability(Configuration["ApiConfig:BargainsApiUrl"], Configuration["ApiConfig:BargainsApiAuthenticationCode"]));

            // Add inmemory cache
            services.AddMemoryCache();

            // Add options
            services.AddOptions();

            // Inject configuration
            services.Configure<CacheConfig>(Configuration.GetSection("CacheConfig"));
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            Utilities.ConfigureLogger(loggerFactory);

            if (hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Check api response time
            //app.Use(async (context, next) =>
            //{
            //    var watch = new Stopwatch();
            //    watch.Start();
            //    await next();
            //    watch.Stop();
            //    context.Response.Headers.Add("ResponseTime", watch.Elapsed.Seconds.ToString());
            //});

            //Configure Cors
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelAvailability.Api V1");
            });
            
            app.UseMvc();
        }
    }
}
