using Intell.Data;
using Intell.Data.Sql;

namespace IntellU;

[TestClass]
public class SqlCommandCreatorT {

    [TestMethod]
    public void IsKeyword() {
        if (SqlCommandCreator.IsKeyword("key") != true) throw new Exception("");
        if (SqlCommandCreator.IsKeyword("email") != false) throw new Exception("");
    }

    [TestMethod]
    public void GetSqlLiteral() {
        Assert.AreEqual("NULL", SqlCommandCreator.GetSqlLiteral(null));
        Assert.AreEqual("3", SqlCommandCreator.GetSqlLiteral(3L));
        Assert.AreEqual("3", SqlCommandCreator.GetSqlLiteral(3));
        Assert.AreEqual("3", SqlCommandCreator.GetSqlLiteral((short)3));
        Assert.AreEqual("3", SqlCommandCreator.GetSqlLiteral((byte)3));
        Assert.AreEqual("0", SqlCommandCreator.GetSqlLiteral(false));
        Assert.AreEqual("1", SqlCommandCreator.GetSqlLiteral(true));
        Assert.AreEqual("'2000-05-05T00:00:00'", SqlCommandCreator.GetSqlLiteral(new DateTime(2000, 5, 5)));
        Assert.AreEqual("N'1234abc'", SqlCommandCreator.GetSqlLiteral("1234abc"));
        Assert.AreEqual("'d2d825b9-aff6-4a59-b567-e82abd9f367f'", SqlCommandCreator.GetSqlLiteral(Guid.Parse("D2D825B9-AFF6-4A59-B567-E82ABD9F367F")));
        Assert.AreEqual("NULL", SqlCommandCreator.GetSqlLiteral(DBNull.Value));
        Assert.AreEqual("1", SqlCommandCreator.GetSqlLiteral(ConditionMode.Exclude));
    }

    [TestMethod]
    public void EscapeIdentifier() {
        if (SqlCommandCreator.EscapeIdentifier("key") != "[key]") throw new Exception("");
        if (SqlCommandCreator.EscapeIdentifier("[distinct]") != "[distinct]") throw new Exception("");
        if (SqlCommandCreator.EscapeIdentifier("email") != "email") throw new Exception("");
    }

    [TestMethod]
    public void GenerateAssignment() {
        string command;
        var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase) {
            ["id"] = 1,
            ["Close"] = 2,
            ["key"] = 3,
            ["email"] = null,
        };
        
        command = SqlCommandCreator.GenerateAssignment(dict, ["id", "close"], " AND ", SqlNullHandling.Allow);
        Assert.AreEqual("id=1 AND [close]=2", command, true);

        Assert.Throws<Exception>(() => {
            SqlCommandCreator.GenerateAssignment(dict, ["id", "email"], " AND ", SqlNullHandling.Disallow);
        });
        command = SqlCommandCreator.GenerateAssignment(dict, ["id", "email"], " AND ", SqlNullHandling.Allow);
        Assert.AreEqual("id=1 AND email=NULL", command, true);
        
        command = SqlCommandCreator.GenerateAssignment(dict, ["id", "email"], " AND ", SqlNullHandling.Skip);
        Assert.AreEqual("id=1", command, true);
    }

    [TestMethod]
    public void GenerateInsertStatement() {
        var dict = new Dictionary<string, object?> {
            ["close"] = 2,
            ["key"] = 3,
            ["email"] = "hello",
            ["news"] = null,
        };
        Assert.Throws<Exception>(() => {
            SqlCommandCreator.GenerateInsertStatement("[ACCOUNT]", dict, ["close", "key", "email", "news"], SqlNullHandling.Disallow);
        });
        var command2 = SqlCommandCreator.GenerateInsertStatement("[ACCOUNT]", dict, ["close", "key", "email", "news"], SqlNullHandling.Skip);
        Assert.AreEqual("INSERT INTO [ACCOUNT]([close],[key],email) VALUES (2,3,N'hello');", command2, true);

        var command3 = SqlCommandCreator.GenerateInsertStatement("[ACCOUNT]", dict, ["close", "key", "email", "news"], SqlNullHandling.Allow);
        Assert.AreEqual("INSERT INTO [ACCOUNT]([close],[key],email,news) VALUES (2,3,N'hello',NULL);", command3, true);

        var command4 = SqlCommandCreator.GenerateInsertStatement("[ACCOUNT]", dict, ["close", "key", "email", "news", "x", "y"], SqlNullHandling.Skip);
        Assert.AreEqual("INSERT INTO [ACCOUNT]([close],[key],email) VALUES (2,3,N'hello');", command4, true);
    }

    [TestMethod]
    public void GenerateInsertStatementMultiple() {
        var dicts = new Dictionary<string, object?>[] {
            new() {
                ["id"] = 1,
                ["name"] = "Georgia",
                ["email"] = "georgia@x.y",
                ["news"] = null,
            },
            new() {
                ["id"] = 2,
                ["name"] = "Louis",
                ["email"] = "louis@x.y",
                ["news"] = true,
            }
        };

        Assert.Throws<Exception>(() => {
            SqlCommandCreator.GenerateInsertStatement("[ACCOUNT]", dicts, ["id", "name", "email", "news"], SqlNullHandling.Disallow);
        });
        Assert.Throws<ArgumentException>(() => {
            SqlCommandCreator.GenerateInsertStatement("[ACCOUNT]", dicts, ["id", "name", "email", "news"], SqlNullHandling.Skip);
        });

        var command2 = SqlCommandCreator.GenerateInsertStatement("[ACCOUNT]", dicts, ["id", "name", "email", "news"], SqlNullHandling.Allow);
        Assert.AreEqual("INSERT INTO [ACCOUNT](id,name,email,news) VALUES(1,N'Georgia',N'georgia@x.y',NULL),(2,N'Louis',N'louis@x.y',1);", command2, true);

    }

    [TestMethod]
    public void GenerateUpdateStatement() {
        var dict = new Dictionary<string, object?> {
            ["id"] = 1,
            ["name"] = "Georgia",
            ["email"] = "georgia@x.y",
            ["news"] = null,
        };
        var command = SqlCommandCreator.GenerateUpdateStatement("[ACCOUNT]", dict,
            set_keys: ["name", "email", "news"],
            where_keys: ["id"], SqlNullHandling.Skip);

        Assert.AreEqual("UPDATE [ACCOUNT] SET name=N'Georgia',email=N'georgia@x.y' WHERE id=1;", command, true);
    }

    [TestMethod]
    public void GenerateDeleteStatement() {
        var dict = new Dictionary<string, object?> {
            ["id"] = 1,
            ["close"] = 2,
            ["key"] = 3,
            ["email"] = "hello",
        };
        var command = SqlCommandCreator.GenerateDeleteStatement("key", dict, ["id"]);

        Assert.AreEqual("delete from [key] where id=1;", command, true);
    }




}