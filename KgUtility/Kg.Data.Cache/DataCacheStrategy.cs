using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Data.Cache
{
    /// <summary>
    /// <para>指定缓存数据的方式</para>
    /// <para>CacheFileToDb</para>
    /// <para>CacheFile</para>
    /// <para>CacheDataToFile</para>
    /// <para>CacheData</para>
    /// </summary>
    public class DataCacheStrategy { }
    public class CacheFileToDb:DataCacheStrategy
    {

    }
    public class SaveToFile : DataCacheStrategy { }
    public class CacheFile : DataCacheStrategy
    {
        /// <summary>
        /// 是否将download下来的文件移动到cache文件夹，默认为false
        /// </summary>
        public bool DeleteOriginalFile { get; set; }
        /// <summary>
        /// Cache文件的命名方式，详细信息请参见：cacheFilePattern文档。
        /// </summary>
        public String CacheFilePattern { get; set; }
        /// <summary>
        /// 指定cache文件夹
        /// </summary>
        public string CacheFolder { get; set; }
    }

    public class CacheDataToFile:CacheFile
    {
        
    }

    

    public class CacheData : DataCacheStrategy
    {

    }

    
}
