using Microsoft.VisualBasic;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionLibrary;
using DAOLibrary;

namespace EntityLibrary
{
    public class Inventory
    {
        //private int _InventoryID;
        private Product _Product;
        private int _QuantityInStock;
        private DateTime _LastStockUpdate;

        public Inventory(Product Product, int QuantityInStock, DateTime LastStockUpdate)
        {
            this.Product = Product;
            this.QuantityInStock = QuantityInStock;
            this.LastStockUpdate = LastStockUpdate;
        }

        /*public int InventoryID 
        {
            get { return _InventoryID; }
        }*/

        public Product Product
        { 
            get { return _Product; } 
            set { _Product = value; }
        }

        public int QuantityInStock
        {
            get { return _QuantityInStock; }
            set 
            {
                if(value < 0)
                {
                    throw new InsufficientStockException("Quantity can't be negative");
                }
                else _QuantityInStock = value; 
            }
        }

        public DateTime LastStockUpdate
        {
            get { return _LastStockUpdate; }
            set { _LastStockUpdate = value; }
        }

    }

    public class InventoryOperations
    {
        public int GetInventoryID(int ProductID)
        {
            return new InventoryDAO().GetInventoryID(ProductID);
        }
        public bool AddToInventory(int ProductID, int Quantity)
        {
            return new InventoryDAO().AddToInventory(ProductID, Quantity);
        }
        public bool RemoveFromInventory(int ProductID, int Quantity)
        {
            return new InventoryDAO().RemoveFromInventory(ProductID, Quantity);
        }
        public bool UpdateStockQuantity(int ProductID, int NewQuantity)
        {
            return new InventoryDAO().UpdateStockQuantity(ProductID, NewQuantity);
        }
        public bool IsProductAvailable(int ProductID, int QuantityToCheck)
        {
            return new InventoryDAO().IsProductAvailable(ProductID, QuantityToCheck);
        }
        public int GetInventoryValue(int ProductID)
        {
            return new InventoryDAO().GetInventoryValue(ProductID);
        }
        public List<Tuple<int, string, int>> ListLowStockProducts(int threshold)
        {
            return new InventoryDAO().ListLowStockProducts(threshold);
        }
        public List<Tuple<int, string, int>> ListAllProducts()
        {
            return new InventoryDAO().ListAllProducts();
        }

    }
}
