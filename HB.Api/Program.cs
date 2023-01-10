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

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<HbContext>(opt => opt.UseNpgsql(connectionString?
    .ToString(), o => { o.MigrationsAssembly("HB.Api"); }));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddMarten(x => { x.Connection(connectionString); });

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddAutoMapper(typeof(AutoMapperRegister).Assembly);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

