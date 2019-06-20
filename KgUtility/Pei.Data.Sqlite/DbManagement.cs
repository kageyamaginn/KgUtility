using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace Pei.Data.Sqlite
{
    public class DbManagement
    {
        public string CreateDb(string dbName,String path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
            }
            SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(System.IO.Path.Combine(path, dbName + ".sqlite"), SQLite.SQLiteOpenFlags.Create);
            conn.Close();
            return dbName;
        }
    }
}
