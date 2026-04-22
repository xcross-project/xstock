using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using xstock.lib.schema.dst;

namespace xstock.lib.comp.webapi
{

    internal partial class webapiserv
    {
        private void __constructor_webapiserv()
        {
            __status = false;

        }

        private void __start()
        {
            var __builder = WebApplication.CreateBuilder(options: new WebApplicationOptions()
            {
                Args = new string[] {
                    "--urls", xstockwebapi.settings.bindurls,
                }
            });

            if (!xstockwebapi.settings.openapi.showlogging)
                __builder.Logging.ClearProviders();

            __builder.Services.AddControllers();

            __builder.Services.AddMvc().AddApplicationPart(typeof(stockcore).Assembly);
            __builder.Services.AddEndpointsApiExplorer();

            if (xstockwebapi.settings.openapi.swaggerswitch)
            {

                __builder.Services.AddSwaggerGen(__sgoptions => {
                    if (null != xstockwebapi.settings.openapi.documents)
                        foreach (var __swaggerdoc in xstockwebapi.settings.openapi.documents)
                            __sgoptions.SwaggerDoc(__swaggerdoc.name, new Microsoft.OpenApi.Models.OpenApiInfo()
                            {
                                Version = __swaggerdoc.version,
                                Title = __swaggerdoc.title,
                                Description = __swaggerdoc.description,
                                TermsOfService = new Uri(__swaggerdoc.termsofservice)
                            });
                    if (File.Exists(xstockwebapi.settings.openapi.injectionxmlcommentsfile))
                        __sgoptions.IncludeXmlComments(xstockwebapi.settings.openapi.injectionxmlcommentsfile);
                    if (null != xstockwebapi.settings.openapi.securitydefinitions)
                        foreach (var __secdef in xstockwebapi.settings.openapi.securitydefinitions)
                            __sgoptions.AddSecurityDefinition(__secdef.key,
                                new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                                {
                                    Description = __secdef.description,
                                    Name = __secdef.name,
                                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                                    BearerFormat = __secdef.bearerformat,
                                    Scheme = __secdef.scheme
                                });
                    if (null != xstockwebapi.settings.openapi.securitydefinitions &&
                        xstockwebapi.settings.openapi.securitydefinitions.Length > 0x00)
                        __sgoptions.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
                            {
                                new Microsoft.OpenApi.Models.OpenApiSecurityScheme(){
                                    Reference = new Microsoft.OpenApi.Models.OpenApiReference(){
                                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                        Id = xstockwebapi.settings.openapi.securitydefinitions[0x00].key
                                    }
                                }, new string[]{ }
                            }
                        });
                });
            }

            __builder.Services.AddAuthentication(__auth => {
                __auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                __auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(__bearer => {
                __bearer.RequireHttpsMetadata = false;
                __bearer.SaveToken = true;
                __bearer.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(xstockwebapi.settings.cors[0x00].secretkey)),
                    ValidateIssuer = true,
                    ValidIssuer = xstockwebapi.settings.cors[0x00].tokenissuer,
                    ValidateAudience = true,
                    ValidAudience = xstockwebapi.settings.cors[0x00].tokenaudience,
                    ValidateLifetime = true
                };
            });

            __builder.Services.AddCors(__options => {
                foreach (var __cors in xstockwebapi.settings.cors)
                    __options.AddPolicy(__cors.policy, __policy =>
                        __policy.WithOrigins(__cors.trustorigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                    );
            });

            #region middlewares

            __builder.Services.AddTransient<middlewares.detectmiddleware>();

            #endregion

            __webapicore = __builder.Build();

            if (xstockwebapi.settings.staticenable)
                __webapicore.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider =
                        new Microsoft.Extensions.FileProviders
                        .PhysicalFileProvider(
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xstockwebapi.settings.staticroute)),
                    RequestPath = $"/{xstockwebapi.settings.staticroute}"
                });

            if (xstockwebapi.settings.openapi.swaggerswitch)
            {
                __webapicore.UseSwagger();
                __webapicore.UseSwaggerUI();
            }
            __webapicore.UseDeveloperExceptionPage();

            __webapicore.UseAuthentication();
            __webapicore.UseAuthorization();

            #region middlewares

            __webapicore.UseMiddleware<middlewares.detectmiddleware>();

            #endregion

            __webapicore.MapControllers();
            __webapicore.RunAsync();

            __status = true;
        }

        private async void __stop()
        {
            if (__status && null != __webapicore)
            {
                await __webapicore.StopAsync();
                __status = false;
            }
        }
    }
}
