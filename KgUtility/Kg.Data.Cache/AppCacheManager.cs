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
       /// <summary>
       ///初始化ApplicationCache子系统
       /// </summary>
        public static void Initialize()
        {
            CacheEntities = new List<ApplicationCache>();
        }

        static List<ApplicationCache> CacheEntities { get; set; }

        /// <summary>
        ///将AppCache的配置加载到Cache系统当中
        /// </summary>
        /// <param name="app"></param>
        public static void Register(ApplicationCache app)
        {
            CacheEntities.Add(app);
            app.Initialize();
            
        }
        /// <summary>
        /// 将Cache配置从cache系统中移除
        /// </summary>
        /// <param name="cacheitem"></param>
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
