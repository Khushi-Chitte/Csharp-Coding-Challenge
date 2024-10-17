using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using ExceptionLayer;
using UtilityLayer;

namespace DAOLayer
{
    //ANS 7
    public class OrderProcessor : IOrderManagementRepository
    {

        //ANS 9
        public void createOrder(User user, List<Product> products)
        {
            using (SqlConnection conn = new SqlConnection(DBUtil.ReturnCn("dbCn")))
            {
                conn.Open();

                string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE UserId = @UserId";
                SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn);
                checkUserCmd.Parameters.AddWithValue("@UserId", user._userId);
                int userExists = (int)checkUserCmd.ExecuteScalar();

                if (userExists == 0)
                {
                    createUser(user);
                }

                string insertOrderQuery = "INSERT INTO Orders (UserId) VALUES (@UserId); SELECT SCOPE_IDENTITY();";
                SqlCommand insertOrderCmd = new SqlCommand(insertOrderQuery, conn);
                insertOrderCmd.Parameters.AddWithValue("@UserId", user._userId);
                int orderId = Convert.ToInt32(insertOrderCmd.ExecuteScalar());

                foreach (var product in products)
                {
                    string insertOrderDetailsQuery = "INSERT INTO OrderDetails (OrderId, ProductId, Quantity) VALUES (@OrderId, @ProductId, @Quantity)";
                    SqlCommand insertOrderDetailsCmd = new SqlCommand(insertOrderDetailsQuery, conn);
                    insertOrderDetailsCmd.Parameters.AddWithValue("@OrderId", orderId);
                    insertOrderDetailsCmd.Parameters.AddWithValue("@ProductId", product._productId);
                    insertOrderDetailsCmd.Parameters.AddWithValue("@Quantity", product._quantityInStock);
                    insertOrderDetailsCmd.ExecuteNonQuery();
                }

                Console.WriteLine("Order created for user " + user._username);
            }
        }

        public void cancelOrder(int userId, int orderId)
        {
            using (SqlConnection conn = new SqlConnection(DBUtil.ReturnCn("dbCn")))
            {
                conn.Open();

                string checkOrderQuery = "SELECT COUNT(*) FROM Orders WHERE OrderId = @OrderId AND UserId = @UserId";
                using (SqlCommand checkCmd = new SqlCommand(checkOrderQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@OrderId", orderId);
                    checkCmd.Parameters.AddWithValue("@UserId", userId);

                    int orderCount = (int)checkCmd.ExecuteScalar();

                    if (orderCount == 0)
                    {
                        throw new OrderNotFoundException($"Order with ID {orderId} for User {userId} not found.");
                    }
                }

                string cancelOrderQuery = "DELETE FROM Orders WHERE OrderId = @OrderId AND UserId = @UserId";
                using (SqlCommand cancelCmd = new SqlCommand(cancelOrderQuery, conn))
                {
                    cancelCmd.Parameters.AddWithValue("@OrderId", orderId);
                    cancelCmd.Parameters.AddWithValue("@UserId", userId);
                    cancelCmd.ExecuteNonQuery();

                    Console.WriteLine("Order canceled successfully.");
                }
            }
        }


        public void createProduct(User user, Product product)
        {
            using (SqlConnection conn = new SqlConnection(DBUtil.ReturnCn("dbCn")))
            {
                conn.Open();

                string sql = "INSERT INTO Products (ProductId, ProductName, Description, Price, QuantityInStock, Type, Brand) " +
                             "VALUES (@ProductId, @ProductName, @Description, @Price, @QuantityInStock, @Type, @Brand)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", product._productId);
                    cmd.Parameters.AddWithValue("@ProductName", product._productName);
                    cmd.Parameters.AddWithValue("@Description", product._description);
                    cmd.Parameters.AddWithValue("@Price", product._price);
                    cmd.Parameters.AddWithValue("@QuantityInStock", product._quantityInStock);
                    cmd.Parameters.AddWithValue("@Type", product._type);

                    if (product is Electronics electronics)
                    {
                        cmd.Parameters.AddWithValue("@Brand", electronics._brand);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Brand", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void createUser(User user)
        {
            using (SqlConnection conn = new SqlConnection(DBUtil.ReturnCn("dbCn")))
            {
                conn.Open();
                string insertUserQuery = "INSERT INTO Users (UserId, Username, Password, Role) VALUES (@UserId, @Username, @Password, @Role)";
                SqlCommand cmd = new SqlCommand(insertUserQuery, conn);

                cmd.Parameters.AddWithValue("@UserId", user._userId);
                cmd.Parameters.AddWithValue("@Username", user._username);
                cmd.Parameters.AddWithValue("@Password", user._password);
                cmd.Parameters.AddWithValue("@Role", user._role);

                cmd.ExecuteNonQuery();
                Console.WriteLine("User created: " + user._username);
            }
        }

        public List<Product> getAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(DBUtil.ReturnCn("dbCn")))
            {
                conn.Open();
                string getAllProductsQuery = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(getAllProductsQuery, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string type = reader["Type"].ToString();
                    Product product;

                    if (type == "Electronics")
                    {
                        product = new Electronics(
                            (int)reader["ProductId"],
                            reader["ProductName"].ToString(),
                            reader["Description"].ToString(),
                            (double)reader["Price"],
                            (int)reader["QuantityInStock"],
                            reader["Brand"].ToString(),
                            (int)reader["WarrantyPeriod"]
                        );
                    }
                    else if (type == "Clothing")
                    {
                        product = new Clothing(
                            (int)reader["ProductId"],
                            reader["ProductName"].ToString(),
                            reader["Description"].ToString(),
                            (double)reader["Price"],
                            (int)reader["QuantityInStock"],
                            reader["Size"].ToString(),
                            reader["Color"].ToString()
                        );
                    }
                    else
                    {
                        product = new Product(
                            (int)reader["ProductId"],
                            reader["ProductName"].ToString(),
                            reader["Description"].ToString(),
                            (double)reader["Price"],
                            (int)reader["QuantityInStock"],
                            reader["Type"].ToString()
                        );
                    }

                    products.Add(product);
                }
            }

            return products;
        }

        public List<Product> getOrderByUser(User user)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(DBUtil.ReturnCn("dbCn")))
            {
                conn.Open();
                string getOrderQuery = @"
            SELECT p.*, od.Brand, od.WarrantyPeriod, od.Size, od.Color 
            FROM Products p
            JOIN OrderDetails od ON p.ProductId = od.ProductId
            JOIN Orders o ON od.OrderId = o.OrderId
            WHERE o.UserId = @UserId";

                using (SqlCommand cmd = new SqlCommand(getOrderQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", user._userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string type = reader["Type"].ToString();
                            Product product;


                            decimal priceDecimal = (decimal)reader["Price"];
                            double price = Convert.ToDouble(priceDecimal);

                            if (type == "Electronics")
                            {
                                product = new Electronics(
                                    (int)reader["ProductId"],
                                    reader["ProductName"].ToString(),
                                    reader["Description"].ToString(),
                                    price,
                                    (int)reader["QuantityInStock"],
                                    reader["Brand"].ToString(),
                                    (int)reader["WarrantyPeriod"]
                                );
                            }
                            else if (type == "Clothing")
                            {
                                product = new Clothing(
                                    (int)reader["ProductId"],
                                    reader["ProductName"].ToString(),
                                    reader["Description"].ToString(),
                                    price,
                                    (int)reader["QuantityInStock"],
                                    reader["Size"].ToString(),
                                    reader["Color"].ToString()
                                );
                            }
                            else
                            {
                                product = new Product(
                                    (int)reader["ProductId"],
                                    reader["ProductName"].ToString(),
                                    reader["Description"].ToString(),
                                    price,
                                    (int)reader["QuantityInStock"],
                                    type
                                );
                            }

                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

    }
}