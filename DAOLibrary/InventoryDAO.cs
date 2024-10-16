using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary
{
    public class InventoryDAO : IInventoryService
    {
        private SqlConnection sqlConnection = UtilLibrary.DBPropertyUtil.GetConnectionString();
        public int GetInventoryID(int productID)
        {
            string query = "SELECT InventoryID FROM Inventory WHERE ProductID = @ProductID";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
               
                cmd.Parameters.AddWithValue("@ProductID", productID);
                sqlConnection.Close();
                sqlConnection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]);
                    }
                }
            }

            return 0; 
        }
        
        public bool AddToInventory(int productID, int quantity)
        {
            int inventoryID = GetInventoryID(productID);
            string query;
            sqlConnection.Close();
            sqlConnection.Open();
            if (GetInventoryValue(productID) > 0)
            {
                query = "UPDATE Inventory SET QuantityInStock = QuantityInStock + @Quantity WHERE InventoryID = @InventoryID";
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {

                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@InventoryID", inventoryID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            else
            {
                query = "INSERT INTO Inventory (ProductID, QuantityInStock, LastStockUpdate) VALUES (@ProductID, @Quantity, @Date)";
                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;

                }
            }

            
        }

        public bool RemoveFromInventory(int productID, int quantity)
        {
            int inventoryID = GetInventoryID(productID);

            string query = "UPDATE Inventory SET QuantityInStock = QuantityInStock - @Quantity WHERE InventoryID = @InventoryID";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
               
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@InventoryID", inventoryID);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public bool UpdateStockQuantity(int productID, int newQuantity)
        {
            int inventoryID = GetInventoryID(productID);

            string query = "UPDATE Inventory SET QuantityInStock = @NewQuantity WHERE InventoryID = @InventoryID";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                cmd.Parameters.AddWithValue("@InventoryID", inventoryID);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public bool IsProductAvailable(int productID, int quantityToCheck)
        {
            int inventoryID = GetInventoryID(productID);

            string query = "SELECT QuantityInStock FROM Inventory WHERE InventoryID = @InventoryID";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@InventoryID", inventoryID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int quantityInStock = reader.GetInt32(0);
                        return quantityInStock >= quantityToCheck;
                    }
                }
            }

            return false; 
        }

        public int GetInventoryValue(int productID)
        {
            int inventoryID = GetInventoryID(productID);

            string query = "SELECT QuantityInStock * Price FROM Inventory INNER JOIN Products ON Inventory.ProductID = Products.ProductID WHERE Inventory.InventoryID = @InventoryID";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@InventoryID", inventoryID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Convert.ToInt32(reader[0]); 
                    }
                }
            }

            return 0;
        }

        public List<Tuple<int, string, int>> ListLowStockProducts(int threshold)
        {
            string query = "SELECT Products.ProductID, Products.ProductName, Inventory.QuantityInStock FROM Inventory INNER JOIN Products ON Inventory.ProductID = Products.ProductID WHERE Inventory.QuantityInStock < @Threshold";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                
                cmd.Parameters.AddWithValue("@Threshold", threshold);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Tuple<int, string, int>> lowStockProducts = new List<Tuple<int, string, int>>();
                    while (reader.Read())
                    {
                        lowStockProducts.Add(Tuple.Create(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                    }

                    return lowStockProducts;
                }
            }
        }

        public List<Tuple<int, string, int>> ListAllProducts()
        {
            string query = "SELECT Products.ProductID, Products.ProductName, Inventory.QuantityInStock FROM Inventory INNER JOIN Products ON Inventory.ProductID = Products.ProductID";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<Tuple<int, string, int>> allProducts = new List<Tuple<int, string, int>>();
                    while (reader.Read())
                    {
                        allProducts.Add(Tuple.Create(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                    }

                    return allProducts;
                }
            }
        }
    }


}
