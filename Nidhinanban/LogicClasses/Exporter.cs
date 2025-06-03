using System.Collections.Generic;
using System;
using System.Data;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using ClosedXML.Excel;

namespace Nidhinanban.LogicClasses
{
    public class Exporter
    {
        public static byte[] ExportInterestViewExcel(string interestamount,string principleamount,string totalAmount,DataTable table, string sheetName = "Sheet1")
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(sheetName);
        worksheet.Cell("A1").Value="Interest Amount:"+interestamount;
        worksheet.Cell("A1").Style.Font.Bold=true;
        worksheet.Cell("A1").Style.Font.FontSize=12;
    
        worksheet.Cell("B1").Value="Principle Amount:"+principleamount;
        worksheet.Cell("B1").Style.Font.Bold=true;
        worksheet.Cell("B1").Style.Font.FontSize=12;

        worksheet.Cell("C1").Value="Total Amount:"+totalAmount;
        worksheet.Cell("C1").Style.Font.Bold=true;
        worksheet.Cell("C1").Style.Font.FontSize=12;

        worksheet.Cell(3,1).InsertTable(table, true);
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }   
    }
}
