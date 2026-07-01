using Microsoft.Data.SqlClient;

namespace Intell.Data.SqlClient; 

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

    ///<summary>[Extension] Executes a Transact-SQL statement against the connection and returns the number of rows affected.</summary>
    public static int ExecuteNonQuery(this SqlConnection connection, string commandText) {
        using var command = new SqlCommand(commandText, connection);
        return command.ExecuteNonQuery();
    }

    ///<summary>[Extension] Executes a Transact-SQL statement against the connection and returns the number of rows affected.</summary>
    public static async Task<int> ExecuteNonQueryAsync(this SqlConnection connection, string commandText) {
        using var command = new SqlCommand(commandText, connection);
        return await command.ExecuteNonQueryAsync();
    }

    ///<summary>[Extension] Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.</summary>
    public static object ExecuteScalar(this SqlConnection connection, string commandText) {
        using var command = new SqlCommand(commandText, connection);
        return command.ExecuteScalar();
    }

    ///<summary>[Extension] Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.</summary>
    public static async Task<object?> ExecuteScalarAsync(this SqlConnection connection, string commandText) {
        using var command = new SqlCommand(commandText, connection);
        return await command.ExecuteScalarAsync();
    }
}
