using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

namespace DAOLibrary
{
    public class CustomerDAO : ICustomerService
    {
        private SqlConnection sqlConnection = UtilLibrary.DBPropertyUtil.GetConnectionString();

        public bool AddCustomer(string FirstName, string LastName, string Email, long Phone, string Address)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Email))
            {
                throw new ArgumentNullException("First first name, last name, and email are required fields.");
            }
            if (!CheckEmailIsUnique(Email))
            {
                throw new ArgumentNullException("Email must be unique");
            }

            string query = "INSERT INTO Customers (FirstName, LastName, Email, Phone, Address) VALUES (@FirstName, @LastName, @Email, @Phone, @Address)";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {

                sqlCommand.Parameters.AddWithValue("@FirstName", FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", LastName);
                sqlCommand.Parameters.AddWithValue("@Email", Email);
                sqlCommand.Parameters.AddWithValue("@Phone", Phone);
                sqlCommand.Parameters.AddWithValue("@Address", Address);

                int rowsAffected = sqlCommand.ExecuteNonQuery();


                if (rowsAffected == 1)
                {
                    Console.WriteLine("Customer added successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Error adding customer.");
                    return false;
                }
            }

        }

        private bool CheckEmailIsUnique(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email is a required field.");
            }

            string query = $"SELECT CustomerID FROM Customers WHERE Email = @Email";

            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Email", email);
            sqlConnection.Open();


            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                return !reader.HasRows;
            }
        }
        public int GetCustomerID(string Email)
        {
            SqlCommand cmd = new SqlCommand($"SELECT CustomerID FROM Customers WHERE Email='{Email}'", sqlConnection);
            sqlConnection.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            int CustomerID;
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                CustomerID = Convert.ToInt32(sqlDataReader[0]);
                return CustomerID;
            }
            
            return 0;
        }
        public int CalculateTotalOrders(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand($"SELECT OrdersCount FROM  Customers WHERE CustomerID = @CustomerID", sqlConnection);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            sqlConnection.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            int OrdersCount;

            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();

                OrdersCount = Convert.ToInt32(sqlDataReader[0]);
                sqlDataReader.Close();
                return OrdersCount;
            }
            sqlDataReader.Close();
            Console.WriteLine("Invalid customer ID: {0}. No orders found.", CustomerID);
            throw new ExceptionLibrary.InvalidDataException("Invalid CustID");
        }

        public Tuple<int, string, string, string, string, string, int> GetCustomerDetails(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand($"SELECT * FROM  Customers WHERE CustomerID = {CustomerID}", sqlConnection);
            Tuple<int, string, string, string, string, string, int> customerTuple = null;

            sqlConnection.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            if (sqlDataReader != null)
            {
                sqlDataReader.Read();
                int CustID = Convert.ToInt32(sqlDataReader[0]);
                string FirstName = sqlDataReader[1].ToString();
                string LastName = sqlDataReader[2].ToString();
                string Email = sqlDataReader[3].ToString();
                string Phone = sqlDataReader[4].ToString();
                string Address = sqlDataReader[5].ToString();
                int OrdersCount = Convert.ToInt32(sqlDataReader[6]);

                sqlDataReader.Close();

                customerTuple = Tuple.Create<int, string, string, string, string, string, int>
                    (CustID, FirstName, LastName, Email, Phone, Address, OrdersCount);
            }
            sqlDataReader.Close();
            return customerTuple;
        }

        public bool UpdateCustomerInfo(int CustomerID, string FirstName, string LastName, string Email, long Phone, string Address)
        {
            SqlCommand cmd = new SqlCommand("[dbo].UpdateCustomer", sqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@param1", CustomerID);
            cmd.Parameters.AddWithValue("@param2", FirstName);
            cmd.Parameters.AddWithValue("@param3", LastName);
            cmd.Parameters.AddWithValue("@param4", Email);
            cmd.Parameters.AddWithValue("@param5", Phone);
            cmd.Parameters.AddWithValue("@param6", Address);

            sqlConnection.Open();
            cmd.ExecuteNonQuery();  
            sqlConnection.Close();


            return true;
        }

        public bool UpdateCustomerEmail(int CustomerID, String newEmail)
        {
            if (!CheckEmailIsUnique(newEmail))
            {
                throw new ArgumentNullException("Email must be unique");
            }
            SqlCommand cmd = new SqlCommand($"UPDATE Customers SET Email = '{newEmail}' WHERE CustomerID = {CustomerID}", sqlConnection);
            
            sqlConnection.Open ();
             
            SqlDataReader reader = cmd.ExecuteReader();
            return reader.HasRows;
        }
    }
}
