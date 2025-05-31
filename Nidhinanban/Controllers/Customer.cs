using System.Reflection.Emit;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Nidhinanban.Controllers
{
    public class Customer: Controller
    {
        public IActionResult AddCustomer()
        {
            return View();
        }
    }
}
