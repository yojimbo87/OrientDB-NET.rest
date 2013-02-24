using Orient.Client.Protocol;

namespace Orient.Client
{
    public class OrientDatabase
    {
        private OrientConnection _connection;

        public OrientDatabase(string alias)
        {
            _connection = OrientClient.GetConnection(alias);
        }

        /*public void Connect(string alias, string databaseName)
        {
            Connect operation = new Connect(_connection);

            return operation.Get(databaseName);
        }*/

        public string Query(string language, string query)
        {
            Query operation = new Query(_connection);

            return operation.Get(language, query, 20, "*:1");
        }
    }
}
