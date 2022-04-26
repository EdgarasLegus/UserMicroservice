using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SimpleInjector;
using System;
using System.IO;
using System.Reflection;
using UserApi.BusinessLogic.Communication;
using UserApi.BusinessLogic.EventServices;
using UserApi.BusinessLogic.LogicHelpers;
using UserApi.BusinessLogic.Services;
using UserApi.Data.DataAccess;

namespace UserApi
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
            var builder = new ContainerBuilder();

            services.AddRazorPages();
            services.AddDbContext<UserContext>(o => o.UseSqlServer(Configuration.GetConnectionString("MS_User")));

            services.AddAutoMapper(typeof(Startup));
            services.AddMvcCore().AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "User Api",
                    Description = "API to perform operations with users"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //var rabbitMq = RabbitHutch.CreateBus("host=localhost");

            services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IValidationHelper, ValidationHelper>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IMessageBusService, MessageBusService>()
                .AddScoped<IUserEventService, UserEventService>()
                .AddScoped<IAttributeExtractionHelper, AttributeExtractionHelper>()
                .AddSingleton(sp => RabbitHutch.CreateBus("host=localhost"));


            //builder.Populate(services);
            //var container = builder.Build();
            //var bus = container.Resolve<IBus>();

            //.Resolve<IBus>(() => RabbitHutch.CreateBus(""));
            //.AddSingleton<IBus>(RabbitHutch.CreateBus("host=localhost"));

            //return new AutofacServiceProvider(container);

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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
