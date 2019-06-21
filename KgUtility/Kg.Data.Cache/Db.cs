using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Linq;
using System.IO;

namespace Kg.Data.Cache
{
    public class Db
    {
        static Db()
        {
            _dbs = new List<SQLiteConnection>();
            _dbs.Add(CreateDb(GetSysdbpath()));
        }
        
        static String GetSysdbpath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sys.sqlite");
        }
        public static SQLiteConnection CreateDb(String path)
        {
            if (File.Exists(path))
            {
                return new SQLiteConnection(path);
            }
            return new SQLiteConnection(path, SQLiteOpenFlags.Create);
        }

        public static bool DeleteDb(String path)
        {
            SQLiteConnection conn = new SQLiteConnection(path);
            throw new Exception();
        }

        static List<SQLiteConnection> _dbs;
        public static SQLiteConnection SysDb
        {
            get
            {
                return _dbs.First();
            }
        }

    }
}
