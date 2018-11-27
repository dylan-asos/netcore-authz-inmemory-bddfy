using System;
using Asos.DotNetCore.Auth.Api.Demo.Orders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Asos.DotNetCore.Auth.Api.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AuthenticationSetup.Initialize(services, Configuration);

            services.AddHttpClient<IOrderRetriever, OrderRetriever>(client => client.BaseAddress = new Uri("https://orders-api.com/"));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}