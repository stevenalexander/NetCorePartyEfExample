﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PartyData;
using PartyData.Data;
using PartyData.Repositories;

namespace WebApplicationParty
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var partyDbContext = (new PartyDbContextFactory()).Create(Configuration.GetConnectionString("DefaultConnection"));

            services.AddSingleton<IPartyRespository>(new PartyRespository(partyDbContext));
            services.AddSingleton<IPagedSortedRepository<PartyWithRegistrationsResultItem>>(new PartyWithRegistrationsSortedRepository(partyDbContext));
            services.AddSingleton<IPagedSortedRepository<OrganisationResultItem>>(new OrganisationPagedSortedRepository(partyDbContext));
            services.AddSingleton<IPagedSortedRepository<PersonResultItem>>(new PersonPagedSortedRepository(partyDbContext));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
