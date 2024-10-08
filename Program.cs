using Microsoft.EntityFrameworkCore;
using PinThePlace.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PinDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:PinDbContextConnection"]);
});

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.MapDefaultControllerRoute();

app.Run();
