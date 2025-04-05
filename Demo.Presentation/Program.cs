using Demo.BusinessLogic.Services;
using Demo.DataAccess.Data.Context;
using Demo.DataAccess.Models;
using Demo.DataAccess.Repositories;
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

            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            //builder.Services.AddAutoMapper(x=>x.AddProfile(new EmployeeProfile));
            builder.Services.AddAutoMapper(typeof(Demo.BusinessLogic.AssemblyRefernce).Assembly);

            //builder.Services.AddScoped<IGenericRepository<Department>,GenericRepository<Department>>();
            //builder.Services.AddScoped<IGenericRepository<Employee>, GenericRepository<Employee>>();
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(IGenericRepository<>));


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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            #endregion

            app.Run();
        }
    }
}
