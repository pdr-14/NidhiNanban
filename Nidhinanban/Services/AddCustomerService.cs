using Internal;
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
        public async Task<string> AddCustomer(string customerName, string CustomerPhonenumber, string CustomerAddress, IFormFile CustomerProfileimage, IFormFile CustomerAadharimage, IFormFile CusomerPancardimage, IFormFile CustomerHouseimage)
        {
            
            string result = "";
            try
            {
                ImageManipulation manipulation=new ImageManipulation();
                await  manipulation.StoreImageToTheServer(customerName, CustomerProfileimage,"Profile");
                await manipulation.StoreImageToTheServer(customerName,CustomerHouseimage,"House");
                await manipulation.StoreImageToTheServer(customerName, CustomerAadharimage, "Aadhaar");
                await manipulation.StoreImageToTheServer(customerName, CusomerPancardimage,"PanCard");
                await con.OpenAsync();
                commandstring.Connection = con;
                commandstring.CommandText = "INSERT INTO CUSTOMER VALUE(@customername,@customerphonenumber,@customeraddress)";
                commandstring.Parameters.AddWithValue("@customername",customerName);
                commandstring.Parameters.AddWithValue("@customerphonenumber",CustomerPhonenumber);
                commandstring.Parameters.AddWithValue("@customeraddress",CustomerAddress);
                int rowcount=await commandstring.ExecuteNonQueryAsync();
                if(rowcount==0)
                {
                    return result="Error Code x0ENNRA"; //Error code "Error No New Row Added" => ENNRA (Expansion)
                }
                else if(rowcount>0)
                {
                    return result="Inserted";
                }
                else
                {
                    return result="Error Code xoUE"; //Error code "Unkown Error" => UE;
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
