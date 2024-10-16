using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary
{
    public class ProductDAO : IProductService
    {
        private SqlConnection sqlConnection = UtilLibrary.DBPropertyUtil.GetConnectionString();

        public Tuple<int, string, string, double, int> GetProductDetails(int ProductID)
        {
            SqlCommand cmd = new SqlCommand($"SELECT * FROM Products WHERE ProductID = {ProductID}", sqlConnection);
            Tuple<int, string, string, double, int> ProductTuple = null;

            sqlConnection.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                int ProdID = Convert.ToInt32(sqlDataReader[0]);
                string ProductName = sqlDataReader[1].ToString();
                string Description = sqlDataReader[2].ToString();
                double Price = Convert.ToDouble(sqlDataReader[3]);
                int CategoryID = Convert.ToInt32(sqlDataReader[4]);

                sqlDataReader.Close();

                ProductTuple = Tuple.Create<int, string, string, double, int>
                    (ProdID, ProductName, Description, Price, CategoryID);
            }

            sqlDataReader.Close();
            return ProductTuple;
        }

        public int GetProductID(string ProductName)
        {
            SqlCommand cmd = new SqlCommand($"SELECT ProductID FROM Products WHERE ProductName= '{ProductName}'", sqlConnection);
            sqlConnection.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            int ProductID;
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                ProductID = Convert.ToInt32(sqlDataReader[0]);
                sqlDataReader.Close();
                return ProductID;
            }
            return 0;
        }

        public bool IsProductInStock(int ProductID)
        {
            SqlCommand cmd = new SqlCommand($"SELECT QuantityInStock FROM  Inventory WHERE ProductID = @ProductID", sqlConnection);
            cmd.Parameters.AddWithValue("ProductID", ProductID);
            sqlConnection.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            int Stock;

            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();

                Stock = Convert.ToInt32(sqlDataReader[0]);
                sqlDataReader.Close();
                return Stock>0;
            }
            sqlDataReader.Close();
            Console.WriteLine("Invalid product ID: {0}. No Product found.", ProductID);
            throw new ExceptionLibrary.InvalidDataException("Invalid ProdID");
        }

        public bool UpdateProductInfo(int ProductID, string ProductName, string Description, double Price, int CategoryID)
        {
            SqlCommand cmd = new SqlCommand("[dbo].UpdateProduct", sqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("ProductID", ProductID);
            cmd.Parameters.AddWithValue("@ProductName", ProductName);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);

            sqlConnection.Open();
            cmd.ExecuteNonQuery();
            sqlConnection.Close();


            return true;
        }

        public bool UpdateProductName(int ProductID, string newProductName)
        {
            SqlCommand cmd = new SqlCommand($"UPDATE Products SET ProductName = '{newProductName}' WHERE ProductID = {newProductName}", sqlConnection);

            sqlConnection.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            return reader.HasRows;
        }

        public List<Tuple<int, string, string, decimal>> GetProducts()
        {
            string query = "SELECT ProductID, ProductName, Description, Price FROM Products";

            List<Tuple<int, string, string, decimal>> allProds = new List<Tuple<int, string, string, decimal>>();

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())

                {
                    while (reader.Read())
                    {
                        int productID = Convert.ToInt32(reader[0]);
                        string productName = reader.GetString(1);

                        string description = reader.GetString(2);
                        decimal price = reader.GetDecimal(3);

                        Tuple<int, string, string, decimal> product = Tuple.Create(productID, productName, description, price);
                        allProds.Add(product);
                    }
                }
            }

            return allProds;
        }

        public bool AddProduct(string productName, string description, decimal price)
        {
            if (string.IsNullOrEmpty(productName))
            {
                throw new ArgumentNullException(nameof(productName), "Product name is required.");
            }

            if (price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.");
            }

            int CategoryID = 1000001;

            string query = "INSERT INTO Products (ProductName, Description, Price, CategoryID) VALUES (@ProductName, @Description, @Price, @CategoryID)";

            sqlConnection.Open();

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    Console.WriteLine("Product added successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Error adding product.");
                    return false;
                }
            }
        }
    }
}
