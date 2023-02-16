using LojaJkMisterG.Context;
using LojaJkMisterG.Models;
using LojaJkMisterG.Repositories;
using LojaJkMisterG.Repositories.Interfaces;
using LojaJkMisterG.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaJkMisterG;

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
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>()
       .AddEntityFrameworkStores<AppDbContext>()
       .AddDefaultTokenProviders();

        #region política de senhas do identity
        // services.Configure<IdentityOptions>(options => {
        //    // Default Password settings.
        //    options.Password.RequireDigit = true;
        //    options.Password.RequireLowercase = true;
        //    options.Password.RequireNonAlphanumeric = true;
        //    options.Password.RequireUppercase = true;
        //    options.Password.RequiredLength = 8;
        //    options.Password.RequiredUniqueChars = 1;
        //});
        #endregion


        services.AddTransient<IRoupaRepository, RoupaRepository>();
        services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        services.AddTransient<IPedidoRepository, PedidoRepository>();
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin",
                politica =>
                {
                    politica.RequireRole("Admin");
                });
        });

        services.AddControllersWithViews();

        //Middlewares
        services.AddMemoryCache();
        services.AddSession();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, 
        IWebHostEnvironment env, ISeedUserRoleInitial seedUserRoleInitial)
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

        app.UseRouting();

        //Cria os perfis
        seedUserRoleInitial.SeedRoles();

        //Cria os usuários e atribui ao perfil
        seedUserRoleInitial.SeedUsers();


        app.UseSession();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                 name: "areas",
                 pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
             );

            endpoints.MapControllerRoute(
                name: "categoriaFiltro",
                pattern: "Roupa/{action}/{categoria?}",
                defaults: new { Controller = "Roupa", action = "List" });


            endpoints.MapControllerRoute(
             name: "default",
             pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}

