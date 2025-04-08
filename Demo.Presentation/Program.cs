using Demo.BusinessLogic.Services;
using Demo.BusinessLogic.Services.AttachmentService;
using Demo.DataAccess.Data.Context;
using Demo.DataAccess.Models;
using Demo.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<Func<IDepartmentRepository>>(provider =>()=>provider.GetRequiredService<IDepartmentRepository>());
            builder.Services.AddScoped<Func<IEmployeeRepository>>(provider  => provider.GetRequiredService<IEmployeeRepository>);

            builder.Services.AddTransient<IAttachmentService,AttachmentService>();




            //builder.Services.AddAutoMapper(x=>x.AddProfile(new EmployeeProfile));
            builder.Services.AddAutoMapper(typeof(Demo.BusinessLogic.AssemblyRefernce).Assembly);

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
            //    options.User.RequireUniqueEmail = true;
            //    options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();


            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "Account/Login";
            //});

            #endregion

            var app = builder.Build();

            #region Configure the HTTP request pipeline.

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            #endregion

            app.Run();
        }
    }
}
