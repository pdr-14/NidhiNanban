using Microsoft.Extensions.Diagnostics;


namespace Nidhinanban.Services
{
    public class InterestService()
    {
        public async Task<(string principleamount, string interestamount, string totalamount, List<string> weekstable)> calculateinterest(float principleamount, float interest, float tenure, string type)
        {
            float principleamounts = principleamount;
            float interestamounts = 0f;
            float totalamounts = 0f;
            List<string> datas = new List<string>();
            await Task.Run(() =>
            {
                if (type.ToLower() == "week")
                {
                    interestamounts = (float)Math.Round(principleamount * interest * tenure / (100 * 52), 2);
                    totalamounts = (float)Math.Round(principleamounts + interestamounts, 2);
                   // return ("₹" + principleamounts, "₹" + interestamounts, "₹" + totalamounts, datas);
                }
                else if (type.ToLower() == "month")
                {
                    float singlemonthamount=principleamount/tenure;
                    interestamounts = (float)Math.Round((principleamount * interest *tenure  / 100), 2);
                    totalamounts = (float)Math.Round((principleamount + interestamounts), 2);
                    
                    //return ("₹" + principleamounts, "₹" + interestamounts*month, "₹" + totalamounts, datas);
                }
                else if (type.ToLower() == "year")
                {
                    interestamounts = (float)Math.Round(principleamount * interest * tenure / 100, 2);
                    totalamounts = (float)Math.Round(principleamounts + interestamounts, 2);
                   // return ("₹" + principleamounts, "₹" + interestamounts, "₹" + totalamounts, datas);
                }
                
            });
            return ("₹" + principleamounts, "₹" + interestamounts, "₹" + totalamounts, datas);
        }
    }
}