using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;
using Nidhinanban.Services;

namespace Nidhinanban.Controllers
{



    public class IntrestController : Controller
    {
        private readonly InterestService _interestService;
        public IntrestController(){
            _interestService = new InterestService();
        }
        [HttpGet]
        public IActionResult Intrestcalc()
        {
            return View(new InterestModel());
        }

        [HttpPost]
        public async Task<IActionResult> Intrestcalc(InterestModel interestModel)
        {
            if(ModelState.IsValid)
            {
                LogicClasses.Intrest intrest=new LogicClasses.Intrest();
                float principleamount=interestModel.principleamount;
                float interestrate=interestModel.interestrate;
                float year=interestModel.year;
                float month=interestModel.month;
                float week=interestModel.week;
                string type=interestModel.type;
                interestModel.showdiv=true;
                var status= await _interestService.calculateinterest(principleamount,interestrate,year,month,week,type);
                interestModel.principleamountstring=status.principleamount;
                interestModel.intrestamount=status.interestamount;
                interestModel.totalamount=status.totalamount;
                interestModel.dt=await intrest.getWeekbasedInterestData(week,month,interestrate.ToString(),principleamount.ToString(),type);
                
            }
            return PartialView("_Intresthidden",interestModel);
        }
    }
}
