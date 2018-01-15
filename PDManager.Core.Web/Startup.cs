using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDManager.Core.Aggregators;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Common.Testing;
using PDManager.Core.DSS;
using PDManager.Core.Services;
using PDManager.Core.Services.Testing;
using PDManager.Core.Web.Context;
using PDManager.Core.Web.Providers;
using Hangfire;
using Hangfire.MemoryStorage;
using PDManager.Core.Jobs;
using Microsoft.Extensions.Logging;


namespace PDManager.Core.Web
{
    /// <summary>
    /// Startup class of ASP Net Core site
    /// </summary>
    public class Startup
    {
      


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices.This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DSSContext>(options => options.UseInMemoryDatabase("dssdb"));
            // Add application services.
            services.AddTransient<IDSSRunner, DSSRunner>();

            //TODO: Replace with proper data proxy
            //services.AddTransient<IDataProxy, DataProxy>();
            services.AddTransient<IDataProxy, DummyDataProxy>();
            services.AddTransient<IAggregator, GenericAggregator>();
            services.AddTransient<IGenericLogger, LoggingProvider>();
            services.AddTransient<IAggrDefinitionProvider, AggrDefinitionProvider>();
            services.AddTransient<IDSSDefinitionProvider,DSSDefinitionProvider>();
            services.AddTransient<IAlertEvaluator, AlertEvaluator>();
            services.AddTransient<IPatientProvider, DummyPatientProvider>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ICommunicationParamProvider, CommunicationParamProvider>();
            services.AddTransient<IRecurringJob, AlertEvaluationJob>();
            services.AddTransient<IJobFactory, JobFactory>();
            services.AddTransient<IAlertInputProvider, AlertInputProvider>();

            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "PDManager DSS API", Version = "v1" });
            });

            services.AddHangfire(config =>
           config.UseMemoryStorage());


            //TODO: If Data PRoxy is used replace with propert Replace with proper data proxy
            // services.AddTransient<IProxyCredientialsProvider, DummyCredentialProvider>();
            services.AddMvc();
        }
        /// <summary>
        /// Configure
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>s

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
       
            app.UseStaticFiles();
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseSwagger();


         

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
