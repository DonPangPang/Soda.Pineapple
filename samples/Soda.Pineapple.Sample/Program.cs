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

builder.Services.AddSodaPineapple<SampleDbContext>(builder =>
    {
        builder.UseInMemoryDatabase("test");
    })
    .ReplaceServices<ITableSplittingRule, SplitBaseOnDate>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSodaPineapple();

// app.UseAuthorization();
app.MapControllers();

app.Run();
