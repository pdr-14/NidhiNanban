using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nidhinanban.Api.Controllers;
using Nidhinanban.LogicClasses;
using Nidhinanban.Services;
using System.Text;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

#region  Injection Region
// Add services to the container.
builder.Services.AddControllersWithViews(options=>{
    options.CacheProfiles.Add("Default",
        new Microsoft.AspNetCore.Mvc.CacheProfile
        {
            Duration = 600, // Cache for 60 seconds
            Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Client,
            NoStore = false ,// Allow caching,
            VaryByHeader = "User-Agent" // Vary cache by User-Agent header
        });
});
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
        ValidAudience = "https://localhost:7065",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["key"]!))
    };
});
#region compression region
//adding the response compression to provide the fastest response
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>(); //adding the gzip compression provider
    options.Providers.Add<BrotliCompressionProvider>(); //adding the brotlicompress provider for the compression method
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" }); //adding the json compression
});
builder.Services.Configure<GzipCompressionProviderOptions>(option =>
{
    option.Level = CompressionLevel.Fastest; //setting compression faster 
});
builder.Services.Configure<BrotliCompressionProviderOptions>(option =>
{
    option.Level = CompressionLevel.Fastest;
});
#endregion
builder.Services.AddResponseCaching();
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
if (app.Environment.IsDevelopment())
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
app.UseResponseCaching();
app.UseResponseCompression();
app.Run();
#endregion