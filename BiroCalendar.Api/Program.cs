using BiroCalendar.Api.Persistance;
using Microsoft.EntityFrameworkCore;
using BiroCalendar.Api.Endpoints;
using BiroCalendar.Api.Services;
using BiroCalendar.Api.Persistance.Entities;
using Microsoft.AspNetCore.Identity;
using BiroCalendar.Api.BackgroundServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using BiroCalendar.Api.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<BiroClient>();
builder.Services.AddHostedService<BiroAccountService>();
builder.Services.AddScoped<IPasswordHasher<Account>, AccountPasswordHasher>();

builder.Services.AddDbContext<AppDbContext>(_ =>
{
    _.UseSqlite(builder.Configuration.GetConnectionString("BiroCalendar"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(_ =>
    {
        _.Cookie.Name = "BiroCalendarSession";
        _.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        _.Cookie.IsEssential = true;
        _.ForwardForbid = CookieAuthenticationDefaults.AuthenticationScheme;
    });
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapHealthChecks("/health", new()
{
    AllowCachingResponses = true,
})
    .WithOpenApi();

app.MapAccountEndpoints();
app.MapBiroAccountEndpoints();
app.MapCalendarEndpoints();

using (var scope = app.Services.CreateScope())
{
    using var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
    dbContext?.Database.Migrate(); 
}

app.Run();
