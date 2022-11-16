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
            string[]selectquery = new string[elements.Length - ids - 1];
            Array.Copy(elements, ids + 1, wherequery, 0, elements.Length - ids - 1);
            List<Object> output = Select(Where(From(data, fromquery), wherequery), selectquery);
            return output.ToString() ?? "null";
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Query Engine");
        }
    }
}