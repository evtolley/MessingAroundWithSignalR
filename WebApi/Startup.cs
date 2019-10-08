using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SignalRTestz
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
            services.AddCors(options => options.AddPolicy("CorsPolicy", 
                p => p
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200", "http://localhost:4201")
                ));

            // requires using Microsoft.Extensions.Options
            services.Configure<MessageDatabaseSettings>(
                Configuration.GetSection(nameof(MessageDatabaseSettings)));

            services.AddSingleton<IMessageDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<MessageDatabaseSettings>>().Value);

            services.AddSingleton<IMessageRepository, MessageRepository>();


            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotifyHub>("/notify");
                routes.MapHub<UpdateHub>("/updatehub");
                routes.MapHub<DeleteHub>("/deletehub");
            });
            app.UseMvc();
        }
    }
}
