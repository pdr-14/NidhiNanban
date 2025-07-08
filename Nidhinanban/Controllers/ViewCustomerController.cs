using System.Reflection.Emit;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;
using Nidhinanban.Services;
using System.Data;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Org.BouncyCastle.Asn1.Cms;
using Microsoft.Extensions.Caching.Memory;

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
        [HttpGet("getall/{pagenumber}")]
        [Authorize]
        [ResponseCache(Duration =600,NoStore =false,VaryByQueryKeys =new[]{"pagenumber"})]        // VaryByHeader is used to vary the cache based on the User-Agent header,
        public async Task<ActionResult<List<ViewCustomer>>> GetAll(int pagenumber)
        {
            try
            {
                int PageNumber = pagenumber * 12;
                var n = await _customerService.getallcustomer(pagenumber);
                if (n.Count == 0)
                {
                    return BadRequest("No customers found");
                }

                return Ok(n);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetCustomerByID(string id)
        {
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            if (cache.TryGetValue(id, out List<ViewCustomer> cachedCustomer))
            {
                return Ok(cachedCustomer);
            }
            var details = await _customerService.getCustomerById(id);
            if (details.Count == 0)
            {
                return BadRequest("No Customer Found");
            }
            else
            {
                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Cache for 10 minutes

                // Save data in cache
                cache.Set(id, details, cacheEntryOptions);
            }
            return Ok(details);
        }

        [HttpGet("getcount")]
        [Authorize]
        public async Task<ActionResult> getcount()
        {
            var count = await _customerService.Totalcount();
            return Ok(count);
        }
       
    }
}
