using System.Drawing;
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nidhinanban.Models;

namespace Nidhinanban.Controllers;

public class Signing : Controller
{

    readonly HttpClient _httpClient;
    public Signing(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7065");
    }
    
    //IT returns the View
    public IActionResult LoginIn()
    {
        return View();
    }
    [AllowAnonymous]
    [HttpPost]
    public  async Task <IActionResult> SignIn(LoginDataModel loginDataModel)
    {
        string user = loginDataModel.UserName;
        string password = loginDataModel.Password;
        var loginData=new LoginDataModel{UserName=user, Password=password};
        var jsondata = System.Text.Json.JsonSerializer.Serialize(loginData);
        var Content = new StringContent(jsondata, Encoding.UTF8, "application/json");
        var postresult=await _httpClient.PostAsync("api/LoginApi/putlogin", Content);
        if (postresult.IsSuccessStatusCode)
        {
            var token = await postresult.Content.ReadAsStringAsync();
             Response.Cookies.Append("token", token!,new CookieOptions
             {
                 HttpOnly = true,
                 Secure = true,
                 SameSite = SameSiteMode.Strict,
                 Expires=DateTime.Now.AddDays(1)  
             });
            Response.Headers.Append( "Authorization", "Bearer "+token );
            return RedirectToAction("Index","Home");
        }
        else
        {
            var reslut = postresult.Content.ReadAsStringAsync();
            return Ok(reslut);
        }
        
    }
}
