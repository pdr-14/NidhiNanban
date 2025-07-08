using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nidhinanban.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Components.Web;
using DocumentFormat.OpenXml.EMMA;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using QuestPDF.Elements.Table;
using QuestPDF.Previewer;

using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Google.Protobuf.WellKnownTypes;
using PdfSharpCore.Pdf;
using System.Reflection.Metadata.Ecma335;



namespace Nidhinanban.Api.Controllers;

[ApiController]
[Route("GeneratePDF")]
public class GeneratePDFController : ControllerBase
{
    
    [HttpGet("getpdf")]
    public  async Task<IActionResult> GetPDF([FromBody] PDFModel pdfModel)
    {

        try
        {
            var qrcode = "https://localhost:7065/Generate/getqrcode/thala";
            using var _httpclient = new HttpClient();
            var qrcodeasbytearray = await _httpclient.GetByteArrayAsync(qrcode);
            var logocode = await _httpclient.GetByteArrayAsync("https://www.designevo.com/res/templates/thumb_small/shake-flask-liquid-test.webp");
            var profileimagebinary = await _httpclient.GetByteArrayAsync("https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Sam_Altman_TechCrunch_SF_2019_Day_2_Oct_3_%28cropped%29.jpg/1200px-Sam_Altman_TechCrunch_SF_2019_Day_2_Oct_3_%28cropped%29.jpg");
            var pdfDocument = QuestPDF.Fluent.Document.Create(pdfcontainer =>
            {

                pdfcontainer.Page(pdfpage =>
                {
                    pdfpage.Size(PageSizes.A4);
                    pdfpage.Margin(20);

                    pdfpage.Header().Row(pageheader =>
                    {
                        pageheader.RelativeItem(1).Element(logo =>
                        {
                            logo.AlignLeft().Width(100).Height(100).Image(logocode);
                        });
                        pageheader.RelativeItem(2).Element(companydetails =>
                        {
                            companydetails.Column(columndetails =>
                            {
                                columndetails.Spacing(8);
                                columndetails.Item().AlignCenter().Text("Hubert Blaine Wolfeschlegelsteinhausenbergerdorff Sr Comany").FontSize(12).SemiBold();
                                columndetails.Item().AlignCenter().Text("No 4/21 Sri Lakshmi Complex 2nd Floor, Opp to ANS Jewellery, Omalur Main Rd, Veerapandiyar Nagar, Sw Salem").FontSize(10);
                                columndetails.Item().AlignCenter().Text("PHNO:9876543210").FontSize(10);
                            });
                        });
                        pageheader.RelativeItem(1).Element(qrcodeplace =>
                        {
                            qrcodeplace.AlignRight().Width(60).Height(60).Image(qrcodeasbytearray).FitArea();
                        });
                    });
                    pdfpage.Content().Column(pagecontent =>
                    {
                        //customer image and date detail
                        pagecontent.Item().Row(imageanddaterow =>
                        {
                            imageanddaterow.RelativeItem(1).Element(profileimage =>
                            {
                                profileimage.AlignLeft().Width(70).Height(70).Image(profileimagebinary);

                            });
                            imageanddaterow.RelativeItem(1).Element(e =>
                            {

                                e.Column(ecolumndata =>
                                {
                                    ecolumndata.Spacing(5);
                                    ecolumndata.Item().AlignRight().Text("Date:" + DateTime.Now.ToString("dd/MM/yyyy")).FontSize(11);
                                    ecolumndata.Item().AlignRight().Text("Time:" + DateTime.Now.ToLongTimeString()).FontSize(11);
                                });
                            });
                        });
                        //name details of the customer
                        pagecontent.Item().Row(customerdetails =>
                        {
                            customerdetails.RelativeItem(1).Element(customerdetailsrow =>
                            {
                                customerdetailsrow.Column(customerdetailscolumn =>
                                {
                                    customerdetailscolumn.Spacing(5);
                                    customerdetailscolumn.Item().AlignLeft().PaddingTop(10).Width(300).Text(name =>
                                    {
                                        name.Span("Name:").Bold().FontSize(11);
                                        name.Span(" Customer Name");
                                    });
                                    customerdetailscolumn.Item().AlignLeft().Text(phonenumber =>
                                    {
                                        phonenumber.Span("Phone Number:").Bold().FontSize(11);
                                        phonenumber.Span("  9876543210").FontSize(11);
                                    });
                                    customerdetailscolumn.Item().AlignLeft().Width(300).Text(address =>
                                    {
                                        address.Span("Address:").Bold().FontSize(11);
                                        address.Span("  No 4/21 Sri Lakshmi Complex 2nd Floor, Opp to ANS Jewellery, Omalur Main Rd, Veerapandiyar Nagar, Sw Salem").FontSize(11);
                                    });
                                });
                            });
                        });

                        //interest and principle and total amount
                        pagecontent.Item().Row(amountrow =>
                        {
                            amountrow.Spacing(5);
                            amountrow.RelativeItem(1).Element(element =>
                            {
                                element.Column(principleamount =>
                                {

                                    principleamount.Spacing(5);
                                    principleamount.Item().PaddingTop(30).Text("PRINCIPLE AMOUNT").AlignCenter().FontSize(11).SemiBold();
                                    principleamount.Item().PaddingTop(3).Text("$100000000000000000000000").AlignCenter().FontSize(11);
                                });
                            });
                            amountrow.RelativeItem(1).Element(element =>
                            {
                                element.Column(principleamount =>
                                {
                                    principleamount.Spacing(5);
                                    principleamount.Item().PaddingTop(30).Text("INTEREST AMOUNT").AlignCenter().FontSize(11).SemiBold();
                                    principleamount.Item().PaddingTop(3).Text("$100000000000000000000000").AlignCenter().FontSize(11);
                                });
                            });
                            amountrow.RelativeItem(1).Element(element =>
                            {
                                element.Column(principleamount =>
                                {
                                    principleamount.Spacing(5);
                                    principleamount.Item().PaddingTop(30).Text("TOTAL AMOUNT").AlignCenter().FontSize(11).SemiBold();
                                    principleamount.Item().PaddingTop(3).Text("$100000000000000000000000").AlignCenter().FontSize(11);
                                });
                            });
                        });

                        //Generate table 
                        pagecontent.Item().PaddingTop(20).Table(table =>
                        {

                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            IContainer CellStyle(IContainer container) =>
                                    container.Border(1)
                                    .BorderColor(Colors.Grey.Medium)
                                   .Padding(5);
                            table.Header(tableheader =>
                            {
                                tableheader.Cell().Element(CellStyle).Text("Sno").AlignCenter().FontSize(11).SemiBold();
                                tableheader.Cell().Element(CellStyle).Text("Date").AlignCenter().FontSize(11).SemiBold();
                                tableheader.Cell().Element(CellStyle).Text("Principle Amount").AlignCenter().FontSize(11).SemiBold();
                                tableheader.Cell().Element(CellStyle).Text("Interest Amount").AlignCenter().FontSize(11).SemiBold();
                                tableheader.Cell().Element(CellStyle).Text("Total Amount").AlignCenter().FontSize(11).SemiBold();
                                tableheader.Cell().Element(CellStyle).Text("Status").AlignCenter().FontSize(11).SemiBold();
                            });
                            for (int i = 1; i <= 300; i++)
                            {
                                table.Cell().Element(CellStyle).AlignCenter().Text(i).FontSize(11);
                                table.Cell().Element(CellStyle).AlignCenter().Text(DateTime.Now.AddMonths(i)).FontSize(11);
                                table.Cell().Element(CellStyle).AlignCenter().Text("1000000000000000000").FontSize(11);
                                table.Cell().Element(CellStyle).AlignCenter().Text("1000000000000000000").FontSize(11);
                                table.Cell().Element(CellStyle).AlignCenter().Text("1000000000000000000").FontSize(11);
                                table.Cell().Element(CellStyle).AlignCenter().Text("UNPAID").FontSize(11);
                            }
                        });

                    });
                    pdfpage.Footer().PaddingTop(10).Row(footerelement =>
                    {
                        footerelement.RelativeItem(3).AlignCenter().Element(text =>
                        {
                            text.Text("This is an computer generate report if you have any inconvinience please contact the administration").FontSize(11).FontColor(Colors.Grey.Darken2);
                        });
                        footerelement.RelativeItem(1).AlignRight().Text("1").FontSize(11).FontColor(Colors.Grey.Darken2);
                    });
                });
                
            
            });

            return File(pdfDocument.GeneratePdf(), "application/pdf", "GeneratedPDF.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }

    }
    
}
