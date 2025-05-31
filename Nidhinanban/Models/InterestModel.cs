using System.Data;

namespace Nidhinanban.Models;

public class InterestModel{
    public float principleamount{get; set;}
    public float interestrate{get; set;}
    public float year{get; set;}
    public float month{get; set;}

    public float week{get; set;}
    public  bool showdiv{get; set;}=false;

    public string? intrestamount{get; set;}=string.Empty;
    public string? totalamount{get; set;}
    public string? principleamountstring{get; set;}=string.Empty;

    public string type{get; set;}=string.Empty;
    public List<string> dt=new List<string>();

}