using Returns.Logic.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Environment, builder.Configuration);

var app = builder.Build();

await app.RunAsync();
