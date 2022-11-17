using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

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
        public double Amount;
        public Order(int id, string name, double amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }
    }

    internal class Program
    {
        static List<Object> From(Data data, string query)
        {
            List<Object> list = new List<Object>();
            if (query.Equals("Users"))
                foreach (User user in data.Users)
                    list.Add(user);
            else if (query.Equals("Orders"))
                foreach (Order order in data.Orders)
                    list.Add(order);

            return list;
        }

        static Boolean WhereStatement(Object o, List<string> query)
        {
            string[] keys = {"<", "<=", ">", ">=", "=", "<>", "!="};
            foreach(string k in keys)
            {
                if (query.Contains(k))
                {
                    try
                    {
                        String name = char.ToUpper(query[0][0]) + query[0].Substring(1);
                        String left = o.GetType().GetProperty(name).GetValue(o, null).ToString();
                        double number, value;
                        if (Double.TryParse(left, out number) & Double.TryParse(query[2].Replace("'", ""), out value))
                        {
                            switch (k)
                            {
                                case "=": return number == value;
                                case "!=": return number != value;
                                case "<>": return number != value;
                                case "<": return number < value;
                                case "<=": return number <= value;
                                case ">=": return number >= value;
                                case ">": return number > value;
                            }
                        }

                        switch (k)
                        {
                            case "=": return left.Equals(query[2].Replace("'", ""));
                        }
                    }
                    catch { }
                    }
            }

            return false;
        }

        static List<bool> bools(List<int> indexes, Object o, List<string> query)
        {
            List<bool> results = new List<bool>();
            results.Add(WhereStatement(o, query.GetRange(0, indexes[0])));
            for (int i = 0; i < indexes.Count - 1; i++)
                results.Add(WhereStatement(o, query.GetRange(indexes[i] + 1, indexes[i + 1] - indexes[i] - 1)));
            results.Add(WhereStatement(o, query.GetRange(indexes[indexes.Count - 1] + 1, query.Count - indexes[indexes.Count - 1] - 1)));
            return results;
        }

        static List<Object> Where(List<Object> list, List<string> query)
        {
            List<Object> output = new List<Object>();
            int id = -1;
            var ands = Enumerable.Range(0, query.Count).Where(i => (query[i] == "and" || query[i] == "AND")).ToList();
            var ors = Enumerable.Range(0, query.Count).Where(i => (query[i] == "or" || query[i] == "OR")).ToList();
            foreach (Object o in list)
            {
                List<bool> results = new List<bool>();
                if (ands.Count != 0)
                {
                    results = bools(ands, o, query);
                    if (!results.Contains(false))
                        output.Add(o);
                }
                else if (ors.Count != 0)
                {
                    results = bools(ors, o, query);
                    if (results.Contains(true))
                        output.Add(o);
                }
                else if (WhereStatement(o, query))
                    output.Add(o);
            }
            return output;
        }
        static List<Object> Select(List<Object> list, List<string> query)
        {
            List<Object> output = new List<Object>();
            foreach (Object o in list)
            { 
                List<object> temp = new List<Object>();
                foreach (String s in query)
                {
                    String name = char.ToUpper(s[0]) + s.Replace(",", "").Substring(1);
                    try 
                    {
                        Object obj = o.GetType().GetProperty(name).GetValue(o, null);
                        
                        temp.Add(obj);
                    }
                    catch(Exception e) 
                    { }

                }
                if (temp.Count != 0)
                    output.Add(temp);
            }      
            return output;
        }
        static List<Object> SQLInterpreter(Data data, string query)
        {
            List<string> elements = query.Split(' ').ToList();
            int ids = elements.IndexOf("select");
            string fromquery = elements[1];
            List<string> wherequery = elements.GetRange(3, ids - 3);
            List<string> selectquery = elements.GetRange(ids + 1, elements.Count - ids - 1);
            List<Object> output = Select(Where(From(data, fromquery), wherequery), selectquery);
            return output;
        }
        static void Main(string[] args)
        {
            List<User> users = new List<User>();
            users.Add(new User("email", "name", 40));
            users.Add(new User("email2", "name2", 50));
            List<Order> orders = new List<Order>();
            orders.Add(new Order(0, "a", 2.5));
            Data data = new Data(users, orders);
            string query = "from Users where age >= 40 and email = 'email' select email, age";
            foreach (List<Object> obj in SQLInterpreter(data, query))
            {
                foreach (Object o in obj)
                    Console.Write(o + "; ");
                Console.WriteLine();
            }
        }
    }
}