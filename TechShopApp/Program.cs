using EntityLibrary;
using System.Transactions;
internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("MENU");
            Console.WriteLine("1. Add a Customer");
            Console.WriteLine("2. List All Products");
            Console.WriteLine("3. Add a Product");
            Console.WriteLine("4. Add an Order");
            Console.WriteLine("5. Track Order Status");
            Console.WriteLine("6. Inventory Management");
            Console.WriteLine("7. Customer Account Updates");



            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Adding a customer: ");
                    Console.Write("Enter First Name: ");
                    string FirstName = Console.ReadLine();
                    Console.Write("Enter Last Name: ");
                    string LastName = Console.ReadLine();
                    Console.Write("Enter Email: ");
                    string Email = Console.ReadLine();
                    Console.Write("Enter Phone: ");
                    string Phone = Console.ReadLine();
                    Console.Write("Enter Address: ");
                    string Address = Console.ReadLine();

                    Customer c = new Customer(FirstName, LastName, Email, Phone, Address);
                    CustomerOperations co = new CustomerOperations();
                    bool success = co.AddCustomer(c);
                    if (success) Console.WriteLine("Added successfully");
                    else Console.WriteLine("Failed to add customer");
                    break;

                case 2:
                    Console.WriteLine("List of all products");
                    ProductOperations po = new ProductOperations();
                    po.GetProducts();
                    break;

                case 3:
                    Console.WriteLine("Adding a product");
                    Console.Write("Enter Product Name: ");
                    string ProductName = Console.ReadLine();
                    Console.Write("Enter Description: ");
                    string Description = Console.ReadLine();
                    Console.Write("Enter Price: ");
                    decimal price = Convert.ToDecimal(Console.ReadLine());

                    Product p = new Product(ProductName, Description, price, 1000001);
                    ProductOperations po2 = new ProductOperations();
                    po2.AddProduct(p);

                    break;

                case 4:
                    Console.WriteLine("Adding an order");
                    Console.Write("Enter Customer ID: ");
                    int custID = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter Product ID: ");
                    int prodID = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter Quanity of Prod: ");   
                    int quant = Convert.ToInt32(Console.ReadLine());


                    CustomerOperations co1 = new CustomerOperations();
                    

                    OrderOperations oo = new OrderOperations();
                    oo.AddOrder(custID);

                    OrderDetailOperations odo = new OrderDetailOperations();
                    odo.AddOrderDetails(oo.GetOrderID(custID), prodID, quant);

                    new OrderOperations().UpdateAllOrderTotals();
                    

                    break;

                case 5:
                    Console.WriteLine("Order status");
                    Console.Write("Order ID: ");
                    int OrdID = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine($"Order Status: {new OrderOperations().GetOrderStatus(OrdID)}");

                    break;

                case 6:
                    Console.WriteLine("Inventory Management");
                    Console.WriteLine("1. Add to inventory");
                    Console.WriteLine("2. UpdateStockQuantity");
                    Console.WriteLine("3. GetInventoryValue");

                    Console.Write("Enter Choice: ");
                    Console.WriteLine();
                    int ch2 = Convert.ToInt32(Console.ReadLine());
                    switch (ch2)
                    {
                        case 1:
                            Console.WriteLine("1. Add to inventory");
                            Console.Write("Enter ProductID:");
                            int PID = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Quantity:");
                            int quanty = Convert.ToInt32(Console.ReadLine());
                            bool flag = new InventoryOperations().AddToInventory(PID, quanty);
                            if (flag)
                            {
                                Console.WriteLine("Added Successfully");
                            }
                            else Console.WriteLine("Can't add");
                            break;

                        case 2:
                            Console.WriteLine("2. UpdateStockQuantity");
                            Console.Write("Enter ProductID:");
                            int PID1 = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter New Quantity:");
                            int quanty1 = Convert.ToInt32(Console.ReadLine());
                            bool flag1 = new InventoryOperations().UpdateStockQuantity(PID1, quanty1);
                            if (flag1)
                            {
                                Console.WriteLine("Added Successfully");
                            }
                            else Console.WriteLine("Can't add");

                            break;

                        case 3:
                            Console.WriteLine("3. GetInventoryValue");
                            Console.Write("Enter ProductID:");
                            int PID2 = Convert.ToInt32(Console.ReadLine());
                            int invVal = new InventoryOperations().GetInventoryValue(PID2);
                            Console.WriteLine($"Inventory Value is {invVal}");

                            break;

                        default:
                            Console.WriteLine("Enter a valid choice");
                            break;
                     
                    }
                    break;

                default:
                    Console.WriteLine("Enter a valid choice");
                    break;
            }


            Console.Read();
        }
        catch (Exception ex)
        {
            //Creating an ExceptionLogger
            FileStream fs;
            if (!File.Exists("Logger.txt"))
            {
                fs = new FileStream("Logger.txt", FileMode.CreateNew, FileAccess.Write);
            }
            else
            {
                fs = new FileStream("Logger.txt", FileMode.Append, FileAccess.Write);
            }

            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(ex.Message);
            sw.WriteLine("Source=" + ex.Source);
            sw.WriteLine(ex.StackTrace);

            sw.Close();
            fs.Close();
            sw.Dispose();
            fs.Dispose();

            //Showing Exception
            Console.WriteLine(ex.Message);
            Console.WriteLine("Source=" + ex.Source);
            Console.WriteLine(ex.StackTrace);
        }


        Console.Read();
    }
}