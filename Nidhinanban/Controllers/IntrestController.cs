using System.Security.Principal;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Nidhinanban.Models;
using Nidhinanban.Services;
using Newtonsoft.Json;
using Nidhinanban.LogicClasses;

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
                float tenure=interestModel.tenure;
                string type=interestModel.type;
                if(principleamount!=0)
                {
                        interestModel.showdiv=true;
                }
                else{
                        interestModel.showdiv=false;
                }
                if(interestrate==0)
                {
                    interestrate=1;
                }
                var status= await _interestService.calculateinterest(principleamount,interestrate,tenure,type);
                interestModel.principleamountstring=status.principleamount;
                interestModel.intrestamount=status.interestamount;
                interestModel.totalamount=status.totalamount;
                interestModel.dt=await intrest.getWeekbasedInterestData(tenure,interestrate.ToString(),principleamount.ToString(),type);
                TempData["ScheduleData"] = JsonConvert.SerializeObject(interestModel.dt);
                TempData["IntrestAmount"]=JsonConvert.SerializeObject(status.interestamount);
                TempData["Principleamount"]=JsonConvert.SerializeObject(status.principleamount);
                TempData["TotalAmount"]=JsonConvert.SerializeObject(status.totalamount);
                
            }
            return PartialView("_Intresthidden",interestModel);
        }
        public IActionResult ExportExcel()
        {
            string? json=TempData["ScheduleData"]?.ToString();
            string? interestamount=TempData["IntrestAmount"]?.ToString();
            string? principleamount=TempData["Principleamount"]?.ToString();
            string? totalamount=TempData["TotalAmount"]?.ToString();
            string result=String.IsNullOrEmpty(json) ?"":json;
            var dataTable = JsonConvert.DeserializeObject<DataTable>(result);
            var fileBytes = Exporter.ExportInterestViewExcel(interestamount!,principleamount!,totalamount!,dataTable!);
            string filename="Interest Report "+DateTime.Now.ToString("");
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
    }
}
