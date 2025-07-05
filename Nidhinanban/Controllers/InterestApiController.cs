using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nidhinanban.Models;
namespace Nidhinanban.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InterestApiController : ControllerBase
{
    [HttpPost("calculateInterest")]
    public async Task<IActionResult> calculateInterest([FromBody] TestApiModel testApiModel)
    {
        if (testApiModel == null || testApiModel.principalAmount <= 0 || testApiModel.interestRate <= 0 || testApiModel.tenure <= 0)
        {
            return BadRequest("Invalid input data.");
        }
        else
        {
            float interest = 0;
            float totalAmount = 0;
            float principalAmount = (float)testApiModel.principalAmount;
            float interestRate = (float)testApiModel.interestRate;
            float month = (float)testApiModel.tenure;
            string type = testApiModel.type.ToLower();
            await Task.Run(() =>
            {
                if (type.ToLower() == "week")
                {
                    interest = (float)Math.Round(principalAmount * interestRate * month / (100 * 52), 2);
                    totalAmount = (float)Math.Round(principalAmount + interest, 2);
                }
                else if (type.ToLower() == "month")
                {
                    float singlemonthamount = principalAmount / month;
                    interest = (float)Math.Round(principalAmount * interestRate * month / 100, 2);
                    totalAmount = (float)Math.Round(principalAmount + interest, 2);
                }
                else if (type.ToLower() == "year")
                {
                    interest = (float)Math.Round(principalAmount * interestRate * month / 100, 2);
                    totalAmount = (float)Math.Round(principalAmount + interest, 2);
                }
            });
            return Ok(new
            {
                PrincipalAmount = $"{testApiModel.principalAmount:F2}",
                InterestAmount = $"{ interest:F2}",
                TotalAmount = $"{totalAmount:F2}"
            });
        }

    }


    [HttpPost("listInterestTable")]
    public async Task<IActionResult> postweektable([FromBody] TestApiModel testApiModel)
    {
        List<string> weeksdatas = new List<string>();
        DateTime startingdate = DateTime.Now;
        string type = testApiModel.type;
        float interestrate = (float)testApiModel.interestRate;
        float principal = (float)testApiModel.principalAmount;
        if (type.ToLower() == "week")
        {
            //task that calculates the interest and returns the data of the weeks
            if (interestrate <= 0 || principal <= 0 || testApiModel.tenure <= 0)
            {
                return BadRequest("Invalid input data. Ensure interest rate, principal amount, and month are greater than zero.");
            }
            
            await Task.Run(() =>
            {
                float numberOfWeeks = (float)testApiModel.tenure;
                float totalInterest = (principal * interestrate * numberOfWeeks) / (100 * 52); // simple weekly interest
                totalInterest = (float)Math.Round(totalInterest, 2);

                float totalAmount = principal + totalInterest;

                float weeklyPrincipal = (float)Math.Floor((principal / numberOfWeeks) * 100) / 100f;
                float weeklyInterest = (float)Math.Floor((totalInterest / numberOfWeeks) * 100) / 100f;

                float weeklyPayment = weeklyPrincipal + weeklyInterest;

                // Track how much was distributed
                float distributedPrincipal = 0f;
                float distributedInterest = 0f;

                for (int i = 1; i <= (int)Math.Floor(numberOfWeeks); i++)
                {
                    DateTime endDate = startingdate.AddDays(i * 7);

                    float thisPrincipal = weeklyPrincipal;
                    float thisInterest = weeklyInterest;

                    distributedPrincipal += thisPrincipal;
                    distributedInterest += thisInterest;

                    weeksdatas.Add($"{i}|{endDate:dd-MM-yyyy}|{interestrate}|{thisInterest}|{thisPrincipal}|{thisPrincipal + thisInterest}");
                }

                // Fix for rounding errors in final week
                float remainingPrincipal = (float)Math.Round(principal - distributedPrincipal, 2);
                float remainingInterest = (float)Math.Round(totalInterest - distributedInterest, 2);

                if (remainingPrincipal != 0 || remainingInterest != 0)
                {
                    int lastIndex = weeksdatas.Count - 1;
                    string[] parts = weeksdatas[lastIndex].Split('|');

                    float lastPrincipal = float.Parse(parts[4]) + remainingPrincipal;
                    float lastInterest = float.Parse(parts[3]) + remainingInterest;
                    float lastTotal = lastPrincipal + lastInterest;
                    // Update the last entry with the remaining amounts
                    weeksdatas[lastIndex] = $"{parts[0]}|{parts[1]}|{interestrate}|{lastInterest:F2}|{lastPrincipal:F2}|{lastTotal:F2}";
                }
            });

        }
        else if (type == "month")
        {
            if (interestrate <= 0 || principal <= 0 || testApiModel.tenure <= 0)
            {
                return BadRequest("Invalid input data. Ensure interest rate, principal amount, and month are greater than zero.");
            }
            //task that calculates the interest and returns the data of the months
            await Task.Run(() =>
            {
                float numberOfMonths = (float)testApiModel.tenure;


                // Monthly principal payment
                float monthlyPrincipal = principal / numberOfMonths;

                // Monthly interest amount (flat)
                float monthlyInterest = principal * interestrate / 100f;

                // Total monthly payment
                float monthlyTotal = monthlyPrincipal + monthlyInterest;

                for (int i = 1; i <= numberOfMonths; i++)
                {
                    DateTime paymentDate = startingdate.AddMonths(i);
                    weeksdatas.Add($"{i}|{paymentDate:dd-MM-yyyy}|{interestrate}|{monthlyInterest}|{monthlyPrincipal}|{monthlyTotal}");
                }
            });
        }
        else if (type == "year")
        {
            if (interestrate <= 0 || principal <= 0 || testApiModel.tenure <= 0)
            {
                return BadRequest("Invalid input data. Ensure interest rate, principal amount, and year are greater than zero.");
            }
            //task that calculates the interest and returns the data of the years
            await Task.Run(() =>
           {
               float numberOfYears = (float)testApiModel.tenure;
               // Full years only
               int fullYears = (int)Math.Floor(numberOfYears);
               float remainingFraction = numberOfYears - fullYears;
               // Yearly principal payment
               float yearlyPrincipal = principal / numberOfYears;
               for (int i = 1; i <= fullYears; i++)
               {
                   float yearlyInterest = principal * interestrate / 100f;
                   float yearlyTotal = yearlyPrincipal + yearlyInterest;
                   DateTime paymentDate = startingdate.AddYears(i);
                   weeksdatas.Add($"{i}|{paymentDate:dd-MM-yyyy}|{interestrate}|{yearlyInterest}|{yearlyPrincipal}|{yearlyTotal}");
               }
               // Handle remaining partial year (e.g., 0.5 year)
               if (remainingFraction > 0)
               {
                   /* 1) calculate the partial principal and interest for the remaining fraction of the year
                      2) Calculate the principal for the partial year*/
                   float partialPrincipal = yearlyPrincipal * remainingFraction;
                   /* 1) Calculate interest for the partial year
                      2) Assuming the interest is calculated on the full principal for the remaining fraction
                      3) This is a common approach for flat interest calculations*/
                   float partialInterest = principal * interestrate / 100f * remainingFraction;
                   // 1) Total for the partial year
                   float partialTotal = partialPrincipal + partialInterest;
                   // 1) this one add the remaining fraction of the year to the last year
                   DateTime partialDate = startingdate.AddYears(fullYears).AddMonths((int)(remainingFraction * 12));
                   weeksdatas.Add($"{fullYears + 1}|{partialDate:dd-MM-yyyy}|{interestrate}|{partialInterest}|{partialPrincipal}|{partialTotal}");
               }
           });
        }
        // 1) convert the weeksdatas to a list of InterestTableAPIModel
        if (weeksdatas.Count == 0)
        {
            return NotFound("No data available for the specified parameters.");
        }
        var table = new List<InterestTableAPIModel>();
        foreach (string row in weeksdatas)
        {
            string[] parts = row.Split('|');
            table.Add(new InterestTableAPIModel
            {
                sno = int.Parse(parts[0]),
                date = parts[1],
                interestrate = float.Parse(parts[2]),
                interestamount = float.Parse(parts[3]),
                principalAmount = float.Parse(parts[4]),
                totalAmount = float.Parse(parts[5])
            }
            );
        }
        return Ok(table);
    }

}

