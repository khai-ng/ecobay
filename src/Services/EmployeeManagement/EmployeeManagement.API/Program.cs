using FastEndpoints.Swagger;
using FastEndpoints;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Domain.Constants;
using EmployeeManagement.API.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using EmployeeManagement.Infrastructure;
using Infrastructure.Kernel.Dependency;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerGen();
builder.Services.SwaggerDocument();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection(AppEnvironment.JWT_SECTION).Get<JwtOption>()!;
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = jwtOptions.Audience,
        ValidIssuer = jwtOptions.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
    };
});
builder.Services.AddAuthorization();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b => b.AutofacRegister());

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContextPool<AppDbContext>((service, opt) =>
{
    var connection = builder.Configuration.GetConnectionString(AppEnvironment.DB_SCHEMA);
    opt.UseSqlServer(connection, option => option.CommandTimeout(100));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.UseDefaultExceptionHandler();
app.UseFastEndpoints();
app.UseSwaggerGen();

if (app.Environment.IsDevelopment())
{
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}
app.Run();