using AutoMapper;
using BLL;
using BLL.Interfaces;
using BLL.Services;
using DAL.HotelDatabaseContext;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.UnitOfWork;
using HotelTests.DALTests;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using WebProject.Filters;


static void AddInMemoryData(IApplicationBuilder app)
{
    var scope = app.ApplicationServices.CreateScope();
    var db = scope.ServiceProvider.GetService<HotelDbContext>();

    DBSeeder.SeedData(db);

}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("HotelDb");

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutomapperProfile());

}).CreateMapper());

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRoomHistoryRepository, RoomHistoryRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(HotelNotFoundExceptionFilterAttribute));
});


//builder.Services.AddDbContext<IHotelDbContext, HotelDbContext>(options =>
//                options.UseSqlServer(connectionString));


//InMemoryDatabase
builder.Services.AddDbContext<IHotelDbContext, HotelDbContext>
(o => o.UseInMemoryDatabase("HotelDb"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

var app = builder.Build();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("/echo",
//        context => context.Response.WriteAsync("echo"))
//        .RequireCors(MyAllowSpecificOrigins);

//    endpoints.MapControllers()
//             .RequireCors(MyAllowSpecificOrigins);

//    endpoints.MapGet("/echo2",
//        context => context.Response.WriteAsync("echo2"));

//    endpoints.MapRazorPages();
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("EnableCORS");

app.UseAuthorization();

app.MapControllers();

//InMemoryDatabase
AddInMemoryData(app);

app.Run();
