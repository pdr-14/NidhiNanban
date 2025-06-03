using System;
using System.Data;
using Nidhinanban.Models;

namespace Nidhinanban.Services
{
    public class ViewCustomerService
    {
        private readonly List<ViewCustomer> _viewCustomers=new();
        private int _id=1;
        public List<string> getall()
        {
            var newcustomer=new List<string>();
            for (int i=0; i<10;i++)
            {
                newcustomer.Add(_id++.ToString());
            }
            _id=1;
            return newcustomer;
        }

        public string getsatus(string id)
        {
            return "Hi "+id;
        }
    }
}
