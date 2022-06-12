using AdminPortal.API.Entities;
using AdminPortal.API.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AdminPortal.API
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
            services.AddCors((options) => 
            {
                options.AddPolicy("angularApplication", (builder) =>
                 {
                     builder.WithOrigins("http://localhost:4200")
                     .AllowAnyHeader()
                     .WithMethods("GET", "POST", "PUT", "DELETE")
                     .WithExposedHeaders("*");

                 });
            });


            services.AddDbContext<AdminContext>(options => options
            .UseSqlServer(Configuration.GetConnectionString("AdminPortalDb")));
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IImageRepository, LocalStorageImageRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AdminPortal.API", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminPortal.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("angularApplication");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
