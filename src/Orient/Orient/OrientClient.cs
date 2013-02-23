using System.Collections.Generic;
using System.Linq;
using Orient.Client.Protocol;

namespace Orient.Client
{
    public static class OrientClient
    {
        #region Properties

        public static string DriverName
        {
            get { return "OrientDB-NET.rest"; }
        }

        public static string DriverVersion
        {
            get { return "Alpha 1.0"; }
        }

        /// <summary>
        /// Collection of nodes which consists of database connection parameters identified by unique alias string.
        /// </summary>
        public static List<OrientNode> Nodes { get; set; }

        #endregion

        static OrientClient()
        {
            Nodes = new List<OrientNode>();
        }

        internal static OrientNode GetNode(string alias)
        {
            return Nodes.Where(node => node.Alias == alias).FirstOrDefault();
        }

        public static OrientDatabase Connect(string alias, string databaseName)
        {
            Connect connect = new Connect(GetNode(alias));
            
            return connect.Get(databaseName);
        }
    }
}
