using Microsoft.Extensions.Diagnostics;


namespace Nidhinanban.Services
{
    public class InterestService()
    {
        public async Task<(string principleamount, string interestamount, string totalamount, List<string> weekstable)> calculateinterest(float principleamount, float interest, float year, float month, float week, string type)
        {
            float principleamounts = principleamount;
            float interestamounts = 0f;
            float totalamounts = 0f;
            Console.WriteLine(week);
            List<string> datas = new List<string>();
            await Task.Run(() =>
            {
                if (type.ToLower() == "week")
                {
                    interestamounts = (float)Math.Round(principleamount * interest * week / (100 * 52), 2);
                    totalamounts = (float)Math.Round(principleamounts + interestamounts, 2);
                }
                else if (type.ToLower() == "month")
                {
                    float singlemonthamount=principleamount/month;
                    interestamounts = (float)Math.Round((singlemonthamount * interest  / 100), 2);
                    totalamounts = (float)Math.Round((singlemonthamount + interestamounts)*month, 2);
                }
                else if (type.ToLower() == "year")
                {
                    interestamounts = (float)Math.Round(principleamount * interest * year / 100, 2);
                    totalamounts = (float)Math.Round(principleamounts + interestamounts, 2);
                }
            });
            return ("₹" + principleamounts, "₹" + interestamounts*month, "₹" + totalamounts, datas);
        }
    }
}