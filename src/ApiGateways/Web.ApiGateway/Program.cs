using FastEndpoints;
using FastEndpoints.Swagger;
using Core.Autofac;
using Core.AspNet.Extensions;
using Web.ApiGateway.Configurations;
using Web.ApiGateway.Extensions;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddFastEndpoints()
//    .AddSwaggerGen()
//    .SwaggerDocument();
//builder.AddServiceDefaults();
//builder.AddAutofac();
//builder.Services.AddAuthorization();
//builder.Services.AddGrpcServices();
//builder.Services.AddReverseProxy(builder.Configuration);
//builder.Services.Configure<UrlsConfiguration>(builder.Configuration.GetSection("urls"));

//var app = builder.Build();

//app.UseServiceDefaults();
//app.UseDefaultSwaggerRedirection();
//app.UseHttpsRedirection();

//app.UseExceptionHandler(opt => { });
//app.UseFastEndpoints(config => config.CommonResponseConfigs());

//app.UseSwagger();
//app.UseSwaggerUI(opt =>
//{
//    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Web ApiGateway");
//    opt.SwaggerEndpoint("/identity/swagger/v1/swagger.json", "Identity Api");
//    opt.SwaggerEndpoint("/employee/swagger/v1/swagger.json", "Employee Api");
//});

//app.MapGetSwaggerForYarp(app.Configuration);
//app.MapReverseProxy();

//await app.RunAsync();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
	opt.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme()
	{
		Type = SecuritySchemeType.OAuth2,
		Flows = new()
		{
			Implicit = new()
			{
				AuthorizationUrl = new Uri(builder.Configuration["Keycloak:AuthorizationUrl"]!),
				Scopes = new Dictionary<string, string>
					{
						{ "openid", "openid" },
						{ "profile", "profile" }
					}
			}
		}
	});

	var secRequirement = new OpenApiSecurityRequirement()
		{
			{
				new()
				{
					Reference = new()
					{
						Id = "Keycloak",
						Type = ReferenceType.SecurityScheme
					},
					In = ParameterLocation.Header,
					Name = "Bearer",
					Scheme = "Bearer"
				},
				[]
			}
		};
	opt.AddSecurityRequirement(secRequirement);
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.RequireHttpsMetadata = false;
		opt.Audience = builder.Configuration["Authentication:Audience"];
		opt.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
		opt.TokenValidationParameters = new()
		{
			ValidIssuer = builder.Configuration["Authentication:ValidateIssuer"]
		};
	});

var app = builder.Build(); 

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("users/me", (ClaimsPrincipal claimPrincipal) =>
{
	return claimPrincipal.Claims.ToDictionary(x => x.Type, x => x.Value);
}).RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();