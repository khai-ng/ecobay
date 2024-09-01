using Core.AspNet.Extensions;
using Core.Autofac;

var builder = WebApplication.CreateBuilder(args);

builder.AddAutofac();
builder.AddServiceDefaults();

var app = builder.Build();

await app.RunAsync();