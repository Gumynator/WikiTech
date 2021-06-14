using WikiTechAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WikiTechAPI.Middleware;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.IO;
using WikiTechAPI.Utility;
using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace WikiTechAPI
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
            services.AddDbContext<WikiTechDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddLogging(loggingBuilder => {
                loggingBuilder.AddFile("./Logs/app.log", append: true);
            });

            //DinkToPdf
            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var wkHtmlToPdfPath = Path.Combine(Directory.GetCurrentDirectory(), $"libs\\{architectureFolder}\\libwkhtmltox.dylib");
                context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var wkHtmlToPdfPath = Path.Combine(Directory.GetCurrentDirectory(), $"libs\\{architectureFolder}\\libwkhtmltox.so");
                context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
            }
            else // RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            {
                var wkHtmlToPdfPath = Path.Combine(Directory.GetCurrentDirectory(), $"libs\\{architectureFolder}\\libwkhtmltox.dll");
                context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
            }
            //var context = new CustomAssemblyLoadContext();
            //context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            //AddNewtonsoftJson, use to ingore max JSON transaction
            services.AddControllers().AddNewtonsoftJson(p => p.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddCors(option =>
            {
                option.AddPolicy("AllowOrigin", options =>
                {
                    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseStaticFiles();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}