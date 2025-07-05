using System.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Nidhinanban.Models;

public class InterestModel{
    [Required(ErrorMessage = "Principle Amount Required")]
    [Range(1,9999999999999999999,ErrorMessage ="Principle Amount Required")]
    public float principleamount{get; set;}
    [Required(ErrorMessage = "Interest Rate is required")]
    [Range(0.1, 100, ErrorMessage = "Enter a valid interest rate between 0.1 and 100")]
    public float interestrate{get; set;}
    public float tenure {get; set;}
    public  bool showdiv{get; set;}=false;

    public string? intrestamount{get; set;}=string.Empty;
    public string? totalamount{get; set;}
    public string? principleamountstring{get; set;}=string.Empty;

    public string type{get; set;}=string.Empty;
    public DataTable dt=new DataTable();

}
public class TestApiModel
{
    public float principalAmount { get; set; }
    public float interestRate { get; set; }
    public float tenure { get; set; }
    public string type { get; set; } = string.Empty;
}

public class InterestTableAPIModel
{
    public int sno { get; set; }
    public string date { get; set; } = string.Empty;
    public float interestrate { get; set; }
    public float interestamount { get; set; }
    public float principalAmount { get; set; }
    public float totalAmount{ get; set; }
}