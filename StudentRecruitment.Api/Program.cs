using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentRecruitment.Api.Extensions;
using StudentRecruitment.DAL;
using StudentRecruitment.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The 'DefaultConnection' connection string is not defined in appsettings.json.");
}

builder.Services.AddDbContext<ApidDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging());


builder.Services.AddServices();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();