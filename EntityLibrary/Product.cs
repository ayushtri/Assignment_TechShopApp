using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLibrary;
using ExceptionLibrary;

namespace EntityLibrary
{
    public class Product
    {  
        //private int _ProductID;
        private string _ProductName;
        private string _Description;
        private decimal _Price;
        private int _CategoryID;

        public Product(string ProductName, string Description, decimal Price, int CategoryID)
        {
            this.ProductName = ProductName;
            this.Description = Description;
            this.Price = Price;
            this.CategoryID = CategoryID;
        }

        /*public int ProductID
        {
            get { return _ProductID; }
        }*/

        public string ProductName
        {
            get { return _ProductName; }
            set 
            {
                if (value == null || value.Length == 0) 
                {
                    throw new ExceptionLibrary.InvalidDataException("Enter a valid Product Name");
                }
                else if (value.Length > 100) 
                {
                    throw new ExceptionLibrary.InvalidDataException("Product Name should be less than 100 characters");
                }
                else { _ProductName = value; }
            }
        }

        public string Description
        {
            get { return _Description; }

            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ExceptionLibrary.InvalidDataException("Enter a valid Product Description");
                }
                else if (value.Length > 500) 
                {
                    throw new ExceptionLibrary.InvalidDataException("Product Description should be less than 500 characters");
                }
                else { _Description = value; }
            }
        }

        public decimal Price
        {
            get { return _Price; }
            set
            {
                if (value < 0) 
                {
                    throw new ExceptionLibrary.InvalidDataException("Enter a valid Product Price");
                }
                else { _Price = value; }
            }
        }

        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

    }

    public class ProductOperations
    {
        public Tuple<int, string, string, double, int> GetProductDetails(int ProductID)
        {
            Tuple<int, string, string, double, int> product = null;
            ProductDAO productDAO = new ProductDAO();
            product = productDAO.GetProductDetails(ProductID);
            return product;
        }

        public int GetProductID(string ProductName)
        {
            return new ProductDAO().GetProductID(ProductName);
        }

        public bool IsProductInStock(int ProductID)
        {
            return new ProductDAO().IsProductInStock(ProductID);
        }

        /// <summary>
        /// ProductName will not be updated using this
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool UpdateProductInfo(Product p)
        {
            int ProductID = GetProductID(p.ProductName);
            return new ProductDAO().UpdateProductInfo(ProductID, p.ProductName, p.Description, Convert.ToDouble(p.Price), p.CategoryID);
        }

        /// <summary>
        /// This method will only update the name of the product
        /// </summary>
        /// <param name="ProductID"></param>
        /// <param name="newProductName"></param>
        /// <returns></returns>
        public bool UpdateProductName(int ProductID, string newProductName)
        {
            return new ProductDAO().UpdateProductName(ProductID, newProductName);
        }

        public void GetProducts() 
        {
            List < Tuple<int, string, string, decimal>> allProds = new ProductDAO().GetProducts();
            foreach (var product in allProds)
            {
                Console.WriteLine($"Product ID: {product.Item1}, Name: {product.Item2}, Description: {product.Item3}, Price: {product.Item4}");
            }
        }

        public bool AddProduct(Product p)
        {
            return new ProductDAO().AddProduct(p.ProductName, p.Description, p.Price);
        }
    }
}
