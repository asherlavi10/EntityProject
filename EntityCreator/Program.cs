
using EntityCreator;
using EntityCreator.Sender;
using EntityCreator.Services;
using EntityDataContract;
using EntityDataContract.Validor;
using FluentValidation;
using Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IValidator<EntityDataContract.EntityDto>, EntityValidator>();
// Add services to the container.
builder.Services.AddTransient<ISendEntityFactory, SendEntityFactory>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("EntityPresentor", HttpClient => 
{

    HttpClient.BaseAddress = new Uri(builder.Configuration.GetSection("EntityPresentorUrl").Value);
    //  new Uri("https://localhost:7116");
});
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.SetUpRabbitMq(builder.Configuration);
builder.Services.AddSingleton<IRabbitSender,RabbitSender>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
