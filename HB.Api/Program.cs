using HB.Domain.Model;
using HB.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Marten;
using HB.Service.Card;
using HB.Service.Customer;
using HB.Service.Transaction;
using HB.Service.Payment;
using HB.Service.Engine;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using HB.SharedObject;
using HB.Service.Log;
using HB.Service.User;
using HB.Infrastructure.Exceptions;
using HB.Infrastructure.Jwt;
using HB.Infrastructure.Engine;
using HB.Service.Firebase;
using HB.Service.File;
using PdfTurtleClientDotnet;
using HB.Infrastructure.Repository;
using HB.Infrastructure.DbContext;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<HbContext>(opt => opt.UseNpgsql(connectionString?
    .ToString(), o => { o.MigrationsAssembly("HB.Api"); }));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddMarten(x => { x.Connection(connectionString); });

builder.Services.FirebaseAuthRegister(configuration);

#region Register Services

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFirebaseService, FirebaseService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.Configure<JwtModel>(configuration.GetSection("Jwt"));
builder.Services.Configure<Commission>(configuration.GetSection("Commission"));
builder.Services.AddPdfTurtle("https://pdfturtle.gaitzsch.dev");
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IContext), typeof(Context));

#endregion

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddAutoMapper(typeof(AutoMapperRegister).Assembly);

#region Register Swagger and Jwt

builder.JwtAndSwaggerRegister();

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region Cors

builder.Services.AddCors(p => p.AddPolicy("CorsApp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

#endregion

var app = builder.Build();

#region CustomExceptionHandler

app.UseExceptionHandlerRegister();

#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsApp");

app.UseAuthorization();

app.MapControllers();

app.Run();

