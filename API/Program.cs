using Microsoft.EntityFrameworkCore;
using DataBase.Context;
using DataBase.Repositories;
using DataBase.Repositories;
using Core.Services;
using DataBase.Repositories;
using API;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

Startup.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

Startup.Configure(app, builder.Environment);

app.Run();
