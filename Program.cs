using Microsoft.EntityFrameworkCore;
using MvcEFRelationshipsDemo.Data;
using MvcEFRelationshipsDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ SEEDING DATA
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!db.Customers.Any())
    {
        db.Customers.Add(new Customer
        {
            Name = "Mudassar Ali",
            Orders = new List<Order>
            {
                new Order { Product = "Laptop" },
                new Order { Product = "Monitor" }
            }
        });

        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
