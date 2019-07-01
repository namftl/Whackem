using System;
using System.IO;
using Game1;
using SQLite;

//[assembly: Dependency(typeof(SQLite_Android))]

namespace Game1
{

    public class SQLite_Android : ISQLite
    {
        public SQLite_Android(){}

        public SQLiteConnection GetConnection()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db3");

            var conn = new SQLite.SQLiteConnection(dbPath);
            return conn;
        }
    }
}
