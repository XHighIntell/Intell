using System;
using System.Collections.Generic;
using System.Data.Common;

// Data

namespace Intell.Data {

    ///<summary>The <see cref="DataBase"/> class implements the generic data interface.</summary>
    public class DataBase {

        private Dictionary<string, object> columns = new Dictionary<string, object>(10, StringComparer.OrdinalIgnoreCase);

        ///<summary>Initializes a new instance of <see cref="DataBase"/> without data.</summary>
        public DataBase() { }

        ///<summary>Initializes a new instance of <see cref="DataBase"/> with specified columns from the current row of DbDataReader.</summary>
        public DataBase(DbDataReader reader) { ReadFromReader(reader); }

        protected void ReadFromReader(DbDataReader reader) {
            if (columns.Count != 0) throw new Exception(nameof(DataBase) + " already load data.");

            for (int i = 0; i < reader.FieldCount; i++) {
                string columnName = reader.GetName(i);
                object columnValue = reader.GetValue(i);
                columns.Add(columnName, columnValue);
            }
        }

        ///<summary>Gets or sets value of column from its name.</summary>
        ///<param name="columnName">The column name.</param>
        public virtual object this[string columnName] {
            get {
                if (columns.ContainsKey(columnName) == true) return columns[columnName];
                else return null;
            }
            set {
                columns[columnName] = value;
            }
        }
    }
}