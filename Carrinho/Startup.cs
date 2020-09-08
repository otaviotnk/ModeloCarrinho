using Carrinho.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Carrinho
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<CarrinhoContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("CarrinhoContext")));

            services.AddScoped<PopularBanco>();
        }
        
        //Incluso o método para popular o Banco de Dados caso não esteja populado
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PopularBanco popularBanco)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                popularBanco.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Pedidos}/{action=Index}/{id?}");
            });
        }
    }
}
