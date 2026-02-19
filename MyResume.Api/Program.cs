using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyResume.Api.Manager;
using MyResume.Api.Repo.Db.Context;
using MyResume.Api.Repo.Repo;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddDbContext<MyResumeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyResumeDbConnection")));

builder.Services.AddScoped<ILogActivityManager, LogActivityManager>();
builder.Services.AddScoped<IChatDataManager, ChatDataManager>();

builder.Services.AddScoped<ILogActivityRepo, LogActivityRepo>();
builder.Services.AddScoped<IChatDataRepo, ChatDataRepo>();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
