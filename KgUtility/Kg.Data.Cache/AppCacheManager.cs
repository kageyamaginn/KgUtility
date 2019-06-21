using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Kg.Data.Cache
{
    /// <summary>
    /// 是Application Cache的宿主，负责运行，保持applicationcache。
    /// </summary>
    public class AppCacheManager
    {
        public static void Initialize()
        {
            CacheEntities = new List<ApplicationCache>();
        }

        static List<ApplicationCache> CacheEntities { get; set; }

        public static void Register(ApplicationCache cacheItem)
        {
            CacheEntities.Add(cacheItem);
            cacheItem.Initialize();
            
        }

        public static void Unregister(ApplicationCache cacheitem)
        {
            CacheEntities.Remove(cacheitem);
            cacheitem.Dispose();
        }

        public static void UnregisterAll()
        {
            foreach(ApplicationCache cacheitem in CacheEntities)
            {
                cacheitem.Dispose();
            }

        }



    }
}
