using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;

namespace Nidhinanban.Controllers
{



    public class IntrestController : Controller
    {

        public IntrestController()
        {

        }
        [HttpGet]
        public IActionResult Intrestcalc()
        {
            return View(new InterestModel());
        }

        [HttpPost]
        public IActionResult Intrestcalc(InterestModel Model)
        {
            if(ModelState.IsValid)
            {
                Model.showdiv=true;
                float interest=Model.interestrate;
                float principleamount=Model.principleamount;
                float year=Model.year;
                float interestamountnumber=(principleamount*interest*year)/100;
                Model.intrestamount="₹ "+(interestamountnumber.ToString());
                Model.totalamount="₹"+ (principleamount+interestamountnumber);
                
            }
            return View(Model);
        }
    }
}
