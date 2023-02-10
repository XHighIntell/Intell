using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intell.Data.SqlClient {
    public static class CommandGenerator {

        ///<summary>Gets literal format of c# struct or class.</summary>
        ///<remarks>If type is null, return "NULL".</remarks>
        public static string GetSqlLiteralFormat(object value, Type type) {

            if (type == null) return "NULL";
            else if (type == typeof(long)) return value.ToString();
            else if (type == typeof(int)) return value.ToString();
            else if (type == typeof(short)) return value.ToString();
            else if (type == typeof(byte)) return value.ToString();
            else if (type == typeof(decimal)) return value.ToString();
            else if (type == typeof(float)) return value.ToString();
            else if (type == typeof(bool)) return (bool)value == true ? "1" : "0";
            else if (type == typeof(DateTime)) return "'" + ((DateTime)value).ToString("s") + "'";
            else if (type == typeof(string)) return "'" + EscapeString(value.ToString()) + "'";
            else if (type == typeof(Guid)) return "'" + value.ToString() + "'";
            else if (type == typeof(DBNull)) return "NULL";
            else if (type == typeof(byte[])) return "0x" + ByteArrayToHexViaLookup32((byte[])value);
            else if (type.IsEnum == true) return Convert.ToInt32(value).ToString();

            else throw new Exception(@"Unsupport """ + type.ToString() + @""" type for getting sql literal format.");
        }
        ///<summary>Escapes the string for value.</summary>
        public static string EscapeString(string value) { return value.Replace("'", "''"); }
        ///<summary>Escapes the column name if nessesary. fn("key") return "[key]".</summary>
        public static string EscapeColumnName(string name) {
            if (name.StartsWith("[") == true && name.EndsWith("]") == true) return name;
            if (name.Equals("key", StringComparison.OrdinalIgnoreCase) == true) return "[key]";
            else {
                if (name.Contains("-")) return "[" + name + "]";
            }

            return name;
        }


        ///<summary>Generate SQL INSERT statement for <seealso cref="DataBase"/>.</summary>
        ///<param name="selectedColumns">Array name of columns that will be used. If the value of a column is null, skip and continue the next column. Use <see cref="DBNull"/> in case you want NULL.</param>
        public static string GenerateInsertCommand(this DataBase database, string tableName, string[] selectedColumns, bool allowNull = false, bool raiseErrorOnNull = true) {
            var names = new List<string>(5);
            var values = new List<string>(5);

            // INSERT INTO table(id,date) VALUES(3, "2023-02-3");
            // 1. loop through columns
            //      a. 
            //      b. 
            // 2. return empty string if there is nothing to insert
            // 3. generate command

            // --1--
            for (var i = 0; i < selectedColumns.Length; i++) {
                var name = selectedColumns[i];
                var value = database[name];

                if (value == null && allowNull == false) {
                    if (raiseErrorOnNull == false) continue;
                    else throw new Exception("Value of set column [" + name + "] can't be null.");
                }

                var escaped_name = EscapeColumnName(name);
                var literal = GetSqlLiteralFormat(value, value?.GetType());

                names.Add(escaped_name);
                values.Add(literal);
            }

            // --2--
            if (values.Count == 0) return "";

            // --3--
            var command = new StringBuilder(200);
            command.Append("INSERT INTO " + tableName + "(");
            command.Append(string.Join(",", names));
            command.Append(") VALUES ("); 
            command.Append(string.Join(",", values)); 
            command.Append(");");

            return command.ToString();
        }

        ///<summary>Generate SQL INSERT statement for array of <seealso cref="DataBase"/>.</summary>
        public static string GenerateInsertCommand(this DataBase[] dataBases, string tableName, string[] selectedColumns) {
            
            // INSERT INTO 
            // 1. return if array is empty
            // 

            // --1--
            if (dataBases == null || dataBases.Length == 0) return "";

            var command = new StringBuilder(200);
            command.Append("INSERT INTO " + tableName + "(");
            command.Append(string.Join(",", selectedColumns.Select((name) => { return EscapeColumnName(name); }).ToArray()));
            command.Append(") VALUES");

            for (var r = 0; r < dataBases.Length; r++) {
                var data = dataBases[r];
                var values = new List<string>(5);

                for (var i = 0; i < selectedColumns.Length; i++) {
                    var name = selectedColumns[i];
                    var value = data[name];
                    var literal = GetSqlLiteralFormat(value, value?.GetType());
                    
                    values.Add(literal);
                }

                command.Append("(");
                command.Append(string.Join(",", values));
                command.Append(")");

                if (r != dataBases.Length - 1) command.Append(",");
            }

            command.Append(";");

            return command.ToString();
        }

        ///<summary>Generate SQL UPDATE statement for <seealso cref="DataBase"/>.</summary>
        ///<param name="database"></param>
        ///<param name="tableName"></param>
        ///<param name="set_columns">Array name of columns that will be used. If the value of a column is null, throw <see cref="Exception"/>. Use <see cref="DBNull"/> in case you want NULL.</param>
        ///<param name="where_columns">Array name of columns that will be used on WHERE statement. If the value of a column is null, throw <see cref="Exception"/>. Use <see cref="DBNull"/> in case you want NULL.</param>
        ///<returns></returns>
        public static string GenerateUpdateCommand(DataBase database, string tableName, string[] set_columns, string[] where_columns) {

            // UPDATE TableName SET a=asd WHERE id1=asdasd AND id2=asdasd

            // 1. built-in throw to help users prevent mistakes
            //      a. without set, nothing will happen
            //      b. without where, the whole table will be destroyed
            // 2. set ... where ...

            // --1--
            if (set_columns == null || set_columns.Length == 0) throw new Exception(nameof(set_columns) + " cannot be null or empty.");
            if (where_columns == null || where_columns.Length == 0) throw new Exception(nameof(where_columns) + " cannot be null or empty.");

            // --2--
            var setCommand = GenerateSetCommand(database, set_columns);
            var whereComand = GenerateWhereCommand(database, where_columns);

            // --3--
            var command = new StringBuilder(200);
            command.Append("UPDATE " + tableName + " SET ");
            command.Append(setCommand);
            command.Append(" WHERE ");
            command.Append(whereComand);
            command.Append(";");

            return command.ToString();
        }

        ///<summary>Generate SQL DELETE statement for <seealso cref="DataBase"/>.</summary>
        ///<param name="database"></param>
        ///<param name="tableName"></param>
        ///<param name="where_columns">Array name of columns that will be used on WHERE statement. If the value of a column is null, throw <see cref="Exception"/>. Use <see cref="DBNull"/> in case you want NULL.</param>
        public static string GenerateDeleteCommand(DataBase database, string tableName, string[] where_columns, bool allowNull = false, bool raiseErrorOnNull = true) {

            // DELETE FROM table WHERE id1=asdasd AND id2=asdasd

            // 1. built-in throw to help users prevent mistakes
            //      b. without where, the whole table will be deleted
            // 2. 

            // --1--
            if (where_columns == null || where_columns.Length == 0) throw new Exception(nameof(where_columns) + " cannot be null or empty.");
            
            // --2--
            var whereComand = GenerateWhereCommand(database, where_columns, allowNull, raiseErrorOnNull);

            return "DELETE FROM " + tableName + " WHERE " + whereComand + ";";
        }

        static string GenerateSetCommand(DataBase database, string[] set_columns,  bool allowSetNull = false, bool raiseErrorOnNull = false) {
            return GenerateEqualCommand(database, set_columns, ",", allowSetNull, raiseErrorOnNull);
        }
        static string GenerateWhereCommand(DataBase database, string[] set_columns, bool allowSetNull = false, bool raiseErrorOnNull = false) {
            return GenerateEqualCommand(database, set_columns, " AND ", allowSetNull, raiseErrorOnNull);
        }
        static string GenerateEqualCommand(DataBase database, string[] set_columns, string separator, bool allowSetNull = false, bool raiseErrorOnNull = false) {
            /*
            Built-in throw Exception when:
                - value[selectedColumns[i]] is null
            OUTPUT:
                id=1{separator}status=2{separator}c=NULL
                id=1,status=2,c=NULL
                id=1 AND status=2 AND c=NULL
            */

            var set_params = new List<string>(7);

            for (var i = 0; i < set_columns.Length; i++) {
                var name = set_columns[i];
                var value = database[name];

                if (value == null && allowSetNull == false) {
                    if (raiseErrorOnNull == false) continue;
                    else throw new Exception("Value of set column [" + name + "] can't be null.");
                }

                var escaped_name = EscapeColumnName(name);
                var literal = GetSqlLiteralFormat(value, value?.GetType());

                set_params.Add(escaped_name + "=" + literal);
            }

            return string.Join(separator, set_params);
        }
        static readonly uint[] _lookup32 = CreateLookup32();
        static uint[] CreateLookup32() {
            var result = new uint[256];
            for (int i = 0; i < 256; i++) {
                string s = i.ToString("X2");
                result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
            }
            return result;
        }
        static string ByteArrayToHexViaLookup32(byte[] bytes) {
            var lookup32 = _lookup32;
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++) {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }
    }
}
