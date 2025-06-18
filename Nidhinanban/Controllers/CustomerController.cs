using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Services;
using Nidhinanban.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Nidhinanban.Controllers
{
    public class CustomerController : Controller
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
        public async Task<IActionResult> AddCustomer(AddCustomerModel model)
        {
            string id = await _addCustomerService.GetCustomerID();
            model.CustomerId = id;
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerToDatabase(AddCustomerModel model)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be null");
            }
            if (ModelState.IsValid)
            {
                string status = await _addCustomerService.AddCustomer(model.CustomerId!, model.CustomerName!, model.CustomerPhonenumber!, model.CustomerAddress!, model.CustomerProfileimage!, model.CustomerAadharimage!, model.CustomerPancardimage!, model.CustomerHouseimage!);
                if (status.ToLower() == "inserted")
                {
                    model.success = true;
                    return PartialView("_added", model);
                }
            }
            else if (!ModelState.IsValid)
            {
                model.success = false;
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
        public async Task<IActionResult> ViewCustomer()
        {
            var response = await _httpClient.GetAsync("/View/ViewCustomer/getall");
            if (!response.IsSuccessStatusCode) //check's the response code is success or not 
            {
                ViewBag.Error = "No customers found";
                return View(new List<ViewCustomer>());
            }
            var data = await response.Content.ReadFromJsonAsync<List<ViewCustomer>>();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> ViewCustomerlist()
        {
            var response = await _httpClient.GetAsync("/View/ViewCustomer/getall");
            if (!response.IsSuccessStatusCode) //check's the response code is success or not
            {
                ViewBag.Error = "No customers found";
                return View(new List<ViewCustomer>());
            }
            
            var data = await response.Content.ReadFromJsonAsync<List<ViewCustomer>>();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> CustomerDetails()
        {
            string id = Request.Query["id"]!;
            if (!Request.Query.TryGetValue("id", out var idValue) || string.IsNullOrEmpty(idValue))
            {
                return RedirectToAction("ViewCustomer");
            }
            var response = await _httpClient.GetAsync($"/View/ViewCustomer/{id}");
            var customerdetail = await response.Content.ReadFromJsonAsync<List<ViewCustomer>>();
            if (customerdetail!.Count == 0)
            {
                return BadRequest("Error");
            }
            return View(customerdetail);
        }
    }
}
