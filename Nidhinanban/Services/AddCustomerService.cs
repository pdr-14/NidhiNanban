using Internal;
using System;
using MySqlConnector;
using System.Linq;
using Nidhinanban.LogicClasses;


namespace Nidhinanban.Services
{
    public class AddCustomerService
    {
        private readonly IConfiguration _configuration;
        private ImageManipulation _imageManipulation;
        public static string? connectionstring { get; set; }
        public AddCustomerService(IConfiguration Configuration,ImageManipulation imageManipulation)
        {
            _configuration = Configuration;
            _imageManipulation = imageManipulation;
            connectionstring = _configuration.GetConnectionString("DefaultConnection");
            
        }
         
       
        MySqlConnection con = new MySqlConnection(connectionstring);
        
        MySqlCommand commandstring = new MySqlCommand();
        public async Task<string> AddCustomer(string CustomerId,string customerName, string CustomerPhonenumber, string CustomerAddress, IFormFile CustomerProfileimage, IFormFile CustomerAadharimage, IFormFile CusomerPancardimage, IFormFile CustomerHouseimage)
        {
            
            string result = "";
            try
            {
            
                await  _imageManipulation.StoreImageToTheServer(CustomerId, CustomerProfileimage,"Profile");
                await _imageManipulation.StoreImageToTheServer(CustomerId,CustomerHouseimage,"House");
                await _imageManipulation.StoreImageToTheServer(CustomerId, CustomerAadharimage, "Aadhaar");
                await _imageManipulation.StoreImageToTheServer(CustomerId, CusomerPancardimage,"PanCard");
                await con.OpenAsync();
                commandstring.Connection = con;
                commandstring.CommandText = "INSERT INTO CUSTOMER VALUE(@customerid,@customername,@customerphonenumber,@customeraddress)";
                commandstring.Parameters.AddWithValue("@customername",customerName);
                commandstring.Parameters.AddWithValue("@customerphonenumber",CustomerPhonenumber);
                commandstring.Parameters.AddWithValue("@customeraddress",CustomerAddress);
                commandstring.Parameters.AddWithValue("@customerid", CustomerId);
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

        public async Task<string> GetCustomerID()
        {
            string returnid = "";
            try
            {
                await con.CloseAsync();
                await con.OpenAsync();
                commandstring.Connection = con;
                commandstring.CommandText = "select customerid from CUSTOMER";
                MySqlDataReader dr = await commandstring.ExecuteReaderAsync();
                if (dr.HasRows)
                {
                    await dr.ReadAsync();
                    string id = dr[0].ToString()!;
                    //extract the string from the id like abc123 => abc
                    string extractedid = new String(id.TakeWhile(Char.IsLetter).ToArray());
                    //extract the number from the id like abc123 => 123
                    string extractednumber = new String(id.Where(x => Char.IsDigit(x)).ToArray());
                    int numberid = int.Parse(extractednumber);
                    //It increments the id and return
                    if (numberid < 10)
                    {
                        returnid = extractedid + "0" + (numberid + 1).ToString();
                    }
                    else
                    {
                        returnid = extractedid + (numberid + 1).ToString();    
                    }
                }
                else
                {
                    returnid = "USER01";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                await con.CloseAsync();
            }
            Console.WriteLine(returnid);
            return returnid;
        }
        public async Task<string> AddCustomer()
        {
            string result = string.Empty;
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
