using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Aplikace.Data
{
    public class OracleDatabaseConnection : IDisposable
    {
        private readonly string connectionString;
        private OracleConnection connection;

        public OracleDatabaseConnection()
        {
            connectionString = "User Id=st67060;Password=abcde;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521))(CONNECT_DATA=(SID=BDAS)(SERVER=DEDICATED)))";
        }

        public void OpenConnection()
        {
            if (connection == null)
            {
                connection = new OracleConnection(connectionString);
            }

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public void Dispose()
        {
            CloseConnection();
            connection?.Dispose();
        }

        public DataTable ExecuteQuery(string query)
        {
            OpenConnection();

            using (var command = new OracleCommand(query, connection))
            using (var adapter = new OracleDataAdapter(command))
            {
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        public DataTable ExecuteQuery(string query, OracleParameter[] parameters = null)
        {
            OpenConnection();

            using (var command = new OracleCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var adapter = new OracleDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }
}

