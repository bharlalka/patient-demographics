using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PatientDemographics.Data;
using PatientDemographics.Data.Entities;
using PatientDemographics.ViewModels;

namespace PatientDemographics
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<PatientContext>(cfg => {
                //cfg.UseInMemoryDatabase("PatientsDb");
                cfg.UseSqlServer(Configuration.GetConnectionString("PatientConnectionString"));
            });

            services.AddAutoMapper();
            services.AddScoped<IPatientRepository, PatientRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<PatientContext>();
                context.Database.Migrate();
            }

            AutoMapper.Mapper.Initialize(mapper =>
                mapper.CreateMap<Patient, PatientViewModel>().ReverseMap()
            );

            app.UseMvc();
        }
    }
}
