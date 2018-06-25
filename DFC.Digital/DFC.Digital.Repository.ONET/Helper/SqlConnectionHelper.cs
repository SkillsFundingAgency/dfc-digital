using System;
using System .Configuration;

namespace DFC.Digital.Repository.ONET.Helper
{
    using System.Data;
    using System.Data.SqlClient;

    /// <devdoc>
    /// </devdoc>
    public static class SqlConnectionHelper
    {
        internal const string SqlErrorConnectionString = "An error occurred while attempting to initialize a System.Data.SqlClient.SqlConnection object. The value that was provided for the connection string may be wrong, or it may contain an invalid syntax.";
        private static object _sLock = new object ( );
        private static string _cachedConnectionString;


        public static SqlConnectionHolder GetConnection ( string connectionString )
        {
            var strTempConnection = connectionString .ToUpperInvariant ( );
            var holder = new SqlConnectionHolder ( connectionString );
            bool closeConn = true;
            try
            {
                holder .Open ( );
                closeConn = false;
            }
            finally
            {
                if ( closeConn )
                {
                    holder .Close ( );
                    holder = null;
                }
            }
            return holder;
        }


        public static string GetConnectionString ( string specifiedConnectionString , bool lookupConnectionString )
        {
            if ( string .IsNullOrEmpty ( specifiedConnectionString ) )
                return null;

            string connectionString = null;

            /////////////////////////////////////////
            // Step 1: Check <connectionStrings> config section for this connection string
            if ( lookupConnectionString || string .IsNullOrEmpty ( _cachedConnectionString ) )
            {
                var connObj = ConfigurationManager .ConnectionStrings [ specifiedConnectionString ];
                if ( connObj != null )
                {
                    connectionString = connObj .ConnectionString;
                    _cachedConnectionString = connectionString;
                }
                if ( connectionString == null )
                    return null;

            }
            else if ( !string .IsNullOrEmpty ( _cachedConnectionString ) )
            {
                connectionString = _cachedConnectionString;
            }

            return connectionString;
        }
    }


    public sealed class SqlConnectionHolder : IDisposable
    {
        private readonly SqlConnection _connection;
        private bool _opened;

        public SqlConnection Connection
        {
            get { return _connection; }
        }


        internal SqlConnectionHolder ( string connectionString )
        {
            try
            {
                _connection = new SqlConnection ( connectionString );
            }
            catch ( ArgumentException e )
            {
                throw new ArgumentException ( SqlConnectionHelper .SqlErrorConnectionString , "connectionString" , e );
            }
        }

        ~SqlConnectionHolder ( )
        {
            Dispose ( true );
        }

       internal void Open ( )
        {
            if ( _opened )
            {
                if ( _connection .State != ConnectionState .Closed && _connection .State != ConnectionState .Broken )
                {
                    return; // Already opened
                }
                _connection .Close ( );
            }

            Connection .Open ( );

            _opened = true; // Open worked!
        }


        internal void Close ( )
        {
            if ( !_opened ) // Not open!
                return;
            // Close connection
            Connection .Close ( );
            _opened = false;
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ( )
        {
            Dispose ( true );
            GC .SuppressFinalize ( this );
        }

        /// <summary>
        /// Disposes the managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose ( bool disposing )
        {
            if ( !disposing )
                return;

            if ( _disposed )
                return;

            Close ( );
            //_repository.Context.Dispose();
            _disposed = true;
        }
        private bool _disposed;
        #endregion

    }
}
