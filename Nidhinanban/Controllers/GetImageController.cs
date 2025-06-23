using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nidhinanban.Api.Controllers;
[ApiController]
[Route("getimage/[Controller]")]
public class GetImageController : ControllerBase
{

    private readonly IHostEnvironment _env;
    public GetImageController(IHostEnvironment env)
    {
        _env = env;
    }
    [HttpGet("{id}/{filename}")]
    [Authorize]
    public IActionResult getImage(string filename, string id)
    {
        filename = Path.GetFileName(filename);
        var folder = Path.Combine(_env.ContentRootPath, "Customer_Images", id);
        var file = Path.Combine(folder, id + filename);
        if (!System.IO.File.Exists(file))
            return NotFound();
        return PhysicalFile(file, "image/jpeg");
    }
}
