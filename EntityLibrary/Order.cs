using ExceptionLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLibrary;

namespace EntityLibrary
{
    public class Order
    {
        private int _OrderID;
        private Customer _Customer;
        private DateTime _OrderDate;
        private decimal _TotalAmount;
        private string _Status;

        public Order(Customer Customer, DateTime OrderDate, decimal TotalAmount)
        {
            this.Customer = Customer;
            this.OrderDate = OrderDate;
            this.TotalAmount = TotalAmount;
        }

        public int OrderID 
        {
            get { return _OrderID; } 
        }

        public Customer Customer
        {
            get { return this._Customer; }
            set
            {
                if(value == null) throw new IncompleteOrderException("Customer can't ne null");
                else this._Customer = value; 
            }
        }

        public DateTime OrderDate
        {
            get { return this._OrderDate; }
            set { this._OrderDate = value; }
        }

        public decimal TotalAmount
        {
            get { return this._TotalAmount; }
            set { this._TotalAmount = value; }
        }

        public string Status
        {
            get { return this._Status; }
            set 
            {
                this._Status = value;
            }
        }


    }
    public class OrderOperations
    {
        public bool CancelOrder(int OrderID)
        {
            return new OrderDAO().CancelOrder(OrderID);
        }

        public int GetOrderID(int CustomerID)
        {
            return new OrderDAO().GetOrderID(CustomerID);
        }

        public decimal CalculateTotalAmount(int CustomerID)
        {
            return new OrderDAO().CalculateTotalAmount(CustomerID);
        }

        public bool UpdateOrderInfo(Order o)
        {
            int OrderID = o.OrderID;
            return new OrderDAO().UpdateOrderInfo(OrderID, o.OrderDate, o.TotalAmount, o.Status);
        }

        public bool UpdateOrderStatus(int OrderID, string Status)
        {
            return new OrderDAO().UpdateOrderStatus(OrderID, Status);
        }

        public bool AddOrder(int CustomerID)
        {
            return new OrderDAO().AddOrder(CustomerID);
        }

        public string GetOrderStatus(int OrderID)
        {
            return new OrderDAO().GetOrderStatus(OrderID);
        }



        public bool UpdateAllOrderTotals()
        {
            return new OrderDAO().UpdateAllOrderTotals();
        }



    }

        
}
