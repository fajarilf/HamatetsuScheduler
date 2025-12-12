using HamatetsuScheduler.Api.Infrastructure;
using HamatetsuScheduler.Api.Middleware;
using HamatetsuScheduler.Api.Repository.Implementation;
using HamatetsuScheduler.Api.Service.Implementation;
using HamatetsuScheduler.Api.Service.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString) 
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<PartRepository>();
builder.Services.AddScoped<ProcessRepository>();
builder.Services.AddScoped<ProcessListRepository>();
builder.Services.AddScoped<ScheduleDetailRepository>();
builder.Services.AddScoped<ScheduleRepository>();
builder.Services.AddScoped<SchedulePerDayRespository>();

builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<IProcessListService, ProcessListService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
