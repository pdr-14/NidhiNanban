using Microsoft.Extensions.Diagnostics;
using MySqlConnector;
using System.Data;


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
                    float singlemonthamount = principleamount / tenure;
                    interestamounts = (float)Math.Round(principleamount * interest * tenure / 100, 2);
                    totalamounts = (float)Math.Round(principleamount + interestamounts, 2);

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
    
    public class SaveIntoInterestTableservice
    {
        private readonly IConfigurationManager _configurationManager;
        public SaveIntoInterestTableservice(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }
        public async Task<string> InsertintoTable(DataTable dataTable)
        {

            string connectionstring = _configurationManager.GetConnectionString("Default Connection")!;
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand command = new MySqlCommand();
            string s = "";
            try
            {
                await connection.OpenAsync();
                command.Connection = connection;
                foreach (DataRow dataRowrow in dataTable.Rows)
                {
                    Console.WriteLine(dataRowrow[0].ToString());
                }
            }
            catch (MySqlException ex)
            {
                s = ex.Message;
            }
            finally
            {
                await connection.CloseAsync();
            }
            return s;
        }
    }
}