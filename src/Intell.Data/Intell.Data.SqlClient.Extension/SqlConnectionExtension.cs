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


        public static int ExecuteNonQuery(this SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return command.ExecuteNonQuery();
            }
        }
        public static async Task<int> ExecuteNonQueryAsync(this SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return await command.ExecuteNonQueryAsync();
            }
        }

        public static SqlDataReader ExecuteReader(this SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return command.ExecuteReader();
            }
        }
        public static async Task<SqlDataReader> ExecuteReaderAsync(this SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return await command.ExecuteReaderAsync();
            }
        }

        public static object ExecuteScalar(this SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return command.ExecuteScalar();
            }
        }
        public static async Task<object> ExecuteScalarAsync(this SqlConnection connection, string commandText) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                return await command.ExecuteScalarAsync();
            }
        }
    }
}
