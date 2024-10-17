namespace EntityLayer
{
    //ANS 1
    public class Product
    {
        //ANS 2
        public int _productId { get; set; }
        public string _productName { get; set; }
        public string _description { get; set; }
        public double _price { get; set; }
        public int _quantityInStock { get; set; }
        public string _type { get; set; }



        public Product(int productId, string productName, string description, double price, int quantityInStock, string type)
        {
            _productId = productId;
            _productName = productName;
            _description = description;
            _price = price;
            _quantityInStock = quantityInStock;
            _type = type;
        }

        public Product() { }
    }


    //ANS 3
    public class Electronics : Product
    {
        public string _brand { get; set; }
        public int _warrantyPeriod { get; set; }

        public Electronics(int productId, string productName, string description, double price, int quantityInStock, string brand, int warrantyPeriod)
            : base(productId, productName, description, price, quantityInStock, "Electronics")
        {
            _brand = brand;
            _warrantyPeriod = warrantyPeriod;
        }
    }


    //ANS 4
    public class Clothing : Product
    {
        public string _size { get; set; }
        public string _color { get; set; }

        public Clothing(int productId, string productName, string description, double price, int quantityInStock, string size, string color)
            : base(productId, productName, description, price, quantityInStock, "Clothing")
        {
            _size = size;
            _color = color;
        }
    }

    //ANS 5
    public class User
    {
        public int _userId { get; set; }
        public string _username { get; set; }
        public string _password { get; set; }
        public string _role { get; set; }

        public User(int userId, string username, string password, string role)
        {
            _userId = userId;
            _username = username;
            _password = password;
            _role = role;
        }
    }


}



