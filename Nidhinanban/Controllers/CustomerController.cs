using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Services;
using Nidhinanban.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Asn1.Ocsp;
using DocumentFormat.OpenXml.Drawing.Charts;

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
            var jwttoken = Request.Cookies["token"]!;
            if (String.IsNullOrEmpty(jwttoken))
            {
                return RedirectToAction("LoginIn", "Signing");
            }
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


        //get's the customer for customer grid
        [HttpGet]
        public async Task<IActionResult> ViewCustomer(int pagenumber = 1)
        {
            var jwttoken = Request.Cookies["token"]!;
            if (!String.IsNullOrEmpty(jwttoken))
            {
                int count = 1;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);

                if (ViewData["TotalCount"] == null)
                {
                    ViewData["TotalCount"] = "0";
                }
                else if (count < pagenumber)
                {
                    pagenumber = count;
                }
                else if (pagenumber < 1)
                {
                    pagenumber = 1;
                }


                var countresponse = await _httpClient.GetAsync("/View/ViewCustomer/getcount");

                if (!countresponse.IsSuccessStatusCode) //check's the response code is success or not 
                {
                    ViewBag.Error = "No customers found";
                    return View(new List<ViewCustomer>());
                }

                count = await countresponse.Content.ReadFromJsonAsync<int>();
                ViewData["TotalCount"] = Math.Ceiling((double)count / 12);
                ViewBag.CurrentPage = pagenumber;
                if (ViewData["TotalCount"] is null) //checking count is null or not
                {
                    ViewData["TotalCount"] = 1;
                }
                else if (Convert.ToInt32(ViewData["TotalCount"]) < pagenumber) //checking if pagenumber is greater than total count
                {
                    pagenumber = Convert.ToInt32(ViewData["TotalCount"]);
                    ViewBag.CurrentPage = pagenumber;
                }
                else if (pagenumber < 1) //checking if pagenumber is less than 1
                {
                    pagenumber = 1;
                    ViewBag.CurrentPage = pagenumber;
                }
                var response = await _httpClient.GetAsync("/View/ViewCustomer/getall/" + pagenumber);
                if (!response.IsSuccessStatusCode) //check's the response code is success or not
                {
                    ViewBag.Error = "No customers found";
                    return View(new List<ViewCustomer>());
                }

                var data = await response.Content.ReadFromJsonAsync<List<ViewCustomer>>();


                return View(data);
            }
            else
            {
                return RedirectToAction("LoginIn", "Signing");
            }
        }

        //gets the customer for the list view
        [HttpGet]
        public async Task<IActionResult> ViewCustomerlist(int pagenumber = 1)
        {
            var jwttoken = Request.Cookies["token"]!;
            if (!String.IsNullOrEmpty(jwttoken))
            {
                int count = 1;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);

                if (ViewData["TotalCount"] == null)
                {
                    ViewData["TotalCount"] = "0";
                }
                else if (count < pagenumber)
                {
                    pagenumber = count;
                }
                else if (pagenumber < 1)
                {
                    pagenumber = 1;
                }


                var countresponse = await _httpClient.GetAsync("/View/ViewCustomer/getcount");

                if (!countresponse.IsSuccessStatusCode) //check's the response code is success or not 
                {
                    ViewBag.Error = "No customers found";
                    return View(new List<ViewCustomer>());
                }

                count = await countresponse.Content.ReadFromJsonAsync<int>();
                ViewData["TotalCount"] = Math.Ceiling((double)count / 12);
                ViewBag.CurrentPage = pagenumber;
                if (ViewData["TotalCount"] is null) //checking count is null or not
                {
                    ViewData["TotalCount"] = 1;
                }
                else if (Convert.ToInt32(ViewData["TotalCount"]) < pagenumber) //checking if pagenumber is greater than total count
                {
                    pagenumber = Convert.ToInt32(ViewData["TotalCount"]);
                    ViewBag.CurrentPage = pagenumber;
                }
                else if (pagenumber < 1) //checking if pagenumber is less than 1
                {
                    pagenumber = 1;
                    ViewBag.CurrentPage = pagenumber;
                }
                var response = await _httpClient.GetAsync("/View/ViewCustomer/getall/" + pagenumber);
                if (!response.IsSuccessStatusCode) //check's the response code is success or not
                {
                    ViewBag.Error = "No customers found";
                    return View(new List<ViewCustomer>());
                }

                var data = await response.Content.ReadFromJsonAsync<List<ViewCustomer>>();


                return View(data);
            }
            else
            {
                return RedirectToAction("LoginIn", "Signing");
            }
        }
        //show the seperate customer details
        [HttpGet]
        public async Task<IActionResult> CustomerDetails()
        {
            var jwttoken = Request.Cookies["token"];
            string id = Request.Query["id"]!;
            if (!Request.Query.TryGetValue("id", out var idValue) || string.IsNullOrEmpty(idValue))
            {
                return RedirectToAction("ViewCustomer","Customer");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwttoken);
            var response = await _httpClient.GetAsync($"/View/ViewCustomer/{id}");
            var customerdetail = await response.Content.ReadFromJsonAsync<List<ViewCustomer>>();
            if (customerdetail!.Count == 0)
            {
                return BadRequest("Error");
            }
            return View(customerdetail);
        }
        //search the customer
        
        [Route("Customer/[controller]/")]
        public async Task<IActionResult> SearchCustomer(string CustomerId)
        {
            var token = Request.Cookies["token"]!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            Console.WriteLine(CustomerId);
            if (String.IsNullOrEmpty(token))
            {
                return RedirectToAction("LoginIn", "Signing");
            }
            if (String.IsNullOrEmpty(CustomerId))
            {
                return RedirectToAction("ViewCustomer","Customer");
            }
            var response = await _httpClient.GetAsync($"/View/ViewCustomer/{CustomerId}");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("ViewCustomer","Customer");
            }
            var details = await response.Content.ReadFromJsonAsync<List<ViewCustomer>>();
            ViewBag.CurrentPage = 1;
            ViewData["TotalCount"] = 1;
            return View("ViewCustomer", details);
        }
    }
}
