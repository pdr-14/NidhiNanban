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
        [Required(ErrorMessage = "Please Select The Customer Profile Image")]
        public IFormFile? CustomerProfileimage { get; set; }
        [Required(ErrorMessage="Please Select The Customer House Image")]
        public IFormFile? CustomerHouseimage { get; set; }
        [Required(ErrorMessage ="Please Select The Customer Aadhaar Image")]
        public IFormFile? CustomerAadharimage { get; set; }
        [Required(ErrorMessage ="Please Select The Customer Pan Card Image")]
        public IFormFile? CustomerPancardimage { get; set; }

        public bool success{get; set;}=false;
        
    }
}
