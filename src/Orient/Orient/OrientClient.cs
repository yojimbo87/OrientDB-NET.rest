using System.Collections.Generic;
using System.Linq;
using Orient.Client.Protocol;

namespace Orient.Client
{
    public static class OrientClient
    {
        #region Properties

        private static List<OrientConnection> _connections;

        public static string DriverName
        {
            get { return "OrientDB-NET.rest"; }
        }

        public static string DriverVersion
        {
            get { return "Alpha 1.0"; }
        }

        #endregion

        static OrientClient()
        {
            _connections = new List<OrientConnection>();
        }

        public static void AddConnection(string server, int port, bool isSecured, string userName, string password, string database, string alias)
        {
            OrientConnection connection = new OrientConnection(server, port, isSecured, userName, password, database, alias);

            _connections.Add(connection);
        }

        internal static OrientConnection GetConnection(string alias)
        {
            return _connections.Where(connection => connection.Alias == alias).FirstOrDefault();
        }
    }
}
