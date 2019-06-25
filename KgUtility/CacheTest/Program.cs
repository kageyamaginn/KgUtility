using System;
using System.IO;
using Kg.Data.Cache;
using SQLite;

namespace CacheTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
            CacheFile imgCacheStrategy = new CacheFile() {
               CacheFolder="D:\\cacheFile"
            };
            PriodCacheStrategy gcStrategy = new PriodCacheStrategy() {
                Proid = 60,
                Renewable = true, RenewProid=30
            };
            ApplicationCache imgCache = new ApplicationCache()
            {
                 Name="ImageCache",
                  DataStrategies=new DataCacheStrategy[] { imgCacheStrategy},
                  GCStrategies=new ICacheExpiryStrategy[] { gcStrategy}
            };
            AppCacheManager.Initialize();
            AppCacheManager.Register(imgCache);
        }
    }
}
