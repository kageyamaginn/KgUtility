using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Linq;
using System.IO;

namespace Kg.Data.Cache
{
    /// <summary>
    /// 使用之前，请务必调用Ini方法。
    /// </summary>
    public class Db
    {
        public delegate void CreateTableHandle();
        public static void Ini(CreateTableHandle createTable=null)
        {
            _dbs = new List<SQLiteConnection>();
            _dbs.Add(CreateDb(GetSysdbpath()));
            if (createTable != null)
            {
                createTable();
            }
        }
        
        
        static String GetSysdbpath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"cachePivot", "sys.sqlite");
        }

        /// <summary>
        /// 创建db
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static SQLiteConnection CreateDb(String path,CreateTableHandle createTable=null)
        {
            if (File.Exists(path))
            {
                return new SQLiteConnection(path);
            }
            if (!Directory.Exists(new FileInfo(path).Directory.FullName))
            {
                Directory.CreateDirectory(new FileInfo(path).Directory.FullName);
            }
            var db= new SQLiteConnection(path);
            if (createTable != null)
            {
                createTable();
            }
            return db;
        }
        /// <summary>
        /// 关闭并删除db
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteDb(String path)
        {
            try
            {
                var deletingDb = _dbs.Find(db => db.DatabasePath == path);
                if (deletingDb != null)
                {
                    deletingDb.Close();
                    _dbs.Remove(deletingDb);
                }
                File.Delete(path);
                return true;
            }
            catch { return false; }
            
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
