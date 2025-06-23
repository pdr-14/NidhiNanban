using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http;
using System;
using System.Net;
using System.Net.Http.Headers;

namespace Nidhinanban.Api.Controllers;

[ApiController]
[Route("getimg/[controller]")]
public class GetImageProxyController : Controller
{
    private readonly HttpClient _httpClient;
    public GetImageProxyController(IHttpClientFactory httpClientFactoryOptions)
    {
        _httpClient = httpClientFactoryOptions.CreateClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7065");
    }
    [HttpGet("{id}/profile")]
    public async Task<IActionResult> GetImageProfileImage(string id)
    {
        var Token = Request.Cookies["token"];
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var response = await _httpClient.GetAsync($"/getimage/GetImage/{id}/ProfileImage.jpg");
        if (!response.IsSuccessStatusCode)
        {
            return Forbid("Access deined");
        }
        var file = await response.Content.ReadAsStreamAsync();
        return File(file, "image/jpeg");
    }

    //House Image
    [HttpGet("{id}/house")]
    public async Task<IActionResult> GetHouseImage(string id)
    {
        var Token = Request.Cookies["token"];
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var response = await _httpClient.GetAsync($"/getimage/GetImage/{id}/HouseImage.jpg");
        if (!response.IsSuccessStatusCode)
        {
            return Forbid("Access deined");
        }
        var file = await response.Content.ReadAsStreamAsync();
        return File(file, "image/jpeg");
    }
    //Aadhar image
    [HttpGet("{id}/aadhar")]
    public async Task<IActionResult> GetAadharImage(string id)
    {
        var Token = Request.Cookies["token"];
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var response = await _httpClient.GetAsync($"/getimage/GetImage/{id}/AadhaarImage.jpg");
        if (!response.IsSuccessStatusCode)
        {
            return Forbid("Access deined");
        }
        var file = await response.Content.ReadAsStreamAsync();
        return File(file, "image/jpeg");
    }
    //Pan Image
    [HttpGet("{id}/pan")]
    public async Task<IActionResult> GetPanCardImage(string id)
    {
        var Token = Request.Cookies["token"];
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var response = await _httpClient.GetAsync($"/getimage/GetImage/{id}/PanCardImage.jpg");
        if (!response.IsSuccessStatusCode)
        {
            return Forbid("Access deined");
        }
        var file = await response.Content.ReadAsStreamAsync();
        return File(file, "image/jpeg");
    }
}
