using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Blazui.Docs.Admin.Data;
using BlazAdmin.ServerRender;
using Blazui.Docs.Admin.EFCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Blazui.Docs.Admin.Service;
using Blazui.Docs.Admin.Repository;
using Blazui.Docs.Admin.Repository.EFCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Blazui.Docs.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DataBaseConnectionString")))
            {
                return;
            }
            services.AddDbContext<DocsDbContext>(options =>
            {
                options.UseNpgsql(Environment.GetEnvironmentVariable("DataBaseConnectionString"));
            });
            services.AddBlazAdmin<DocsDbContext>();
            services.AddScoped<ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IQuickStartStepRepository, QuickStartStepRepository>();
            services.AddScoped<IComponentRepository, ComponentRepository>();
            services.AddScoped<IExportedTypeRepository, ExportedTypeRepository>();
            services.AddScoped<IProductVersionRepository, ProductVersionRepository>();
            services.AddScoped<ComponentService>();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DocsDbContext>().Database.Migrate();
        }
    }
}
