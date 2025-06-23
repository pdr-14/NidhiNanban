using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nidhinanban.Api.Controllers;
using Nidhinanban.LogicClasses;
using Nidhinanban.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region  Injection Region
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<InterestService>();
builder.Services.AddTransient<AddCustomerService>();
builder.Services.AddTransient<ViewCustomerService>();
builder.Services.AddSingleton<ImageManipulation>();
builder.Services.AddScoped<AddCustomerService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpClient();
#endregion

#region  Hosting Region
//use this if https is not working means
builder.WebHost.UseUrls("https://0.0.0.0:7065");

// builder.WebHost.ConfigureKestrel(serverOptions=>{
//      serverOptions.ListenAnyIP(5209);
//       serverOptions.ListenAnyIP(7065,listenOptions=>{
//           listenOptions.UseHttps();
//       });
//   });

#endregion

#region Authentication Region

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "https://localhost:7065",
        ValidAudience="https://localhost:7065",
        IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Ayds1jdAxlbJvSovJcXtckE9wXvNfh+oNDIdX+7ezOU="))
    };
});

builder.Services.AddAuthorization();
#endregion
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
#region  Required Region
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Signing}/{action=LoginIn}/{id?}");


app.UseAuthentication();
app.UseAuthorization();
app.Run();
#endregion