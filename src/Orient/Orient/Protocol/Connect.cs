using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using ServiceStack.Text;

namespace Orient.Client.Protocol
{
    internal class Connect
    {
        private string _apiUri { get { return "connect/"; } }
        private OrientNode _node;

        internal Connect(OrientNode node)
        {
            _node = node;
        }

        internal OrientDatabase Get(string databaseName)
        {
            var request = new Request();
            request.RelativeUri = _apiUri + databaseName;
            request.Method = RequestMethod.GET.ToString();

            var response = _node.Process(request);

            OrientDatabase database = new OrientDatabase();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    database.Version = response.JsonObject.Get("server.version");
                    database.OsName = response.JsonObject.Get("server.osName");
                    database.OsVersion = response.JsonObject.Get("server.osVersion");
                    database.OsArchitecture = response.JsonObject.Get("server.osArch");
                    database.JavaVendor = response.JsonObject.Get("server.javaVendor");
                    database.JavaVersion = response.JsonObject.Get("server.javaVersion");
                    break;
                default:
                    break;
            }

            return database;
        }
    }
}
