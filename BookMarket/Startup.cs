using System.Text.Encodings.Web;
using System.Text.Unicode;
using BookMarket.Models.DataBase;
using BookMarket.Models.UsersIdentity;
using BookMarket.Services;
using BookMarket.Services.Books;
using BookMarket.Services.Profile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.WebEncoders;

namespace BookMarket
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
            services.AddControllersWithViews();
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            // �������� ������ ����������� �� ����� ������������
            string userConnect = Configuration.GetConnectionString("UsersIdentityConnection");

            // ���������� �������� ������ � �������������� Identity
            services.AddDbContext<UsersContext>(options =>
                options.UseSqlServer(userConnect));



            services.AddIdentity<User, IdentityRole>(i => 
            {
                i.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<UsersContext>();

            // �������� ������ ����������� �� ����� ������������
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BookMarketContext>(options =>
                options.UseSqlServer(connection), ServiceLifetime.Transient);

            // ���������� ������ �� ������ � �������
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IProfileService, ProfileService>();
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    // ����������� ��������������
            app.UseAuthorization();



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
