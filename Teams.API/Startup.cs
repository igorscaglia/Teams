using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Teams.API.Persistence;

namespace Teams.API
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
            services.AddControllers().AddNewtonsoftJson();

            // Vamos deixar o repositório em memória com uma única instância
            services.AddSingleton<ITeamRepository>(new MemoryTeamRepository());

            // Adicionar o AutoMapper para mapear nosso modelos do domínio em DTOs
            services.AddAutoMapper(typeof(MemoryTeamRepository).Assembly);

            // Vamos deixar as urls geradas em caixa baixa
            services.AddRouting(opt => opt.LowercaseUrls = true);

            // Adicionar CORS para não ter problema principalmente nos testes
            services.AddCors();

            // Vamos registrar o gerador de UI do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Teams API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            void ConfigureExceptionHandling()
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    // Se estiver em produção ou staging vamos deixar a mensagem de erro voltar no header e no body
                    app.UseExceptionHandler(builder =>
                    {
                        builder.Run(async httpContext =>
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                            var error = httpContext.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                httpContext.Response.Headers.Add("Application-Error", error.Error.Message);
                                httpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
                                
                                // O CORS tem que ser configurado aqui manualmente pois o pipeline é diferente do main (do middleware)
                                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                                
                                // Escreve o erro no response
                                await httpContext.Response.WriteAsync(error.Error.Message);
                            }
                        });
                    });
                }
            }

            ConfigureExceptionHandling();

            // Configurar os dados para o ambiente de desenvolvimento
            void ConfigureSeeding()
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    try
                    {
                        if (env.IsDevelopment())
                        {
                            logger.LogInformation("Em Development mode. Semeando banco de dados...");

                            var repository = scope.ServiceProvider.GetRequiredService<ITeamRepository>();

                            Setup.SeedDatabase.SeedDomain(repository);
                        }
                        else
                        {
                            logger.LogInformation("Aplicação não está em Development mode");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        logger.LogError(ex, "Erro ao semear o banco de dados");
                    }
                }
            }

            ConfigureSeeding();

             // Gerar os endpoints do Swagger em JSON
            app.UseSwagger();

            // Habilitar o Swagger UI (HTML, JS, CSS, etc.), especificando o JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Teams API V1");

                // Fazer Swagger UI abrir na raiz (http://localhost:<port>/)
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            // Adicionar o CORS
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
