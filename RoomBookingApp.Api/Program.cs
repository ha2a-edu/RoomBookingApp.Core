using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.DataServices;
using RoomBookingApp.Persistance;
using RoomBookingApp.Persistance.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = "DataSource=:memory:";
var connection = new SqliteConnection(connectionString);
connection.Open();
builder.Services.AddDbContext<RoomBookingAppDbContext>(options => options.UseSqlite(connection));

EnsureDatabaseCreated(connection);

builder.Services.AddScoped<IRoomBookingRequestProcessor, RoomBookingRequestProcessor>();
builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();

var app = builder.Build();

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

void EnsureDatabaseCreated(SqliteConnection connection)
{
    var dbBuilder = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
    dbBuilder.UseSqlite(connection);

    using var context = new RoomBookingAppDbContext(dbBuilder.Options);
    context.Database.EnsureCreated();
}