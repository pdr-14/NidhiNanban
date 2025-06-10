using System;
using System.Collections.Immutable;
using Nidhinanban.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<InterestService>();
builder.Services.AddTransient<AddCustomerService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

//use this if https is not working means
builder.WebHost.UseUrls("https://0.0.0.0:7066");

//  builder.WebHost.ConfigureKestrel(serverOptions=>{
//     serverOptions.ListenAnyIP(5209);
//      serverOptions.ListenAnyIP(7066,listenOptions=>{
//          listenOptions.UseHttps();
//      });
//  });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Shows detailed errors during development

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
