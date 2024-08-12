using System;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleAPI;
using SampleAPI.Application.Command;
using SampleAPI.Application.Validator;
using SampleAPI.Domain.Entities;
using SampleAPI.Domain.Repository;
using SampleAPI.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<IValidator<CreateOrderRequestCommand>, CreateOrderRequestValidator>();
builder.Services.AddScoped<IOrderDetailRepository,OrderDetailRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddDbContext<InMemoryDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "SampleDB");
    options.EnableSensitiveDataLogging();
});

var app = builder.Build();

var context = app.Services.CreateScope().ServiceProvider.GetService<InMemoryDbContext>();
DataSetup.Initialize(context!);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
