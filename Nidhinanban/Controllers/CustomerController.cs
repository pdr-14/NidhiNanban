using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Services;
using Nidhinanban.Models;
using System.Net.Http;

namespace Nidhinanban.Controllers
{
    public class CustomerController: Controller
    {
        
        private readonly AddCustomerService _addCustomerService;
        private readonly HttpClient _httpClient;
        public CustomerController(AddCustomerService addCustomerService, IHttpClientFactory httpClientFactory)
        {
            _addCustomerService = addCustomerService;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7065/"); 
        }
        
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View(new AddCustomerModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerModel model)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be null");
            }
            if (ModelState.IsValid)
            {

                string status = "Inserted"; //await _addCustomerService.AddCustomer(model.CustomerName!, model.CustomerPhonenumber!, model.CustomerAddress!, model.CustomerProfileimage!, model.CustomerAadharimage!, model.CustomerPancardimage!, model.CustomerHouseimage!);
                Console.WriteLine(status+":"+(status.ToLower() == "inserted").ToString());
                if (status.ToLower() == "inserted")
                {
                    model.success = true;
                    return PartialView("_added", model);
                }
            }
            else if (!ModelState.IsValid)
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
        [HttpGet]
        public async  Task<IActionResult> ViewCustomer()
        {
            var response = await _httpClient.GetAsync("/View/ViewCustomer");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "No customers found";
                return View(new List<ViewCustomer>());
            }

            var data = await response.Content.ReadFromJsonAsync<List<ViewCustomer>>();
            return View(data);
        }
    }
}
