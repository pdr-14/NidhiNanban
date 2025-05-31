using System.Collections.Generic;
using System;
using System.Data;

namespace Nidhinanban.LogicClasses
{
    public class Intrest
    {
        public async Task<List<string>> getWeekbasedInterestData()
        {
            List<string> weeksdatas=new List<string>();
            await Task.Run(() =>
            {
                DateTime startingdate = DateTime.Now;
                int numberofweeks = 4;
                
                for (int i = 0; i < numberofweeks; i++)
                {
                    DateTime enddate = startingdate.AddDays(i * 7);
                    weeksdatas.Add(enddate.ToString("dd-MM-yyyy"));
                }
            });
            return weeksdatas;
        }
    }
}
