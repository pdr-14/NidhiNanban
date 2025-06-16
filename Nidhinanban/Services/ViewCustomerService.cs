using MySqlConnector;
using System;
using System.Collections.Generic;
using Nidhinanban.Models;

namespace Nidhinanban.Services
{
    public class ViewCustomerService
    {
       
        private readonly IConfiguration _configuration;
        public static string? connectionstring {get; set;}
        public ViewCustomerService(IConfiguration Configuration)
        {
            _configuration = Configuration;
            connectionstring = _configuration.GetConnectionString("DefaultConnection");
        }
         
       
        
        public async Task<List<ViewCustomer>> getallcustomer()
        {
            MySqlConnection connection = new MySqlConnection(connectionstring);
            MySqlCommand commandstring = new MySqlCommand();
            var customerslist=new List<ViewCustomer>();
            try
            {
                await connection.OpenAsync();
                commandstring.Connection = connection;
                commandstring.CommandText = "select * from CUSTOMER limit 12 offset 0";
                MySqlDataReader dr = await commandstring.ExecuteReaderAsync();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        customerslist.Add(new ViewCustomer
                        {
                            customername = dr[0].ToString(),
                            cutomerphonenumber = dr[1].ToString(),
                            customeraddress = dr[2].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                await connection.CloseAsync();
            }
            return customerslist;
        }

    }
}
