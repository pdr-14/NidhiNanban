using System.Reflection.Emit;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;
using Nidhinanban.Services;
using System.Data;
using DocumentFormat.OpenXml.Drawing.Charts;

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
        [HttpGet("getall")]
        public async Task<ActionResult<List<ViewCustomer>>> GetAll()
        {
            var n = await _customerService.getallcustomer();
            if (n.Count == 0)
            {
                return BadRequest("No customers found");
            }
            return Ok(n);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerByID(string id)
        {
            var details = await _customerService.getCustomerById(id);
            if (details.Count == 0)
            {
                return BadRequest("No Customer Found");
            }

            return Ok(details);
        }
       
    }
}
