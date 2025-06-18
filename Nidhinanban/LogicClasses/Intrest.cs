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
        public async Task<DataTable> getWeekbasedInterestData(float tenure ,string interestrate, string principleamount, string type)
        {
            List<string> weeksdatas = new List<string>();
            DateTime startingdate = DateTime.Now;
            if (type.ToLower() == "week")
            {
                await Task.Run(() =>
                {
                        float interestratePercent = float.TryParse(interestrate, out float ir) ? ir : 0f;
                        float principal = float.TryParse(principleamount, out float pa) ? pa : 0f;
                        float numberOfWeeks = tenure;

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
            else if (type == "month")
            {
                await Task.Run(() =>
                {
                    float numberOfMonths = tenure;

                    // Parse interest rate
                    float interestRate = float.TryParse(interestrate, out float parsedInterestRate) ? parsedInterestRate : 0f;

                    // Parse principal amount
                    float principalAmount = float.TryParse(principleamount, out float parsedPrincipal) ? parsedPrincipal : 0f;

                    // Monthly principal payment
                    float monthlyPrincipal = principalAmount / numberOfMonths;

                    // Monthly interest amount (flat)
                    float monthlyInterest = principalAmount * interestRate / 100f;

                    // Total monthly payment
                    float monthlyTotal = monthlyPrincipal + monthlyInterest;

                    for (int i = 1; i <= numberOfMonths; i++)
                    {
                        DateTime paymentDate = startingdate.AddMonths(i);
                        weeksdatas.Add($"{i}|{paymentDate:dd-MM-yyyy}|{interestRate}|{monthlyInterest}|{monthlyPrincipal}|{monthlyTotal}");
                    }
                });
            }
            else if (type == "year")
            {
                     await Task.Run(() =>
                    {
                            float numberOfYears = tenure;
                            // Parse interest rate
                            float interestRate = float.TryParse(interestrate, out float parsedInterestRate) ? parsedInterestRate : 0f;
                            // Parse principal amount
                            float principalAmount = float.TryParse(principleamount, out float parsedPrincipal) ? parsedPrincipal : 0f;
                             // Full years only
                            int fullYears = (int)Math.Floor(numberOfYears);
                            float remainingFraction = numberOfYears - fullYears;
                            // Yearly principal payment
                            float yearlyPrincipal = principalAmount / numberOfYears;
                            for (int i = 1; i <= fullYears; i++)
                            {
                                    float yearlyInterest = principalAmount * interestRate / 100f;
                                    float yearlyTotal = yearlyPrincipal + yearlyInterest;
                                    DateTime paymentDate = startingdate.AddYears(i);
                                    weeksdatas.Add($"{i}|{paymentDate:dd-MM-yyyy}|{interestRate}|{yearlyInterest}|{yearlyPrincipal}|{yearlyTotal}");
                            }
                            // Handle remaining partial year (e.g., 0.5 year)
                            if (remainingFraction > 0)
                            {
                                    float partialPrincipal = yearlyPrincipal * remainingFraction;
                                    float partialInterest = principalAmount * interestRate / 100f * remainingFraction;
                                    float partialTotal = partialPrincipal + partialInterest;
                                    DateTime partialDate = startingdate.AddYears(fullYears).AddMonths((int)(remainingFraction * 12));
                                    weeksdatas.Add($"{fullYears + 1}|{partialDate:dd-MM-yyyy}|{interestRate}|{partialInterest}|{partialPrincipal}|{partialTotal}");
                         }
                    });
            }


    DataTable table = new DataTable();
    table.Columns.Add("WeekOrMonth", typeof(int));
    table.Columns.Add("Date", typeof(string));
    table.Columns.Add("InterestRate", typeof(float));
    table.Columns.Add("InterestAmount", typeof(float));
    table.Columns.Add("PrincipalAmount", typeof(float));
    table.Columns.Add("TotalPayment", typeof(float));

    foreach (string row in weeksdatas)
    {
        string[] parts = row.Split('|');
        table.Rows.Add(
            int.Parse(parts[0]),
            parts[1],
            float.Parse(parts[2]),
            float.Parse(parts[3]),
            float.Parse(parts[4]),
            float.Parse(parts[5])
        );
    }

    return table;
        }
    }
}
