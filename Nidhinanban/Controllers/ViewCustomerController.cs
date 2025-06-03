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
        public ActionResult<List<ViewCustomer>> GetAll()
        {
            return Ok(_customerService.getall());
        }
        [HttpGet("ID")]
        public ActionResult<string> Get([FromQuery]string id)
        {
            return Ok(_customerService.getsatus(id));
        }
    }
}
