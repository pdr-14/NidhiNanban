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
    public IActionResult Getdata(ViewIntresetModel input)
    {
        Console.WriteLine("Input amount",input.amount);
            return View(input);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
