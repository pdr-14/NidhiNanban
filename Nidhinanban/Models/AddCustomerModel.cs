using System.Data;
using System.ComponentModel.DataAnnotations;

namespace Nidhinanban.Models
{
    public class AddCustomerModel
    {
        
        [Required(ErrorMessage = "Enter The Customer Name")]
        public string? CustomerName { get; set; } =string.Empty;
        [Required(ErrorMessage="Enter The Customer Phone Number")]
        public string? CustomerPhonenumber { get; set; } = string.Empty;
        [Required(ErrorMessage="Enter The Customer Address")]
        public string? CustomerAddress { get; set; } = string.Empty;
    
        public string? CustomerProfileimage { get; set; } = string.Empty;

        public string? CustomerHouseimage { get; set; } = string.Empty;

        public string? CustomerAadharimage { get; set; } = string.Empty;

        public string? CusomerPancardimage { get; set; } = string.Empty;

        public bool success{get; set;}=false;
        
    }
}
