using System;
using System.Data;
using System.Linq;
using MySqlConnector;
using Nidhinanban.Models;

namespace Nidhinanban.Services
{
    public class ViewCustomerService
    {
        private readonly List<ViewCustomer> _viewCustomers=new();
        private int _id=1;
        public async Task<List<ViewCustomer>> getall()
        {
            int _id=0;
            var data=new DataTable();
            await Task.Run(() =>
            {
                data.Columns.Add("ID", typeof(int));
                data.Columns.Add("Name", typeof(string));
                for (int i = 0; i < 10;)
                {
                    data.Rows.Add(new Object[] { _id++, "Demo" });
                }
            });
            var n=data.AsEnumerable().Select(row=>new ViewCustomer{
                Id=row.Field<int>("ID"),
                Text=row.Field<string>("Name")!  
            }).ToList();
            return n;
        }

        public string getsatus(string id)
        {
            return "Hi "+id;
        }
    }
}
