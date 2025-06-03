using System.Data;
using System.ComponentModel.DataAnnotations;

namespace Nidhinanban.Models;

public class InterestModel{
    [Required(ErrorMessage = "Principle Amount Required")]
    [Range(1,9999999999999999999,ErrorMessage ="Principle Amount Required")]
    public float principleamount{get; set;}
    [Required(ErrorMessage = "Interest Rate is required")]
    [Range(0.1, 100, ErrorMessage = "Enter a valid interest rate between 0.1 and 100")]
    public float interestrate{get; set;}
    public float year{get; set;}
    public float month{get; set;}

    public float week{get; set;}
    public  bool showdiv{get; set;}=false;

    public string? intrestamount{get; set;}=string.Empty;
    public string? totalamount{get; set;}
    public string? principleamountstring{get; set;}=string.Empty;

    public string type{get; set;}=string.Empty;
    public DataTable dt=new DataTable();

}