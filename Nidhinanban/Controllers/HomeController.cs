using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;

namespace Nidhinanban.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private HttpClient _httpClient;
    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient =httpClientFactory.CreateClient("https://localhost:7065");
    }

    public IActionResult Index()
    {
        var token = Request.Cookies["token"];
        if (String.IsNullOrEmpty(token))
        {
            return RedirectToAction("LoginIn", "Signing");
        }
        else
        {
            var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var decrepttoken = JwtSecurityTokenHandler.ReadJwtToken(token);
            LoggedInUserModel.UserName = decrepttoken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult ViewIntreset()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult ViewIntreset(ViewIntresetModel input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        Console.WriteLine("Input amount"+input.amount.ToString());
        decimal amount = input.amount;
        decimal month = input.month;
        decimal interestrate    = input.interestrate;
        input.totalamount = Math.Ceiling((amount*interestrate*month)/100);
        Console.WriteLine(""+input.totalamount);
            return View(input);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
