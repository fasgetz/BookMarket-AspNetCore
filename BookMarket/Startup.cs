using System;
using System.IO.Compression;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using BookMarket.Models.DataBase;
using BookMarket.Models.UsersIdentity;
using BookMarket.Services;
using BookMarket.Services.Books;
using BookMarket.Services.Profile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;

namespace BookMarket
{
    //
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
            services.AddControllersWithViews();
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            // получаем строку подключения из файла конфигурации
            string userConnect = Configuration.GetConnectionString("UsersIdentityConnection");

            // Подключаем контекст работы с пользователями Identity
            services.AddDbContext<UsersContext>(options =>
                options.UseSqlServer(userConnect));



            services.AddIdentity<User, IdentityRole>(i => 
            {
                i.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<UsersContext>();

            // получаем строку подключения из файла конфигурации
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BookMarketContext>(options =>
                options.UseSqlServer(connection), ServiceLifetime.Transient);

            // Подключаем сервис по работе с логикой
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IProfileService, ProfileService>();

            // добавляем сервис компрессии
            services.AddResponseCompression(options => options.EnableForHttps = true);

            // добавление кэширования
            services.AddMemoryCache();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".MyApp.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.IsEssential = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse =
                    r =>
                    {
                        string path = r.File.PhysicalPath;
                        if (path.EndsWith(".css") || path.EndsWith(".js") || path.EndsWith(".gif") || path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".svg"))
                        {
                            TimeSpan maxAge = new TimeSpan(7, 0, 0, 0);
                            r.Context.Response.Headers.Append("Cache-Control", "max-age=" + maxAge.TotalSeconds.ToString("0"));
                        }
                    }
            });

            app.UseRouting();

            app.UseAuthentication();    // подключение аутентификации
            app.UseAuthorization();

            app.UseSession();   // добавляем механизм работы с сессиями

            // подключаем компрессию
            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "aboutBook",
                    pattern: "book/{id}/{name}",
                    defaults: new { controller = "Books", action = "AboutBook" }
                );


                endpoints.MapControllerRoute(
                    name: "BookRoute",
                    pattern: "book/ReadBook/{idBook}/",
                    defaults: new { controller = "Books", action = "GetBook"}
                );

                endpoints.MapControllerRoute(
                    name: "AuthorsRoute",
                    pattern: "Author/{id}/{name}",
                    defaults: new { controller = "Authors", action = "Details" }
                );

                endpoints.MapControllerRoute(
                    name: "Genres",
                    pattern: "SearchBook/{name}",
                    defaults: new { controller = "SearchBook", action = "Index" }
                );



                endpoints.MapControllerRoute(
                    name: "profileMap",
                    pattern: "profile/{name}",
                    defaults: new { controller = "Profile", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");





            });
        }
    }
}
