using System.Collections.Generic;
using System;
using System.Data;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Nidhinanban.LogicClasses
{
    public class Intrest
    {
        public async Task<List<string>> getWeekbasedInterestData(float no_of_weeks, float no_of_months, string interestrate, string principleamount, string type)
        {
            List<string> weeksdatas = new List<string>();
            DateTime startingdate = DateTime.Now;
            if (type.ToLower() == "week")
            {
                await Task.Run(() =>
                {
                        float interestratePercent = float.TryParse(interestrate, out float ir) ? ir : 0f;
                        float principal = float.TryParse(principleamount, out float pa) ? pa : 0f;
                        float numberOfWeeks = no_of_weeks;

                        float totalInterest = (principal * interestratePercent * numberOfWeeks) / (100 * 52); // simple weekly interest
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

                            weeksdatas.Add($"{i}|{endDate:dd-MM-yyyy}|{interestratePercent}|{thisInterest}|{thisPrincipal}|{thisPrincipal + thisInterest}");
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

                            weeksdatas[lastIndex] = $"{parts[0]}|{parts[1]}|{interestratePercent}|{lastInterest:F2}|{lastPrincipal:F2}|{lastTotal:F2}";
                        }
                    });

            }
            else if (type == "month" )
            {
                await Task.Run(() =>
                {
                    float numberofmonths = no_of_months;
                    float interestamount = 0f;
                    float interestrates = 0f;
                    interestrates = float.TryParse(interestrate, out interestamount) ? interestamount : 0;
                    float totalamount = 0f;
                    float principleamounts = 0f;
                    //getting principleamount
                    principleamounts = float.TryParse(principleamount, out principleamounts) ? principleamounts : 0;

                    float monthlypayamount = principleamounts / numberofmonths;

                    float monthlyintrestamount = monthlypayamount * (interestrates / 100);

                    totalamount = monthlypayamount + monthlyintrestamount;
                    for (int i = 1; i <= numberofmonths; i++)
                    {
                        DateTime enddate = startingdate.AddMonths(i);
                        weeksdatas.Add(i + "|" + enddate.ToString("dd-MM-yyyy") + "|" + interestrate + "|" + monthlyintrestamount + "|" + monthlypayamount + "|" + totalamount);
                        Console.WriteLine(enddate.ToString(""));
                    }
                });
            }
            return weeksdatas;
        }
    }
}
