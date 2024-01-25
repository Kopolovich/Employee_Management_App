using System.Text.RegularExpressions;

namespace Stage0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            welcome0464();
            welcome1940();
            Console.ReadKey();
        }

        static partial void welcome1940();
        private static void welcome0464()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}

