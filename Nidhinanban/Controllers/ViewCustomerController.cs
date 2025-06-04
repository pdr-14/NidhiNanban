using System.Reflection.Emit;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;
using Nidhinanban.Services;
using System.Data;

namespace Nidhinanban.Controllers
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
            var n =  await _customerService.getall(); 
            return Ok(n);
        }
        [HttpGet("ID")]
        public ActionResult<string> Get([FromQuery]string id)
        {
            return Ok(_customerService.getsatus(id));
        }
    }
}
