using AutoMapper;
using ISUCorp.API.Extensions;
using ISUCorp.Core.Domain;
using ISUCorp.Infra.Contexts;
using ISUCorp.Infra.Contracts;
using ISUCorp.Infra.Repositories;
using ISUCorp.Services.Contracts.Services;
using ISUCorp.Services.Mappers;
using ISUCorp.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ISUCorp.API
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
            // Add swagger for documentation
            services.AddCustomSwagger();

            // Add Cors
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => 
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });

            // Add DbContext using SQL Server Provider
            var dataConnectionString = Configuration["ConnectionStrings:Data"];
            services.AddDbContext<CoreDbContext>(options => options.UseSqlServer(dataConnectionString));

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new ProfileMapper()); });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // add DI rules
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IUtilService, UtilService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // generic repository dependencies
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IAsyncRepository<Reservation>, EfRepository<Reservation, CoreDbContext>>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ISUCorpApp/dist";
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseCustomSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "ISUCorpApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
