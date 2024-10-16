using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionLibrary;
using System.Collections;

namespace DAOLibrary
{
    public class OrderDAO : IOrderService
    {
        private SqlConnection sqlConnection = UtilLibrary.DBPropertyUtil.GetConnectionString();
        public decimal CalculateTotalAmount(int customerID)
        {
            string query = "SELECT SUM(od.Quantity * p.Price) AS TotalAmount " +
                   "FROM OrderDetails od " +
                   "INNER JOIN Products p ON od.ProductID = p.ProductID " +
                   "WHERE od.OrderID = (SELECT MAX(OrderID) FROM Orders WHERE CustomerID = @CustomerID)";

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                sqlConnection.Close();
                sqlConnection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                        {
                            return reader.GetDecimal(0);
                        }
                        else
                        {
                            Console.WriteLine("No order details found for customer ID {0}.", customerID);
                            return 0.0m;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No order details found for customer ID {0}.", customerID);
                        return 0.0m; 
                    }
                }
            }

        }

        public bool CancelOrder(int OrderID)
        {
            string query = $"DELETE FROM Orders WHERE OrderID = {OrderID}";

            SqlCommand cmd = new SqlCommand (query, sqlConnection);

            sqlConnection.Open();
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.HasRows) 
            { 
                sqlDataReader.Read();
                return true;
            }
            sqlDataReader.Close();
            return false;

        }

        public int GetOrderID(int CustomerID)
        {
            string query = $"SELECT OrderID FROM Orders WHERE CustomerID = {CustomerID}";
            
            SqlCommand cmd = new SqlCommand(query,sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if(sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                int OrdID =  Convert.ToInt32(sqlDataReader[0]);
                sqlDataReader.Close();
                return OrdID;
            }
            sqlDataReader.Close();
            Console.WriteLine($"Customer ID not found {CustomerID}");
            throw new ExceptionLibrary.InvalidDataException("Customer ID not found");
        }

        public bool UpdateOrderInfo(int OrderID, DateTime OrderDate, decimal TotalAmount, string Status)
        {
            SqlCommand cmd = new SqlCommand("[dbo].UpdateOrder", sqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderID", OrderID);
            cmd.Parameters.AddWithValue("@OrderDate", OrderDate);
            cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
            cmd.Parameters.AddWithValue("@Status", Status);

            sqlConnection.Open();
            cmd.ExecuteNonQuery();
            sqlConnection.Close();


            return true;
        }
        public bool UpdateOrderStatus(int OrderID, string newStatus)
        {
            string query = $"UPDATE Orders SET STATUS = {newStatus} WHERE OrderID = {OrderID}";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                return sqlDataReader.Read();
            }
            return false;
        }
        

        public bool AddOrder(int customerID)
        {

            
            string query = "INSERT INTO Orders (CustomerID, OrderDate, Status) VALUES (@CustomerID, @OrderDate, @Status)";

            

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                sqlConnection.Open();
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@Status", "Pending");

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    Console.WriteLine("Order added successfully.");

                     return true; 
                
                }
                else
                {
                    Console.WriteLine("Error adding order.");
                    return false;
                }
            }
        }

        public string GetOrderStatus(int orderID)
        {
            if (orderID <= 0)
            {
                throw new ArgumentException("Order ID must be greater than zero.");
            }

            string query = "SELECT Status FROM Orders WHERE OrderID = @OrderID";

            sqlConnection.Close();
             sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return reader.GetString(0);
                        }
                        else
                        {
                            throw new OrderNotFoundException($"Order ID {orderID} not found.");
                        }
                    }
                }
        }

        
        public bool UpdateAllOrderTotals()
        {
            string query = "UPDATE Orders SET TotalAmount = (SELECT SUM(od.Quantity * p.Price) FROM OrderDetails od INNER JOIN Products p ON od.ProductID = p.ProductID WHERE od.OrderID = Orders.OrderID)";

                sqlConnection.Close();
                sqlConnection.Open();

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Total amounts updated successfully for all orders.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Error updating total amounts for all orders.");
                        return false;
                    }
                }
            
        }


    }
}
