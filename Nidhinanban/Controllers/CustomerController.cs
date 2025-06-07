using System.Security.Principal;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Services;
using Newtonsoft.Json;
using Nidhinanban.Models;

namespace Nidhinanban.Controllers
{
    public class CustomerController: Controller
    {
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View(new AddCustomerModel());
        }

        [HttpPost]
        public IActionResult AddCustomer(AddCustomerModel model)
        {
           Console.WriteLine(ModelState.IsValid.ToString());
            if(ModelState.IsValid)
            {
                model.success=true;
                
                return PartialView("_added",model);
            }
            if (!ModelState.IsValid)
{
    model.success=false;
    foreach (var error in ModelState)
    {
        Console.WriteLine($"Key: {error.Key}");
        foreach (var e in error.Value.Errors)
        {
            Console.WriteLine($"Error: {e.ErrorMessage}");
        }
    }
}

            return View(model);
        }
    }
}
