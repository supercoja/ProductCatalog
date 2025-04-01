using ProductCatalog.Data.Fake;
using ProductCatalog.Domain;
using webAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<ICategoryRepository, FakeCategoryRepository>();
builder.Services.AddSingleton<IProductRepository, FakeProductRepository>();

builder.Services.AddSingleton<IOrderRepository, FakeOrderRepository>();
builder.Services.AddSingleton<IOrderQueue, InMemoryOrderQueue>();
builder.Services.AddScoped<IOrderProcessor, OrderProcessor>();

builder.Services.AddHostedService<OrderProcessingService>();

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