using DAOLayer;
using EntityLayer;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        IOrderManagementRepository orderProcessor = new OrderProcessor();

        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Create Product");
            Console.WriteLine("3. Cancel Order");
            Console.WriteLine("4. Get All Products");
            Console.WriteLine("5. Get Order by User");
            Console.WriteLine("6. Exit");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateUser(orderProcessor);
                    break;
                case "2":
                    CreateProduct(orderProcessor);
                    break;
                case "3":
                    CancelOrder(orderProcessor);
                    break;
                case "4":
                    GetAllProducts(orderProcessor);
                    break;
                case "5":
                    GetOrderByUser(orderProcessor);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    static void CreateUser(IOrderManagementRepository orderProcessor)
    {
        Console.WriteLine("Enter user ID:");
        int userId = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();
        Console.WriteLine("Enter role (Admin/User):");
        string role = Console.ReadLine();

        User user = new User(userId, username, password, role);
        orderProcessor.createUser(user);

        Console.WriteLine("User created successfully.");
    }


    static void CreateProduct(IOrderManagementRepository orderProcessor)
    {
        Console.WriteLine("Enter user ID (Admin):");
        int userId = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter product ID:");
        int productId = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter product name:");
        string productName = Console.ReadLine();
        Console.WriteLine("Enter product description:");
        string description = Console.ReadLine();
        Console.WriteLine("Enter price:");
        double price = double.Parse(Console.ReadLine());
        Console.WriteLine("Enter quantity in stock:");
        int quantityInStock = int.Parse(Console.ReadLine());

        User user = new User(userId, username, "password", "Admin");
        Product product = new Product(productId, productName, description, price, quantityInStock, "General");

        orderProcessor.createProduct(user, product);

        Console.WriteLine("Product created successfully.");
    }


    static void CancelOrder(IOrderManagementRepository orderProcessor)
    {
        Console.WriteLine("Enter user ID:");
        int userId = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter order ID:");
        int orderId = int.Parse(Console.ReadLine());

        orderProcessor.cancelOrder(userId, orderId);

        Console.WriteLine("Order canceled successfully.");
    }

    static void GetAllProducts(IOrderManagementRepository orderProcessor)
    {
        List<Product> products = orderProcessor.getAllProducts();
        foreach (var product in products)
        {
            Console.WriteLine($"Product ID: {product._productId}, Name: {product._productName}, Description: {product._description}, Price: {product._price}, Quantity: {product._quantityInStock}, Type: {product._type}");
        }
    }

    static void GetOrderByUser(IOrderManagementRepository orderProcessor)
    {
        try
        {
            Console.WriteLine("Enter user ID:");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.WriteLine("Enter username:");
                string username = Console.ReadLine();

                User user = new User(userId, username, "password", "User");
                List<Product> products = orderProcessor.getOrderByUser(user);

                if (products.Count > 0)
                {
                    Console.WriteLine($"Orders for user {username}:");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"Product ID: {product._productId}, Name: {product._productName}, Description: {product._description}, Price: {product._price}, Quantity: {product._quantityInStock}");
                    }
                }
                else
                {
                    Console.WriteLine("No orders found for this user.");
                }
            }
            else
            {
                Console.WriteLine("Invalid User ID.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

