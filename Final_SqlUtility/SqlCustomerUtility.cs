using Final_Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Globalization;
using System.Data.SqlTypes;

namespace Final_SqlUtility
{
    public class SqlCustomerUtility
    {
        private string _connectionStringName;

        //Constructor
        public SqlCustomerUtility(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        //Add Customer
        public Customer AddCustomer(Customer template)
        {
            // Variables
            DbParameter[] parameters;

            // Date Setting
            template.ModifiedDate = DateTime.Now;

            // Generate Password
            int passLength = 6;
            string password = GenerateRandomString(passLength);

            
            // Password Salt
            template.PasswordSalt = GenerateSaltValue();

            // Password Hash
            //string password = template.Password;
            template.PasswordHash = EncodePassword(password, template.PasswordSalt);

            // Row Guid Setup
            Guid guidrow = Guid.NewGuid();
            template.rowguid = guidrow;

            //Setup
            parameters = BuildParameters(template);

            //DO Insert
            DbConnection conn = GetConnection();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO [AdventureWorksLT2008R2].[SalesLT].[Customer] " +
                        "(Title, FirstName, LastName, MiddleName,  Suffix, EmailAddress, Phone, CompanyName, SalesPerson, ModifiedDate, PasswordHash, PasswordSalt, rowguid) " +
                "VALUES (@title, @firstname, @lastName, @middleName, @suffix, @emailAddress, @phone, @companyName, @salesPerson, @modifiedDate, @passwordHash, @passwordSalt, @rowguid) " +
                "SELECT * FROM [AdventureWorksLT2008R2].[SalesLT].[Customer] WHERE CustomerId = @@Identity";

            cmd.Parameters.AddRange(parameters);
            //execute insert
            try
            {
                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();
                List<Customer> mapResults = MapFromSqlReader(reader);
                if (mapResults.Count == 1)
                {
                    return mapResults[0];
                }
                else
                {
                    throw new ApplicationException("Too many results from the SQL INSERT");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Update Customer Entry
        public Customer UpdateCustomer(Customer customerObj)
        {
            //variables

            //DbConnection conn = GetConnection();
            //DbCommand cmd = conn.CreateCommand();

            string connstring = ConfigurationManager.ConnectionStrings["SqlCustomerConnection"].ToString();

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                //conn.Open();
                SqlCommand cmd = new SqlCommand();

            //setup
            cmd.CommandText = "UPDATE [AdventureWorksLT2008R2].[SalesLT].[Customer] " +
                "SET FirstName = @firstName, LastName = @lastName, EmailAddress = @emailAddress, Title = @title, " +
                "MiddleName = @middleName, Suffix = @suffix, CompanyName = @companyName, SalesPerson = @salesPerson, " +
                "Phone = @phone, PasswordHash = @passwordHash, PasswordSalt = @passwordSalt, rowguid = @rowguid " +
                "WHERE CustomerID = @CustomerID;" +
                "SELECT * FROM [AdventureWorksLT2008R2].[SalesLT].[Customer] WHERE CustomerId = @CustomerID";

            //cmd.Parameters.AddRange(BuildParameters(customerObj));
                cmd.Connection = conn;

                if (string.IsNullOrEmpty(customerObj.Title))
                {
                    cmd.Parameters.AddWithValue("@title", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@title", customerObj.Title);
                }

                if (string.IsNullOrEmpty(customerObj.MiddleName))
                {
                    cmd.Parameters.AddWithValue("@middleName", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@middleName", customerObj.MiddleName);
                }

                if (string.IsNullOrEmpty(customerObj.Suffix))
                {
                    cmd.Parameters.AddWithValue("@suffix", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@suffix", customerObj.Suffix);
                }

                cmd.Parameters.AddWithValue("@customerID", customerObj.CustomerID);
                cmd.Parameters.AddWithValue("@modifiedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@firstName", customerObj.FirstName);
                cmd.Parameters.AddWithValue("@rowguid", customerObj.rowguid);
                cmd.Parameters.AddWithValue("@lastName", customerObj.LastName);
                cmd.Parameters.AddWithValue("@passwordSalt", customerObj.PasswordSalt);
                cmd.Parameters.AddWithValue("@companyName", customerObj.CompanyName);
                cmd.Parameters.AddWithValue("@salesPerson", customerObj.SalesPerson);
                cmd.Parameters.AddWithValue("@emailAddress", customerObj.EmailAddress);
                cmd.Parameters.AddWithValue("@phone", customerObj.Phone);
                cmd.Parameters.AddWithValue("@passwordHash", customerObj.PasswordHash);
                                            
                                            
            //Execute                       
            try
            {
                conn.Open();
                List<Customer> resultCustomer = MapFromSqlReader(cmd.ExecuteReader());

                if (resultCustomer != null && resultCustomer.Count == 1)
                {
                    return resultCustomer[0];
                }
                else
                {
                    throw new ApplicationException("Customer was not updated.");
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                conn.Close();
            }

            }
        }

        //Delete Customer Entry
        public void DeleteCustomer(Customer customerObj)
        {
            //Variables
            DbConnection conn = GetConnection();
            DbCommand cmd = conn.CreateCommand();

            //Setup
            cmd.CommandText = "DELETE FROM [SalesLT].[CustomerAddress] WHERE [CustomerID] = " + customerObj.CustomerID +
                "DELETE FROM [AdventureWorksLT2008R2].[SalesLT].[Customer] WHERE [CustomerId] = " + customerObj.CustomerID ;
            //DbParameter[] param = BuildParameters(customerObj);
            //cmd.Parameters.AddRange(param);


            //Execute
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        //Get All Customers
        public List<Customer> GetAllCustomers()
        {
            List<Customer> allCustomers;

            DbConnection conn = GetConnection();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT [CustomerID], [Title], [FirstName], [MiddleName], [LastName], [Suffix], [CompanyName]" +
                ", [SalesPerson], [EmailAddress], [Phone], [PasswordHash], [PasswordSalt], [rowguid], [ModifiedDate] " + 
                " FROM [AdventureWorksLT2008R2].[SalesLT].[Customer] WHERE CustomerID = 30102";
            try
            {
                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();
                allCustomers = MapFromSqlReader(reader);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            if (allCustomers == null)
            {
                return new List<Customer>();
            }
            else
            {
                return allCustomers;
            }
            

        }


        public List<Orders> GetAllOrdersforCustomer(int CustomerID_Order)
        {
            List<Orders> allOrders;

            DbConnection conn = GetConnection();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM [AdventureWorksLT2008R2].[SalesLT].[SalesOrderHeader] where [CustomerID] = " + CustomerID_Order;
            try
            {
                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();
                allOrders = MapFromSqlReaderOrders(reader);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            if (allOrders == null)
            {
                return new List<Orders>();
            }
            else
            {
                return allOrders;
            }


        }




        public List<Product> GetProductDetails(int ProductID)
        {
            List<Product> allOrders;

            DbConnection conn = GetConnection();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM [AdventureWorksLT2008R2].[SalesLT].[Product] where [ProductID] = " + ProductID;
            try
            {
                conn.Open();
                DbDataReader reader = cmd.ExecuteReader();
                allOrders = MapFromSqlReaderProduct(reader);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            if (allOrders == null)
            {
                return new List<Product>();
            }
            else
            {
                return allOrders;
            }


        }

        public List<OrderDetails> GetOrderDetails(int SalesOrderID)
        {
            List<OrderDetails> allOrderDetails;
            string strConn = ConfigurationManager.ConnectionStrings["SqlCustomerConnection"].ToString();

            SqlConnection conn = new SqlConnection(strConn);
            //DbConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand("SELECT [SalesOrderID], [SalesOrderDetailID], [OrderQty], [ProductID], [UnitPrice], [UnitPriceDiscount]" +
                    ",  LineTotal, [rowguid], [ModifiedDate] FROM [AdventureWorksLT2008R2].[SalesLT].[SalesOrderDetail] " +
                    "WHERE SalesOrderID = " + SalesOrderID, conn);
            //DbCommand cmd = conn.CreateCommand();
            //cmd.CommandText = "SELECT [SalesOrderID], [SalesOrderDetailID], [OrderQty], [ProductID], [UnitPrice], [UnitPriceDiscount]" +
                   // ", CAST([LineTotal] as float) as LineTotal, [rowguid], [ModifiedDate] FROM [AdventureWorksLT2008R2].[SalesLT].[SalesOrderDetail] " +
                    //"WHERE SalesOrderID = " + SalesOrderID;
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //DbDataReader reader = cmd.ExecuteReader();
                allOrderDetails = MapFromSqlReaderOrdersDetails(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            if (allOrderDetails == null)
            {
                return new List<OrderDetails>();
            }
            else
            {
                return allOrderDetails;
            }
        }

        private List<Product> MapFromSqlReaderProduct(DbDataReader reader)
        {
            int i = 0;
            List<Product> product = new List<Product>();
            while (reader.Read())
            {
                Product productDataTransferObject = new Product()
                {
                    ProductID = (int)reader["ProductID"],
                    Name = (string)reader["Name"] ,
                    ProductNumber = (string)reader["ProductNumber"],
                    Color = reader["Color"] as string ?? default(string),
                    StandardCost = (decimal)reader["StandardCost"],
                    ListPrice = (decimal)reader["ListPrice"] ,
                    Size = reader["Size"] as string ?? default(string),
                    Weight = (reader["Weight"] as decimal?) ?? -1,
                    ProductCategoryID = (reader["ProductCategoryID"] as int?) ?? -1,
                    ProductModeID = (reader["ProductCategoryID"] as int?) ?? -1,
                    SellStartDate = (reader["SellStartDate"] as DateTime?) ?? DateTime.MinValue,
                    SellEndDate = (reader["SellEndDate"] as DateTime?) ?? DateTime.MinValue,
                    DiscontinuedDate = (reader["DiscontinuedDate"] as DateTime?) ?? DateTime.MinValue,
                    ////ThumbNailPhoto = (Byte)reader["ThumbNailPhoto"], 
                    ThumbNailPhotoFileName = (string)reader["ThumbNailPhotoFileName"],
                    rowguid = (Guid)reader["rowguid"],
                    ModifiedDate = (DateTime)reader["ModifiedDate"],
                    

                    

                };
                product.Add(productDataTransferObject);
            }

            return product;
        }

        private List<OrderDetails> MapFromSqlReaderOrdersDetails(SqlDataReader reader)
        {
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            while (reader.Read())
            {
                OrderDetails OrderDetailsDataTransferObject = new OrderDetails()
                {
                    SalesOrderID = (int)reader["SalesOrderID"],
                    SalesOrderDetailID = (int)reader["SalesOrderDetailID"],
                    OrderQty = (short)reader["OrderQty"],
                    ProductID = (int)reader["ProductID"],
                    UnitPrice = (decimal)reader["UnitPrice"],
                    UnitPriceDiscount = (decimal)reader["UnitPriceDiscount"],
                    LineTotal = (decimal)reader["LineTotal"],
                    rowguid = (Guid)reader["rowguid"],
                    ModifiedDate = (DateTime)reader["ModifiedDate"]

                };
                orderDetails.Add(OrderDetailsDataTransferObject);
            }

            return orderDetails;
        }

        //Connection Object Helper
        private DbConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;

            return conn;
        }

        //Map From Sql Reader: takes sql reader and returns a list/collection of Customer objects
        private List<Customer> MapFromSqlReader(DbDataReader reader)
        {
            List<Customer> customers = new List<Customer>();
            while (reader.Read())
            {
                Customer customerDataTransferObject = new Customer()
                {
                    CustomerID = (int)reader["CustomerID"],
                    Title = reader["Title"] as string ?? default(string),
                    //Title = (string)reader["Title"],
                    FirstName = (string)reader["FirstName"],
                    MiddleName = reader["MiddleName"] as string ?? default(string),
                    //MiddleName = (string)reader["MiddleName"],
                    LastName = (string)reader["LastName"],
                    Suffix = reader["Suffix"] as string ?? default(string),
                    //Suffix = (string)reader["Suffix"],
                    CompanyName = (string)reader["CompanyName"],
                    SalesPerson = (string)reader["SalesPerson"],
                    EmailAddress = (string)reader["EmailAddress"],
                    Phone = (string)reader["Phone"],
                    PasswordHash = (string)reader["PasswordHash"],
                    PasswordSalt = (string)reader["PasswordSalt"],
                    rowguid = (Guid)reader["rowguid"],
                    ModifiedDate = (DateTime)reader["ModifiedDate"],
                    
                };
                customers.Add(customerDataTransferObject);
            }

            return customers;
        }


        private List<Orders> MapFromSqlReaderOrders(DbDataReader reader)
        {
            List<Orders> orders = new List<Orders>();
            while (reader.Read())
            {
                Orders OrderDataTransferObject = new Orders()
                {
                    SalesOrderID = (int)reader["SalesOrderID"],
                    RevisionNumber = (int)(byte)reader["RevisionNumber"],
                    OrderDate = (DateTime)reader["OrderDate"],
                    DueDate = (DateTime)reader["DueDate"],
                    ShipDate = (reader["ShipDate"] as DateTime?) ?? DateTime.MinValue,
                    //ShipDate = (int)reader["SalesOrderID"],
                    Status = (int)(Byte)reader["Status"],
                    OnlineOrderFlag = (bool)reader["OnlineOrderFlag"],
                    SalesOrderNumber = (string)reader["SalesOrderNumber"],
                    PurchaseOrderNumber = (reader["PurchaseOrderNumber"] as string) ?? "",
                    AccountNumber = (reader["AccountNumber"] as string) ?? "",
                    CustomerID_Order = (int)reader["CustomerID"],
                    ShipToAddressID = (reader["ShipToAddressID"] as int?) ?? -1,
                    BillToAddressID = (reader["BillToAddressID"] as int?) ?? -1,
                    ShipMethod = (string)reader["ShipMethod"],
                    CreditCardApprovalCode = (reader["CreditCardApprovalCode"] as string) ?? "",
                    SubTotal = (decimal)reader["SubTotal"],
                    TaxAmt = (decimal)reader["TaxAmt"],
                    Freight = (decimal)reader["Freight"],
                    TotalDue = (decimal)reader["TotalDue"],
                    Comment = (reader["Comment"] as string) ?? "",
                    rowguid = (Guid)reader["rowguid"],
                    ModifiedDate = (DateTime)reader["ModifiedDate"]

                };
                orders.Add(OrderDataTransferObject);
            }

            return orders;
        }

        //Build Parameters: takes customer object and returns a collection of sql parameters and do the mapping
        private DbParameter[] BuildParameters(Customer customerObj)
        {
            DbParameter[] param = new DbParameter[]
            {
                new SqlParameter("@customerID", customerObj.CustomerID),
                new SqlParameter("@title", customerObj.Title),
                new SqlParameter("@firstName", customerObj.FirstName),
                new SqlParameter("@middleName", customerObj.MiddleName),
                new SqlParameter("@lastName", customerObj.LastName),
                new SqlParameter("@suffix", customerObj.Suffix),
                new SqlParameter("@companyName", customerObj.CompanyName),
                new SqlParameter("@salesPerson", customerObj.SalesPerson),
                new SqlParameter("@emailAddress", customerObj.EmailAddress),
                new SqlParameter("@phone", customerObj.Phone),
                new SqlParameter("@passwordHash", customerObj.PasswordHash),
                new SqlParameter("@passwordSalt", customerObj.PasswordSalt),
                new SqlParameter("@rowguid", customerObj.rowguid),
                new SqlParameter("@modifiedDate", customerObj.ModifiedDate)
            };

            return param;
        }

        //Generate Password Salt
        public static string GenerateSaltValue()
        {
            UnicodeEncoding utf16 = new UnicodeEncoding();

            if (utf16 != null)
            {
                // Create a random number object seeded from the value of the last random seed value. 
                // This is done interlocked because it is a static value and we want it to roll forward safely.

                Random random = new Random(unchecked((int)DateTime.Now.Ticks));

                if (random != null)
                {
                    // Create an array of random values.

                    byte[] saltValue = new byte[8];

                    random.NextBytes(saltValue);

                    // Convert the salt value to a string. Note that the resulting string
                    // will still be an array of binary values and not a printable string. 
                    // Also it does not convert each byte to a double byte.

                    string saltValueString = utf16.GetString(saltValue);

                    // Return the salt value as a string.

                    return saltValueString;
                }
            }

            return null;
        }
        
        //Generalte Password Hash
        public string EncodePassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        //Generate Random Password : String
        public static string GenerateRandomString(int length)
        {
            //It will generate string with combination of small,capital letters and numbers
            char[] charArr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            string randomString = string.Empty;
            Random objRandom = new Random();
            for (int i = 0; i < length; i++)
            {
                //Don't Allow Repetation of Characters
                int x = objRandom.Next(1, charArr.Length);
                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                    randomString += charArr.GetValue(x);
                else
                    i--;
            }
            return randomString;
        }


    }
}
