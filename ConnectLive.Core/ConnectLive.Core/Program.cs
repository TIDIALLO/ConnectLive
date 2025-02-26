using ConnectLive.Application;
using ConnectLive.Core.Api.Configurations;
using ConntectLive.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.PostgreSql;
using Workers;
using ConnectLive.Core.Api.Filters;
using Connectlive.Proxy;
using ConnectLive.Core.Api.Extensions;
using ConnectLive.Core.Api;

//var builder = WebApplication.CreateBuilder(args);
var builder = HostExtensions.CreateWebHostBuilder<ClassInfo>(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectLiveContext"));
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailWorker, EmailWorker>();
builder.Services.AddScoped<IProxy, Proxy>();

builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddBusPublisherRegistration(builder.Configuration);

builder.Services.AddAutoMapper(typeof(MappingProfile), typeof(ApplicationDbContext));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(MappingProfile).Assembly));

builder.Services.AddMemoryCache();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(WatchBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));

builder.Services.AddHangfire(x => x.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("ConnectLiveContext"))));
//builder.Services.AddHangfireServer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        var origins = builder.Configuration["Security:AllowedOrigins"].Split(";");
        policyBuilder.WithOrigins(origins);
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();

app.UseHangfireDashboard("/workers", new DashboardOptions
{
    Authorization = new[] { new AuthorizationFilter() }
});

//app.UseCustomException();
app.MapControllers();
app.UseCors("CorsPolicy");

app.Run();
