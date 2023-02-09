using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Intell.Data.SqlClient.Extension {

    ///<summary>The extension class for <see cref="SqlConnection"/>.</summary>
    public static class SqlConnectionExtension {
        ///<summary>Creates and returns a <see cref="SqlCommand"/> object associated with the <see cref="SqlConnection"/>.</summary>
        public static SqlCommand CreateCommand(this SqlConnection connection, string commandText) {
            var cmd = connection.CreateCommand();
            cmd.CommandText = commandText;

            return cmd;
        }

        ///<summary>Creates and returns a <see cref="SqlCommand"/> object associated with the <see cref="SqlConnection"/>.</summary>
        public static SqlCommand CreateCommand(this SqlConnection connection, string commandText, int commandTimeout) {
            var cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandTimeout = commandTimeout;
            
            return cmd;
        }


        public static int ExecuteNonQuery(SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return command.ExecuteNonQuery();
            }
        }
        public static async Task<int> ExecuteNonQueryAsync(SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return await command.ExecuteNonQueryAsync();
            }
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return command.ExecuteReader();
            }
        }
        public static async Task<SqlDataReader> ExecuteReaderAsync(SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return await command.ExecuteReaderAsync();
            }
        }

        public static object ExecuteScalar(SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return command.ExecuteScalar();
            }
        }
        public static async Task<object> ExecuteScalarAsync(SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return await command.ExecuteScalarAsync();
            }
        }
    }
}
