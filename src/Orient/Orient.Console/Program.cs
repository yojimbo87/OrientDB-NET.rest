using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using Orient.Client;

using System.Net;
using System.IO;

namespace Orient.Console
{
    class Program
    {
        static string _alias = "test";

        static void Main(string[] args)
        {
            OrientNode node = new OrientNode(
                "localhost.",
                int.Parse("2480"),
                false,
                "admin",
                "admin",
                _alias
            );
            OrientClient.Nodes.Add(node);

            //TestConnect();
            //TestConnect();
            //TestConnect();
            TestAuth();

            System.Console.ReadLine();
        }

        static void TestConnect()
        {
            OrientDatabase database = OrientClient.Connect(_alias, "TinkerPop");

            //database.PrintDump();
        }

        static void TestAuth()
        {
            string url = "http://localhost.:2480/connect/Tinkerpop";
            HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
            WebResponse resp;
            StreamReader reader;


            // TODO: is the realm necessary when the keep-alive connection is used?
            string user = "admin";
            string pwd = "admin";
            string domain = "OrientDB db-TinkerPop";

            string auth = "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(user + ":" + pwd));
            req.KeepAlive = true;
            req.Headers.Add("Authorization", auth);
            req.UserAgent = "whoa";
            resp = req.GetResponse();
            reader = new StreamReader(resp.GetResponseStream());

            System.Console.WriteLine(reader.ReadToEnd().Length);
            resp.Close();


            req = HttpWebRequest.Create(url) as HttpWebRequest;
            req.KeepAlive = true;
            req.Credentials = new NetworkCredential(user, pwd, domain);
            req.UserAgent = "whoa";
            resp = req.GetResponse();
            reader = new StreamReader(resp.GetResponseStream());

            System.Console.WriteLine(reader.ReadToEnd().Length);
            //resp.Close();

            req = HttpWebRequest.Create(url) as HttpWebRequest;
            req.KeepAlive = true;
            req.Credentials = new NetworkCredential(user, pwd, domain);
            req.UserAgent = "whoa";
            resp = req.GetResponse();
            reader = new StreamReader(resp.GetResponseStream());

            System.Console.WriteLine(reader.ReadToEnd().Length);
        }
    }
}
