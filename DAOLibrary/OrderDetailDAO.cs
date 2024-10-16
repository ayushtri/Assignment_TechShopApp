using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary
{
    public class OrderDetailDAO : IOrderDetailService
    {
        private SqlConnection sqlConnection = UtilLibrary.DBPropertyUtil.GetConnectionString();
        public bool AddDiscount(int OrderID, int DiscountPercent)
        {
            if (DiscountPercent < 0 || DiscountPercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(DiscountPercent), "Discount percentage must be between 0 and 100.");
            }
            decimal discountAmount = OrderID * (DiscountPercent / 100m);
            string query = $"UPDATE Orders SET TotalAmount = TotalAmount - {discountAmount}";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            if (rowsAffected == 0)
            {
                Console.WriteLine($"Order ID {OrderID} not found.");
                return false; 
            }

            return true;
        }

        public decimal CalculateSubtotal(int OrderDetailID)
        {
            String query = "SELECT SUM(od.Quantity * p.Price) AS TotalAmount " +
                "FROM OrderDetails od " +
                "INNER JOIN Products p " +
                "ON od.ProductID = p.ProductID " +
                $"WHERE OrderDetailID = {OrderDetailID}";

            SqlCommand cmd = new SqlCommand(@query, sqlConnection);
            sqlConnection.Open ();

            using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
            {
                if(sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    return Convert.ToDecimal (sqlDataReader.GetValue(0));
                }
                else
                {
                    Console.WriteLine($"OrderDetail ID {OrderDetailID} not found.");
                    return 0;
                }
            }
        }

        public int GetOrderDetailID(int OrderID)
        {
            string query = $"SELECT OrderDetailID FROM OrderDetails WHERE OrderID = {OrderID}";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();

            using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
            {
                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();
                    return Convert.ToInt32(sqlDataReader[0]);
                }
                else
                {
                    Console.WriteLine($"Order ID {OrderID} not found.");
                    return -1; 
                }
            }
        }

        public Tuple<int, int, int, int> GetOrderDetailInfo(int OrderDetailID)
        {
            Tuple<int, int, int, int> OrderDetailTuple = null;
            String query = $"SELECT * FROM OrderDetails WHERE OrderDetailID = {OrderDetailID}";
            SqlCommand  sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                OrderDetailTuple = Tuple.Create(Convert.ToInt32(sqlDataReader[0]), Convert.ToInt32(sqlDataReader[1]), Convert.ToInt32(sqlDataReader[2]), Convert.ToInt32(sqlDataReader[3]));
                sqlDataReader.Close();
                return OrderDetailTuple;
            }
            sqlDataReader.Close();
            Console.WriteLine($"OrderDetail ID not found {OrderDetailID}");
            throw new ExceptionLibrary.InvalidDataException("OrderDetail ID not found");
        }

        public bool UpdateQuantity(int OrderDetailID, int NewQuanity)
        {
            String query = $"UPDATE OrderDetails SET Quantity = {NewQuanity} WHERE OrderDetailsID = {OrderDetailID}";
            SqlCommand sqlCommand = new SqlCommand( query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows) 
            { 
                return sqlDataReader.Read();         
            }

            sqlDataReader.Close();
            Console.WriteLine($"OrderDetail ID not found {OrderDetailID}");
            throw new ExceptionLibrary.InvalidDataException("OrderDetail ID not found");
        }

        public bool AddOrderDetails(int orderID, int productID, int quantity)
        {
            if (orderID <= 0 || productID <= 0 || quantity <= 0)
            {
                throw new ArgumentException("Order ID, Product ID, and Quantity must be greater than zero.");
            }

            string query = "INSERT INTO OrderDetails (OrderID, ProductID, Quantity) VALUES (@OrderID, @ProductID, @Quantity)";

            sqlConnection.Open();

            using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 1)
                {
                    Console.WriteLine("Order details added successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Error adding order details.");
                    return false;
                }
            }
        }

       
    }
}
