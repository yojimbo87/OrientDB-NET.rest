using System.Net;

namespace Orient.Client.Protocol
{
    internal class Query
    {
        private string _apiUri { get { return "query/"; } }
        private OrientConnection _connection;

        internal Query(OrientConnection connection)
        {
            _connection = connection;
        }

        internal string Get(string language, string query, int limit, string fetchPlan)
        {
            var request = new Request();
            request.RelativeUri = _apiUri + _connection.Database + "/" + language + "/" + query;
            request.Method = RequestMethod.GET.ToString();
            //request.Realm = "OrientDB db-" + databaseName;

            var response = _connection.Process(request);

            string result = "";

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    result = response.JsonString;
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
