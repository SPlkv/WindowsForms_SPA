using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace SUBDLab6
{
    class DataBase
    {
        static string connectionString = @"Data Source=LAPTOP-15G6O1UK\SQLEXPRESS;Initial Catalog=SPA;Integrated Security=True";
        SqlConnection sqlConnection = new SqlConnection(connectionString);

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
        }
        public void closedConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                sqlConnection.Close();
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
