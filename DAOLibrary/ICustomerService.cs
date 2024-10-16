using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary
{
    internal interface ICustomerService
    {
        public bool AddCustomer(string FirstName, string LastName, string Email, long Phone, string Address);
        public int GetCustomerID(string Email);
        public int CalculateTotalOrders(int CustomerID);
        public Tuple<int, string, string, string, string, string, int> GetCustomerDetails(int CustomerID);
        public bool UpdateCustomerInfo(int CustomerID, string FirstName, string LastName, string Email, long Phone, string Address);
    }
}
