using Microsoft.Extensions.Diagnostics;


namespace Nidhinanban.Services
{
    public class InterestService() {
        public async Task< (string principleamount,string interestamount,string totalamount,List<string> weekstable)> calculateinterest(float principleamount,float interest,float year,float month,float week,string type){
            float principleamounts=principleamount;
            float interestamounts=0f;
            float totalamounts=0f;
            List<string> datas=new List<string>();
            if(type.ToLower()=="week")
            {
                interestamounts=(float) Math.Round(principleamount*interest*week/(100*52),2);
                totalamounts=(float) Math.Round(principleamounts+interestamounts,2);
                
            await Task.Run(() =>
            {
                DateTime startingdate = DateTime.Now;
                int numberofweeks = 4;
                
                for (int i = 0; i < numberofweeks; i++)
                {
                    DateTime enddate = startingdate.AddDays(i * 7);
                    datas.Add(enddate.ToString("dd-MM-yyyy"));
                }
            });
            }
            else if(type.ToLower()=="month")
            {
                interestamounts=(float) Math.Round(principleamount*interest*month/(100*12),2);
                totalamounts=(float) Math.Round(principleamounts+interestamounts,2);
            }
            else if(type.ToLower()=="year")
            {
                interestamounts=(float) Math.Round(principleamount*interest*year/100,2);
                totalamounts= (float) Math.Round(principleamounts+interestamounts,2);
            }
            return("₹"+principleamounts,"₹"+interestamounts,"₹"+totalamounts,datas);
        }
    }
}