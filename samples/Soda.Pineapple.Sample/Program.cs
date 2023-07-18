using Microsoft.EntityFrameworkCore;
using Soda.Pineapple;
using Soda.Pineapple.Options;
using Soda.Pineapple.Sample.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<SampleDbContext>(opts =>
// {
//     opts.UseInMemoryDatabase("test");
// });

builder.Services.AddSodaPineapple<SampleDbContext>(opts =>
{
    opts.UseMySql("Server=127.0.0.1;Port=3306;Database=pine;Uid=root;Pwd=Mx@7722111;", new MySqlServerVersion(new Version(8, 0)), opt =>
    {

    });
}, ServiceLifetime.Scoped, options => options.SplittingRule = new SplitBaseOnDate(DateMode.YearMonthDayHourMin));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var scoped = app.Services.CreateScope();
var db = scoped.ServiceProvider.GetRequiredService<SampleDbContext>();
db.Database.EnsureCreated();

app.UseSodaPineapple();

// app.UseAuthorization();
app.MapControllers();

app.Run();
