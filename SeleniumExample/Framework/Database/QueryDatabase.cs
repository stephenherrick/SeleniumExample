using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Framework.Database
{
    public class QueryDatabase
    {
        private static string ConnString;

        /// <summary>
        /// Query data from a database.
        /// </summary>
        /// <param name="ConnectionString">Include the full connection string.</param>
        public QueryDatabase(string ConnectionString)
        {
            ConnString = ConnectionString;
        }

        /// <summary>
        /// Executes query on the specified database
        /// </summary>
        /// <param name="query">SQL Query</param>
        /// <param name="NumberOfResults">Returns number of rows returned from the query</param>
        /// <returns>Returns data table with complete result of query.</returns>
        public DataTable QueryTableResults(string query, out int NumberOfResults)
        {
            DataTable Table = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnString))
            using (SqlCommand command = new SqlCommand(query, conn))
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                dataAdapter.Fill(Table);
                NumberOfResults = (Table == null) ? 0 : Table.Rows.Count;
                return Table;
            }
        }

        /// <summary>
        /// Executes Insert, Update, Delete in database
        /// </summary>
        /// <param name="query"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int ExecuteCommand(string query, string action)
        {
            using (SqlConnection conn = new SqlConnection(ConnString))
            using (SqlCommand command = new SqlCommand(query, conn))
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                int numberOfRecords = 0;

                switch (action)
                {
                    case "Update":

                        dataAdapter.UpdateCommand = command;
                        numberOfRecords = dataAdapter.UpdateCommand.ExecuteNonQuery();
                        break;
                    case "Insert":
                        dataAdapter.InsertCommand = command;
                        numberOfRecords = dataAdapter.InsertCommand.ExecuteNonQuery();
                        break;
                    case "Delete":
                        dataAdapter.DeleteCommand = command;
                        numberOfRecords = dataAdapter.DeleteCommand.ExecuteNonQuery();
                        break;
                }
                return numberOfRecords;
            }
        }



        /// <summary>
        /// This executes a stored procedure on the specified database
        /// </summary>
        /// <param name="storedProcedure">Name of the stored procedure on the database to be executed</param>
        /// <param name="NumberOfResults">Number of rows returned after execution of the stored procedure</param>
        /// <param name="parameterNameList">List of parameter names, e.g. "@FirstName"</param>
        /// <param name="parameterValueList">List of arguments for the corresponding parameters</param>
        /// <returns></returns>
        public DataTable ExecuteStoredProcedure(string storedProcedure, out int NumberOfResults, List<string> parameterNameList = null, List<string> parameterValueList = null)
        {
            DataTable Table = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnString))
            using (SqlCommand command = new SqlCommand(storedProcedure, conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameterNameList != null)
                {
                    for (int i = 0; i < parameterNameList.Count; i++)
                    {
                        command.Parameters.AddWithValue(parameterNameList[i], parameterValueList[i]);
                    }
                }

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.Fill(Table);
                    NumberOfResults = (Table == null) ? 0 : Table.Rows.Count;
                    return Table;
                }
            }
        }
    }
}
