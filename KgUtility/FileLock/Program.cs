using System;
using System.IO;
using System.Threading;
using SQLite;

namespace FileLock
{
    class Program
    {
        static void Main(string[] args)
        {
            String filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "text.db");
            //FileStream fs= File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.Read);
            SQLiteConnection conn = new SQLiteConnection(filePath);
            
            conn.CreateTable(typeof(Entity), CreateFlags.AutoIncPK);
            conn.Close();
            System.Diagnostics.Process.Start(filePath);
            while (true) { }
        }
    }

    public class Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
