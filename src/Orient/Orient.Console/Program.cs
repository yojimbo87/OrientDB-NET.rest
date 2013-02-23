using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using Orient.Client;

namespace Orient.Console
{
    class Program
    {
        static string _alias = "test";

        static void Main(string[] args)
        {
            OrientNode node = new OrientNode(
                "localhost",
                int.Parse("2480"),
                false,
                "admin",
                "admin",
                _alias
            );
            OrientClient.Nodes.Add(node);

            TestConnect();

            System.Console.ReadLine();
        }

        static void TestConnect()
        {
            OrientDatabase database = OrientClient.Connect(_alias, "TinkerPop");

            database.PrintDump();
        }
    }
}
