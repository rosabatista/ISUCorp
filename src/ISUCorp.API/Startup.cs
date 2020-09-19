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
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // generic repository dependencies
            services.AddScoped<IAsyncRepository<Contact>, EfRepository<Contact, CoreDbContext>>();
            services.AddScoped<IAsyncRepository<Place>, EfRepository<Place, CoreDbContext>>();
            services.AddScoped<IAsyncRepository<Reservation>, EfRepository<Reservation, CoreDbContext>>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
