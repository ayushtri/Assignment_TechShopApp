using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary
{
    internal interface IProductService
    {
        public int GetProductID(string ProductName);
        public Tuple<int, string, string, double, int> GetProductDetails(int ProductID);
        public bool UpdateProductInfo(int ProductID, string ProductName, string Description, double Price, int CategoryID);
        public bool IsProductInStock(int ProductID);
        public bool AddProduct(string productName, string description, decimal price);
    }
}
