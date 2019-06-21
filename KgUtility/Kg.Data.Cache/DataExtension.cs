using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Data.Cache
{
    public static class DataExtension
    {
        public static void Load<T>(this T source, string key, string value,ApplicationCache cache) 
        {
            foreach (DataCacheStrategy dcs in cache.DataStrategies)
            {
                /// <para>CacheFileToDb</para>
                /// <para>CacheFile</para>
                /// <para>CacheDataToFile</para>
                /// <para>CacheData</para>
                switch (dcs.GetType().FullName)
                {
                    case "Kg.Data.Cache.CacheFileToDb":
                        break;
                    case "Kg.Data.Cache.CacheFile":
                        
                        break;
                    case "Kg.Data.Cache.CacheDataToFile":
                        break;
                    case "Kg.Data.Cache.CacheData":
                        break;
                }
            }
        }

        public static void Update<T>(this T source) 
        {

        }

        public static void Abandon<T>(this T source)
        {

        }

       
    }
}
