using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;

namespace Nidhinanban.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
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
