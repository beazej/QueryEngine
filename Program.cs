namespace QueryEngine
{
    public class Data
    {
        public List<User> Users;
        public List<Order> Orders;
    }

    public class User
    {
        public string Email; 
        public string FullName;
        public int Age;
    }

    public class Order
    {
        public int Id;
        public string Name;
    }

    internal class Program
    {
        static List<Object> From(Data data, string query)
        {
           List<Object> list= new List<Object>();
            return list;
        }
        static List<Object> Where(List<Object> list, string query)
        {
            return list;
        }
        static List<Object> Select(List<Object> list, string query)
        {
            return list;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Query Engine");
        }
    }
}