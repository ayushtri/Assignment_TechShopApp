using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionLibrary;
using DAOLibrary;

namespace EntityLibrary
{
    public class Customer
    {
        //private int _CustomerID;
        private string _FirstName;
        private string _LastName;
        private string _Email;
        private string _Phone;
        private string _Address;

        public Customer(string FirstName, string LastName, string Email, string Phone, string Address)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
        }


        /*public int CustomerID
        {
            get { return _CustomerID; }
        }*/

        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ExceptionLibrary.InvalidDataException("Enter a valid First Name");
                }
                else if (value.Length > 50)
                {
                    throw new ExceptionLibrary.InvalidDataException("First Name couldn't exceed 50 characters");
                }
                else _FirstName = value;
            }
        }

        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ExceptionLibrary.InvalidDataException("Enter a valid Last Name");
                }
                else if (value.Length > 50)
                {
                    throw new ExceptionLibrary.InvalidDataException("Last Name couldn't exceed 50 characters");
                }
                else _LastName = value;
            }
        }

        public string Email
        {
            get { return _Email; }
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ExceptionLibrary.InvalidDataException("Enter a valid Email");
                }
                else if (value.Length > 150)
                {
                    throw new ExceptionLibrary.InvalidDataException("Email couldn't exceed 150 characters");
                }
                else _Email = value;
            }
        }

        public string Phone
        {
            get { return _Phone; }
            set
            {
                var isNumeric = long.TryParse(value.ToString(), out long n);

                if (!isNumeric || value == null || value.Length == 0)
                {
                    throw new ExceptionLibrary.InvalidDataException("Enter a valid Phone Number");
                }
                else if (value.Length != 10)
                {
                    throw new ExceptionLibrary.InvalidDataException("Phone Number should be of 10 digits");
                }
                else _Phone = value;
            }
        }

        public string Address
        {
            get { return _Address; }
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ExceptionLibrary.InvalidDataException("Enter a valid Address");
                }
                else if (value.Length > 250)
                {
                    throw new ExceptionLibrary.InvalidDataException("Address couldn't exceed 250 characters");
                }
                else _Address = value;
            }
        }
    }

    public class CustomerOperations
    {
        public Customer GetCustomerDetails(int CustomerID)
        {
            Tuple<int, string, string, string, string, string, int> customerTuple = null;
            CustomerDAO customerDAO = new CustomerDAO();
            customerTuple = customerDAO.GetCustomerDetails(CustomerID);
            Customer c = new Customer(customerTuple.Item2, customerTuple.Item3, customerTuple.Item4,
                customerTuple.Item5, customerTuple.Item6);
            return c;
        }

        public int CalculateTotalOrders(int CustomerID)
        {
            return new CustomerDAO().CalculateTotalOrders(CustomerID);
        }

        /// <summary>
        /// Customer Email will not be updated using this
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool UpdateCustomerInfo(Customer c)
        {
            CustomerDAO customerDAO = new CustomerDAO();
            int CustomerID = new CustomerDAO().GetCustomerID(c.Email);
            return customerDAO.UpdateCustomerInfo(CustomerID, c.FirstName, c.LastName, c.Email, Convert.ToInt64(c.Phone), c.Address);
        }

        /// <summary>
        /// Only to updated customer email
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="NewEmail"></param>
        /// <returns></returns>
        public bool UpdateCustomerEmail(int CustomerID, string NewEmail)
        {
            return new CustomerDAO().UpdateCustomerEmail(CustomerID, NewEmail);
        }

        public bool AddCustomer(Customer c)
        {
            return new CustomerDAO().AddCustomer(c.FirstName, c.LastName, c.Email, Convert.ToInt64(c.Phone), c.Address);
        }
    }
}
