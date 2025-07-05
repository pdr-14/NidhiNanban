using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using QRCoder.Core;
using Microsoft.AspNetCore.Authorization;

namespace Nidhinanban.Controllers;

[ApiController]
[Route("Generate")]

public class GenerateQRcodeController : ControllerBase
{
    
    [HttpGet("getqrcode/{id}")]
    public async Task<IActionResult> GetQRCode(string id)
    {
        try
        {
            byte[] qrcodeImage = null!;
            await Task.Run(() =>
            {
                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                Uri url = new Uri($"https://www.google.com/search?q={id}");
                QRCodeData qrdata = qRCodeGenerator.CreateQrCode(url.ToString(), QRCodeGenerator.ECCLevel.H);
                PngByteQRCode pngQRCode = new PngByteQRCode(qrdata);
                qrcodeImage = pngQRCode.GetGraphic(20);

            });
            return File(qrcodeImage, "image/png");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
