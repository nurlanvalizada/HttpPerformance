using System;
using System.Net.Http;
using HttpPerformance.Middlewares;
using HttpPerformance.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace HttpPerformance
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
            services.AddSingleton<HttpClient>();
            services.AddHttpClient();
            services.AddHttpClient("github", client =>
            {
                client.BaseAddress = new Uri("https://api.github.com/");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.nightshade-preview+json");
                client.DefaultRequestHeaders.Add("User-Agent", "my-agent");
            });
            services.AddHttpClient<IGithubClient, GithubClient>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(3, retryCount =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryCount))));
                //.AddHttpMessageHandler();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ResponseTimeMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
