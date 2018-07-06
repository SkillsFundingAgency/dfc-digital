namespace DFC.Digital.Repository.ONET.Helper
{
    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public DbConnectionFactory(string connectionString = "name=MetaSchemaRepositoryContext")
        {
            if(connectionString != null)
            {
                var match = Regex.Match(connectionString, @"^name=([a-zA-Z0-9]+)", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(1000));
                if(match.Success)
                {
                    var configName = match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).FirstOrDefault();
                    _connectionString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;
                }
                else
                {
                    _connectionString = connectionString;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
        }
        public DbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}