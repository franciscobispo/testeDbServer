using AutoMapper;
using ContaCorrenteService.Interfaces;
using ContaCorrenteService.OpenApp;
using Domain.Interfaces.Generics;
using Domain.Interfaces.InterfaceAgencia;
using Domain.Interfaces.InterfaceContaCorrente;
using Domain.Interfaces.InterfaceContaCorrenteLancamentos;
using Domain.Interfaces.InterfaceUsuario;
using Infrastructure.Repository.Generics;
using Infrastructure.Repository.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Text;

namespace ContaCorrenteService
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
            services.TryAddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            services.AddHttpContextAccessor();

            services.AddSingleton(typeof(IGeneric<>), typeof(RepositoryGenerics<>));
            services.AddSingleton<IContaCorrente, RepositoryContaCorrente>();
            services.AddSingleton<IAgencia, RepositoryAgencia>();
            services.AddSingleton<IContaCorrenteLancamentos, RepositoryContaCorrenteLancamentos>();
            services.AddSingleton<IUsuario, RepositoryUsuario>();

            services.AddSingleton<InterfaceUsuarioApp, AppUsuario>();
            services.AddSingleton<InterfaceContaCorrenteLancamentosApp, AppContaCorrenteLancamentos>();
            services.AddSingleton<InterfaceContaCorrenteApp, AppContaCorrente>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            ConfigureSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Teste Francisco API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Teste Francisco API", Version = "v1" });
                x.AddSecurityDefinition("Bearer",
                                        new OpenApiSecurityScheme
                                        {
                                            In = ParameterLocation.Header,
                                            Description = "Para autenticação digite a palavra 'Bearer' seguido por espaço e o JWT Token",
                                            Name = "Authorization",
                                            Type = SecuritySchemeType.ApiKey
                                        });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                                                                            {
                                                                                new OpenApiSecurityScheme
                                                                                {
                                                                                Reference = new OpenApiReference
                                                                                    {
                                                                                    Type = ReferenceType.SecurityScheme,
                                                                                    Id = "Bearer"
                                                                                    },
                                                                                    Scheme = "oauth2",
                                                                                    Name = "Bearer",
                                                                                    In = ParameterLocation.Header,

                                                                                },
                                                                                new List<string>()
                                                                            }
                });
            });
        }

    }
}
