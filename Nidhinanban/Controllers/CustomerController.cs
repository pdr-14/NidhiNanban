using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Services;
using Nidhinanban.Models;

namespace Nidhinanban.Controllers
{
    public class CustomerController: Controller
    {
        
        private readonly AddCustomerService _addCustomerService;
        public CustomerController(AddCustomerService addCustomerService)
        {
            _addCustomerService = addCustomerService;
        }
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View(new AddCustomerModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerModel model)
        {
           Console.WriteLine(ModelState.IsValid.ToString());
            if(ModelState.IsValid)
            {
                model.success=true;
                string status = await _addCustomerService.AddCustomer(model.CustomerName!,model.CustomerPhonenumber!,model.CustomerAddress!,model.CustomerProfileimage!,model.CustomerAadharimage!,model.CusomerPancardimage!,model.CustomerHouseimage!);
                
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
    return PartialView("_added", model);
        
        }
    }
}
