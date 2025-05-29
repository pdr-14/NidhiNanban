using Microsoft.Extensions.Diagnostics;


namespace Nidhinanban.Services
{
    public class InterestService() {
        public (string principleamount,string interestamount,string totalamount) calculateinterest(float principleamount,float interest,float year,float month,float week,string type){
            float principleamounts=principleamount;
            float interestamounts=0f;
            float totalamounts=0f;
            if(type.ToLower()=="week")
            {
                interestamounts=(float) Math.Round(principleamount*interest*week/(100*52),2);
                totalamounts=(float) Math.Round(principleamounts+interestamounts,2);
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
            return("₹"+principleamounts,"₹"+interestamounts,"₹"+totalamounts);
        }
    }
}