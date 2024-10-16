using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary
{
    internal interface IOrderService
    {
        public int GetOrderID(int CustomerID);
        public decimal CalculateTotalAmount(int CustomerID);
        public bool UpdateOrderStatus(int OrderID, string newStatus);
        public bool CancelOrder(int OrderID);
        public bool AddOrder(int customerID);
    }
}