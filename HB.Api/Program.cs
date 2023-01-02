using HB.Domain.Model;
using HB.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Marten;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<HbContext>(opt => opt.UseNpgsql(connectionString?
    .ToString(), o => { o.MigrationsAssembly("HB.Api"); }));

builder.Services.AddMarten(x => { x.Connection(connectionString); });

builder.Services.AddScoped<IInformationService, InformationService>();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


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

