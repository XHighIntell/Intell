namespace Intell.Data.Sql;

///<summary>Defines how the <see cref="SqlCommandCreator"/> handles null values.</summary>
///<remarks><see cref="DBNull"/> is not null.</remarks>
public enum SqlNullHandling {
    ///<summary>Throws an exception if the value is null.</summary>
    Disallow = 0,

    ///<summary>Skips the value if it is null. This is ideal for generating the SET clause, as excluding NULLS fields reduces unnecessary database modifications.</summary>
    Skip = 1,

    ///<summary>Treats null values as <see cref="DBNull"/>. This is ideal for generating the WHERE clause, as including more filtering conditions increases query precision.</summary>
    Allow = 2,
}
