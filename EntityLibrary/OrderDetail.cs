using DAOLibrary;
using ExceptionLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class OrderDetail
    {
        //private int _OrderDetailID;
        private Order _Order;
        private Product _Product;
        private int _Quantity;

        public OrderDetail(Order Order, Product Product, int Quantity)
        {
            this.Order = Order;
            this.Product = Product;
            this.Quantity = Quantity;
        }

        /*public int OrderDetailID
        {
            get { return _OrderDetailID; }
        }*/

        public Order Order
        {
            get { return _Order; }
            set { _Order = value; }
        }

        public Product Product
        { 
            get { return _Product; } 
            set 
            {
                if (value == null)
                {
                    throw new IncompleteOrderException("Product reference can't be null");
                }
                else _Product = value; 
            }
        }

        public int Quantity
        {
            get { return _Quantity; }
            set 
            { 
                if(value <= 0)
                {
                    throw new ExceptionLibrary.InvalidDataException("Quantity should be atleast 1");
                }
                else _Quantity = value;
            }
        }
    }

    public class OrderDetailOperations
    {
        public decimal CalculateSubtotal(int OrderDetailID)
        {
            return new OrderDetailDAO().CalculateSubtotal(OrderDetailID);
        }
        public Tuple<int, int, int, int> GetOrderDetailInfo(int OrderDetailID)
        {
            return new OrderDetailDAO().GetOrderDetailInfo(OrderDetailID);
        }
        public bool UpdateQuantity(int OrderDetailID, int NewQuanity)
        {
            return new OrderDetailDAO().UpdateQuantity(OrderDetailID, NewQuanity);
        }
        public bool AddDiscount(int OrderDetailID, int DiscountPercent)
        {
            return new OrderDetailDAO().AddDiscount(OrderDetailID, DiscountPercent);
        }
        public int GetOrderDetailID(int OrderID)
        {
            return new OrderDetailDAO().GetOrderDetailID(OrderID);
        }
        public bool AddOrderDetails(int orderID, int productID, int quantity)
        {
            return new OrderDetailDAO().AddOrderDetails(orderID, productID, quantity);
        }

        
    }
}
