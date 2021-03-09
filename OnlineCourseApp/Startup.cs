using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.DataRepository.IDataRepository;
using OnlineCourseApp.Data.DataRepository.OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.EF;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.SignalR.Hubs;

namespace OnlineCourseApp
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
            services.AddDbContext<MyDBContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("connection")));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<MyDBContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
           .AddGoogle(opts =>
           {
               opts.ClientId = "875597490713-j0ev8lha75qnolquq3l915apr0ra3bc3.apps.googleusercontent.com";
               opts.ClientSecret = "La5sv0xKfyuUvCwmolHTuRCc";
               opts.SignInScheme = IdentityConstants.ExternalScheme;
           });

            services.AddDistributedMemoryCache(); 
            services.AddSession();
            services.AddSignalR();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
            });

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));   
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseTypeRepository, CourseTypeRepository>();
            services.AddScoped<ICourseTypeRepository2, CourseTypeRepository>();
            services.AddScoped<ICourseSectionRepository, CourseSectionRepository>();
            services.AddScoped<ICourseSectionRepository2, CourseSectionRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ICourseParticipantRepository, CourseParticipantRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDocumentShareRepository, DocumentShareRepository>();
            services.AddScoped<IExamRepository, ExamRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionCategoryRepository, QuestionCategoryRepository>();
            services.AddScoped<IChoiceRepository, ChoiceRepository>();
            services.AddScoped<IExamAnsweredQuestionRepository, ExamAnsweredQuestionRepository>();
            services.AddControllersWithViews();
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");
               endpoints.MapHub<NotificationHub>("/notificationHub");
            });
        }
    }
}
