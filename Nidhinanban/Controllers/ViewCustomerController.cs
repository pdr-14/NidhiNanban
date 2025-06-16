using System.Reflection.Emit;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;
using Nidhinanban.Services;
using System.Data;

namespace Nidhinanban.Api.Controllers
{
    [ApiController]
    [Route("View/[controller]")]
    public class ViewCustomerController : ControllerBase
    {

        private readonly ViewCustomerService _customerService;
        public ViewCustomerController(ViewCustomerService customer)
        {
            _customerService = customer;
        }
        [HttpGet]
        public async Task<ActionResult<List<ViewCustomer>>>GetAll()
        {
            var n = await _customerService.getallcustomer();
            if (n.Count == 0)
            {
                return BadRequest("No customers found");
            }
            return Ok(n);
        }
       
    }
}
