using System;
using MySqlConnector;
using Nidhinanban.LogicClasses;


namespace Nidhinanban.Services
{
    public class AddCustomerService
    {
        private readonly IConfiguration _configuration;
        public static string? connectionstring {get; set;}
        public AddCustomerService(IConfiguration Configuration)
        {
            _configuration = Configuration;
            connectionstring = _configuration.GetConnectionString("DefaultConnection");
        }
         
       
        MySqlConnection con = new MySqlConnection(connectionstring);
        
        MySqlCommand commandstring = new MySqlCommand();
        public async Task<string> AddCustomer(string customerName, string CustomerPhonenumber, string CustomerAddress, string CustomerProfileimage, string CustomerAadharimage, string CusomerPancardimage, string CustomerHouseimage)
        {
            
            string result = "";
            try
            {
                ImageManipulation manipulation=new ImageManipulation();
                byte[] CustomerProfileimagebyte=manipulation.ConvertBase64ImageToBlob(CustomerProfileimage);          
                await con.OpenAsync();
                commandstring.Connection = con;
                commandstring.CommandText = "Show Tables";
                MySqlDataReader dr = await commandstring.ExecuteReaderAsync();
                if (dr.HasRows)
                {
                    await dr.ReadAsync();
                    result = dr[0].ToString()!;
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                await con.CloseAsync();
            }
            return result;

        }
        public async Task<string> AddCustomer()
        {
            string result=string.Empty;
            try
            {
                await con.OpenAsync();
                commandstring.Connection = con;
                commandstring.CommandText = "Show Tables";
                MySqlDataReader dr = await commandstring.ExecuteReaderAsync();
                if (dr.HasRows)
                {
                    await dr.ReadAsync();
                    result = dr[0].ToString()!;
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                await con.CloseAsync();
            }
            return result;

        }
    }
}
