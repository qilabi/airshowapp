using System.Configuration;
using System.Data;
using ServiceStack.OrmLite;

namespace Senparc.Weixin.MP.Sample.CommonService.Data
{
    public class MSSQLProvider
    {
        private static OrmLiteConnectionFactory _connectionFactory;

        /// <summary>
        /// instance
        /// </summary>
        private static readonly MSSQLProvider Instance = new MSSQLProvider();

        public static MSSQLProvider CreateConnection()
        {
            return Instance;
        }

        /// <summary>
        /// Default constructor which uses the "DefaultConnection" connectionString
        /// </summary>
        public MSSQLProvider()
            : this("airshowConnection")
        {
        }

        /// <summary>
        /// Constructor which takes the connection string name
        /// </summary>
        /// <param name="connectionStringName"></param>
        public MSSQLProvider(string connectionStringName)
        {
            if (_connectionFactory != null)
                return;
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _connectionFactory = new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider);
        }
        
        public IDbConnection Open()
        {
            return _connectionFactory.OpenDbConnection();
        }
    }
}
