using System.Net;

namespace Orient.Client.Protocol
{
    internal class Connect
    {
        private string _apiUri { get { return "connect/"; } }
        private OrientConnection _connection;

        internal Connect(OrientConnection connection)
        {
            _connection = connection;
        }

        /*internal OrientDatabase Get(string databaseName)
        {
            var request = new Request();
            request.RelativeUri = _apiUri + databaseName;
            request.Method = RequestMethod.GET.ToString();
            request.Realm = "OrientDB db-" + databaseName;

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
        }*/
    }
}
