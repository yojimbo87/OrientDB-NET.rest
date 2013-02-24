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
            OrientClient.AddConnection(
                "localhost.",
                int.Parse("2480"),
                false,
                "admin",
                "admin",
                "TinkerPop",
                _alias    
            );

            //TestConnect();
            //TestConnect();
            //TestConnect();
            TestQuery();
            //TestAuth();

            /*SetTimeout(() =>
            {
                TestQuery();
            }, 9000);

            SetTimeout(() =>
            {
                TestQuery();
            }, 20000);*/

            System.Console.WriteLine("\nEND");
            System.Console.ReadLine();
        }

        static void TestConnect()
        {
            //OrientDatabase database = OrientClient.Connect(_alias, "TinkerPop");

            //database.PrintDump();
        }

        static void TestQuery()
        {
            //OrientDatabase database = new OrientDatabase(_alias);

            //System.Console.WriteLine(database.Query("sql", "select from OGraphVertex"));

            long total = 0;

            for (int i = 0; i < 50; i++)
            {
                long tps = Do();
                total += tps;

                System.Console.WriteLine("TPS: " + tps);
            }

            System.Console.WriteLine("Average: " + total / 50);
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

        static long Do()
        {
            DateTime start = DateTime.Now;
            bool running = true;
            long tps = 0;

            do
            {
                OrientDatabase database = new OrientDatabase(_alias);

                //string s = database.Query("sql", "select name from ographvertex where in[0].label = 'followed_by' and in[0].out.name = 'JAM'");
                string s = database.Query("sql", "select from ographedge");
                tps++;

                TimeSpan dif = DateTime.Now - start;

                if (dif.TotalMilliseconds > 1000)
                {
                    running = false;
                }
            }
            while (running);

            return tps;
        }

        #region Timers

        static IDisposable SetInterval(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.Enabled = true;
            timer.Start();

            // Returns a stop handle which can be used for stopping
            // the timer, if required
            return timer as IDisposable;
        }

        static IDisposable SetTimeout(Action method, int delayInMilliseconds)
        {
            System.Timers.Timer timer = new System.Timers.Timer(delayInMilliseconds);
            timer.Elapsed += (source, e) =>
            {
                method();
            };

            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();

            // Returns a stop handle which can be used for stopping
            // the timer, if required
            return timer as IDisposable;
        }

        #endregion
    }
}
