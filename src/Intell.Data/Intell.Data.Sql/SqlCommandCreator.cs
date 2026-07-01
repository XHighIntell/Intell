using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace Intell.Data.Sql;

///<summary>Class that contain helper methods for generating T-SQL command.</summary>
public static class SqlCommandCreator {
    ///<summary>Contains all reserved keywords for T-SQL (SQL Server 2025 v17).</summary>
    ///<remarks>https://learn.microsoft.com/en-us/sql/t-sql/language-elements/reserved-keywords-transact-sql?view=sql-server-ver17</remarks>
    static readonly string[] Keywords = [
        "add","all","alter","and","any","as","asc","authorization","backup","begin","between","break","browse","bulk","by","cascade","case","check",
        "checkpoint","close","clustered","coalesce","collate","column","commit","compute","constraint","contains","containstable","continue","convert",
        "create","cross","current","current_date","current_time","current_timestamp","current_user","cursor","database","dbcc","deallocate","declare",
        "default","delete","deny","desc","disk","distinct","distributed","double","drop","dump","else","end","errlvl","escape","except","exec","execute",
        "exists","exit","external","fetch","file","fillfactor","for","foreign","freetext","freetexttable","from","full","function","goto","grant",
        "group","having","holdlock","identity","identity_insert","identitycol","if","in","index","inner","insert","intersect","into","is","join","key",
        "kill","left","like","lineno","load","merge","national","nocheck","nonclustered","not","null","nullif","of","off","offsets","on","open",
        "opendatasource","openquery","openrowset","openxml","option","or","order","outer","over","percent","pivot","plan","precision","primary","print",
        "proc","procedure","public","raiserror","read","readtext","reconfigure","references","replication","restore","restrict","return","revert",
        "revoke","right","rollback","rowcount","rowguidcol","rule","save","schema","securityaudit","select","semantickeyphrasetable",
        "semanticsimilaritydetailstable","semanticsimilaritytable","session_user","set","setuser","shutdown","some","statistics","system_user","table",
        "tablesample","textsize","then","to","top","tran","transaction","trigger","truncate","try_convert","tsequal","union","unique","unpivot",
        "update","updatetext","use","user","values","varying","view","waitfor","when","where","while","with","within","writetext"
    ];

    ///<summary>Indicates whether the word is a T-SQL keyword.</summary>
    public static bool IsKeyword(string word) => Keywords.Contains(word.ToLower());

    ///<summary>Returns the literal representation of a class or primitive value.</summary>
    ///<remarks>If type is null, return "NULL".</remarks>
    public static string GetSqlLiteral(object? value) {
        if (value == null) return "NULL";
        var type = value.GetType();

        if (type == null) return "NULL";
        else if (type == typeof(long)) return ((long)value).ToString();
        else if (type == typeof(int)) return ((int)value).ToString();
        else if (type == typeof(short)) return ((short)value).ToString();
        else if (type == typeof(byte)) return ((byte)value).ToString();
        else if (type == typeof(decimal)) return ((decimal)value).ToString();
        else if (type == typeof(float)) return ((float)value).ToString();
        else if (type == typeof(bool)) return (bool)value == true ? "1" : "0";
        else if (type == typeof(DateTime)) return $"'{(DateTime)value:s}'";
        else if (type == typeof(string)) return $"N'{EscapeString((string)value)}'";
        else if (type == typeof(Guid)) return $"'{value}'";
        else if (type == typeof(DBNull)) return "NULL";
        else if (type == typeof(byte[])) return $"0x{Convert.ToHexString((byte[])value)}";
        else if (type.IsEnum == true) return Convert.ToInt32(value).ToString();

        else throw new Exception(@$"Type '{type}' is not supported for SQL literal formatting.");
    }

    ///<summary>Escapes the specified string value.</summary>
    public static string EscapeString(string value) { return value.Replace("'", "''"); }

    ///<summary>Escapes identifiers and object names when necessary.</summary>
    ///<remarks>
    /// key → [key]<br/>
    /// email → email<br/>
    /// [name] → [name]<br/>
    /// user name → [user name]<br/>
    ///</remarks>
    public static string EscapeIdentifier(string name) {
        if (name.StartsWith('[') == true && name.EndsWith(']') == true) return name;
        else if (name.Contains('-') || name.Contains(' ') || Keywords.Contains(name.ToLower()))
            return $"[{name}]";

        return name;
    }

    ///<summary>Generates the assignment clause from specified dictionary.</summary>
    ///<param name="selectKeys">List of selected dictionary keys.</param>
    ///<returns>Examples:
    /// id=1{separator}status=2{separator}c=NULL<br/>
    /// id=1,status=2,c=NULL<br/>
    /// id = 1 AND status = 2 AND c = NULL<br/>
    ///</returns>
    public static string GenerateAssignment(Dictionary<string, object?> dictionary, string[] selectKeys, string separator, SqlNullHandling nullHandling) {
        /*
        Built-in throw Exception when:
            - value[selectedColumns[i]] is null
        OUTPUT:
            id=1{separator}status=2{separator}c=NULL
            id=1,status=2,c=NULL
            id=1 AND status=2 AND c=NULL
        */

        var clauses = new List<string>(7);

        for (var i = 0; i < selectKeys.Length; i++) {
            var name = selectKeys[i];
            dictionary.TryGetValue(name, out object? value);

            if (value == null) {
                if (nullHandling == SqlNullHandling.Disallow) throw new Exception($"The value for the SET column [{name}] cannot be null.");
                else if (nullHandling == SqlNullHandling.Skip) continue;
                else if (nullHandling == SqlNullHandling.Allow) value = DBNull.Value;
                else throw new NotImplementedException($"The SqlNullHandling value '{nullHandling}' is not supported for {nameof(GenerateAssignment)}.");
            }

            var escaped_name = EscapeIdentifier(name);
            var literal = GetSqlLiteral(value);

            clauses.Add($"{escaped_name}={literal}");
        }

        return string.Join(separator, clauses);
    }

    ///<summary>Generates an INSERT INTO statement from the specified dictionary.</summary>
    ///<param name="keys">The array of dictionary keys that are used to generate the SET/VALUES clause.</param>
    public static string GenerateInsertStatement(string tablename, Dictionary<string, object?> dictionary, string[] keys, SqlNullHandling nullHandling = SqlNullHandling.Disallow) {
        var names = new List<string>(keys.Length);
        var values = new List<string>(keys.Length);
        
        // INSERT INTO table(id,date) VALUES(3, "2023-02-3");
        // 1. loop through columns
        //      a. 
        //      b. 
        // 2. return empty string if there is nothing to insert
        // 3. generate command

        // --1--
        for (var i = 0; i < keys.Length; i++) {
            var name = keys[i];
            dictionary.TryGetValue(name, out object? value);

            if (value == null) {
                if (nullHandling == SqlNullHandling.Disallow) throw new Exception($"The value for the SET column [{name}] cannot be null.");
                else if (nullHandling == SqlNullHandling.Skip) continue;
                else if (nullHandling == SqlNullHandling.Allow) value = DBNull.Value;
                else throw new NotImplementedException($"The SqlNullHandling value '{nullHandling}' is not supported for {nameof(GenerateInsertStatement)}.");
            }

            var escaped_name = EscapeIdentifier(name);
            var literal = GetSqlLiteral(value);

            names.Add(escaped_name);
            values.Add(literal);
        }

        // --2--
        if (values.Count == 0) return "";

        // --3--
        var command = new StringBuilder(200);
        command.Append("INSERT INTO " + tablename + "(");
        command.Append(string.Join(",", names));
        command.Append(") VALUES (");
        command.Append(string.Join(",", values));
        command.Append(");");

        return command.ToString();
    }

    ///<summary>Generates an INSERT INTO statement from an array of dictionary.</summary>
    ///<param name="keys">The array of dictionary keys that are used to generate the SET/VALUES clause.</param>
    ///<param name="nullHandling">Permitted values are <see cref="SqlNullHandling.Disallow"/> or <see cref="SqlNullHandling.Allow"/>.</param>
    public static string GenerateInsertStatement(string tablename, Dictionary<string, object?>[] dictionaries, string[] keys, SqlNullHandling nullHandling = SqlNullHandling.Disallow) {

        // INSERT INTO 
        // 1. return if array is empty
        // 

        // --1--
        if (dictionaries == null || dictionaries.Length == 0) return "";

        var command = new StringBuilder(200);
        command.Append("INSERT INTO " + tablename + "(");
        command.Append(string.Join(",", keys.Select(EscapeIdentifier)));
        command.Append(") VALUES");

        for (var r = 0; r < dictionaries.Length; r++) {
            var dict = dictionaries[r];
            var values = new List<string>(5);

            for (var i = 0; i < keys.Length; i++) {
                var name = keys[i];
                dict.TryGetValue(name, out object? value);

                if (value == null) {
                    if (nullHandling == SqlNullHandling.Disallow) throw new Exception($"The value for the SET column [{name}] cannot be null.");
                    else if (nullHandling == SqlNullHandling.Skip) throw new ArgumentException($"The SqlNullHandling value '{SqlNullHandling.Skip} is not allowed in the multi-row SQL generator.");
                    else if (nullHandling == SqlNullHandling.Allow) value = DBNull.Value;
                    else throw new NotImplementedException($"The SqlNullHandling value '{nullHandling}' is not supported for {nameof(GenerateInsertStatement)}.");
                }

                var literal = GetSqlLiteral(value);

                values.Add(literal);
            }

            command.Append('(');
            command.Append(string.Join(',', values));
            command.Append(')');

            if (r != dictionaries.Length - 1) command.Append(',');
        }

        command.Append(';');

        return command.ToString();
    }

    ///<summary>Generate an UPDATE statement from the specified dictionary.</summary>
    ///<param name="set_keys">The array of dictionary keys that are used to generate the SET clause.</param>
    ///<param name="where_keys">The array of dictionary keys that are used to generate the WHERE clause.</param>
    public static string GenerateUpdateStatement(string tableName, Dictionary<string, object?> dictionary, string[] set_keys, string[] where_keys, SqlNullHandling nullHandling = SqlNullHandling.Disallow) {

        // UPDATE TableName SET a=asd WHERE id1=asdasd AND id2=asdasd

        // 1. built-in throw to help users prevent mistakes
        //      a. without set, nothing will happen
        //      b. without where, the whole table will be destroyed
        // 2. set ... where ...

        // --1--
        if (set_keys == null || set_keys.Length == 0) throw new Exception(nameof(set_keys) + " cannot be null or empty.");
        if (where_keys == null || where_keys.Length == 0) throw new Exception(nameof(where_keys) + " cannot be null or empty.");

        // --2--
        var setCommand = GenerateAssignment(dictionary, set_keys, ",", nullHandling);
        var whereComand = GenerateAssignment(dictionary, where_keys, " AND ", SqlNullHandling.Disallow);

        // --3--
        var command = new StringBuilder(200);
        command.Append("UPDATE " + tableName + " SET ");
        command.Append(setCommand);
        command.Append(" WHERE ");
        command.Append(whereComand);
        command.Append(';');

        return command.ToString();
    }

    ///<summary>Generates a DELETE statement.</summary>
    ///<param name="whereKeys">The array of dictionary keys that are used to generate the WHERE clause.</param>
    ///<returns>DELETE FROM table WHERE id1=1d AND email='asdasd'</returns>
    public static string GenerateDeleteStatement(string tableName, Dictionary<string, object?> dictionary, string[] whereKeys, SqlNullHandling nullHandling = SqlNullHandling.Allow) {
        // 1. built-in throw to help users prevent mistakes
        //      b. without where, the whole table will be deleted
        // 2. 

        // --1--
        if (whereKeys == null || whereKeys.Length == 0) throw new Exception(nameof(whereKeys) + " cannot be null or empty.");

        // --2--
        tableName = EscapeIdentifier(tableName);
        var whereComand = GenerateAssignment(dictionary, whereKeys, " AND ", nullHandling);

        // DELETE FROM table WHERE id1=asdasd AND id2=asdasd
        return $"DELETE FROM {tableName} WHERE {whereComand};";
    }


}
