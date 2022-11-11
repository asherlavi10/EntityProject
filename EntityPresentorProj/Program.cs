using EntityDataContract;
using EntityPresentorProj.Extention;
using EntityPresentorProj.Hubs;
using EntityPresentorProj.Models;
using EntityPresentorProj.Services;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddEntityServies();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetSection("RedisCacheUrl").Value;
    option.InstanceName = "";
});


builder.Services.Configure<SignalROptions>(
    builder.Configuration.GetSection(SignalROptions.Name));

builder.Services.Configure<ImageDrawOptions>(
    builder.Configuration.GetSection(ImageDrawOptions.Name));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img")),
    RequestPath = "/wwwroot/img"
});

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ImageHub>("/chatHub");

var cahcheService = app.Services.GetService<ICacheService>();
cahcheService.SetStringValue("curImg", Consts.MainImage);

app.Run();
