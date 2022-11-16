using System.ComponentModel.DataAnnotations;

namespace QueryEngine
{
    public class Data
    {
        public List<User> Users;
        public List<Order> Orders;
        public Data(List<User> users, List<Order> orders)
        {
            Users = users;
            Orders = orders;
        }   
    }

    public class User
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public User(string email, string fullName, int age)
        {
            Email = email;
            FullName = fullName;
            Age = age;
        }
    }

    public class Order
    {
        public int Id;
        public string Name;
        public Order(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    internal class Program
    {
        static List<Object> From(Data data, string query)
        {
            List<Object> list= new List<Object>();
            return list;
        }
        static List<Object> Where(List<Object> list, string[] query)
        {
            return list;
        }
        static List<Object> Select(List<Object> list, string[] query)
        {
            return list;
        }
        static string SQLInterpreter(Data data, string query)
        {
            string[] elements = query.Split(' ');
            int ids=0;
            for(int i=0; i<elements.Length; i++)
            {
                if (elements[i].Equals("select"))
                {
                    ids = i;
                }
            }
            string fromquery = elements[1];
            string[] wherequery = new string[ids - 3];
            Array.Copy(elements, 3, wherequery, 0, ids - 3);
            string[] selectquery = new string[elements.Length - ids - 1];
            Array.Copy(elements, ids + 1, selectquery, 0, elements.Length - ids - 1);
            List<Object> output = Select(Where(From(data, fromquery), wherequery), selectquery);
            return output.ToString() ?? "null";
        }
        static void Main(string[] args)
        {
            List<User> users = new List<User>();
            users.Add(new User("email", "name", 40));
            List<Order> orders = new List<Order>();
            orders.Add(new Order(0, "a"));
            Data data = new Data(users, orders);
            string query = "from Users where age=40 select email";
            Console.WriteLine(SQLInterpreter(data, query));
        }
    }
}