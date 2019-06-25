using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Data.Cache
{
    /// <summary>
    /// 配置信息： 缓存失效（固定日期失效）
    /// </summary>
    public class PriodCacheStrategy: ICacheExpiryStrategy
    {
        /// <summary>
        ///指定数据在缓存后多久失效(second)
        /// </summary>
         public int Proid { get; set; }  
        /// <summary>
        /// 指定缓存被命中后是否可以延期
        /// </summary>
        public bool Renewable { get; set; }
        /// <summary>
        /// 指定每次被命中后延期的时间(秒）
        /// </summary>
        public int RenewProid { get; set; }

        public void Renew(CacheItem item)
        {
            if (Renewable)
            {
                item.ExpirDate = item.ExpirDate.AddMilliseconds(RenewProid * 1000);
            }
            else
            {
                throw new UnrenewableException();
            }
        }

    }
    public class SizeCacheStrategy : ICacheExpiryStrategy
    {
        /// <summary>
        /// 缓存上线，如果超过上限，触发缓存清理程序
        /// </summary>
        public  int CacheUpLimit { get; set; }
        /// <summary>
        /// 指定当缓存超出上线时候可以自动扩展
        /// </summary>
        public bool Extensible { get; set; }
        /// <summary>
        /// 每次扩展的大小（MB）
        /// </summary>
        public int ExtensibleSize { get; set; }

        /// <summary>
        /// not support this method
        /// </summary>
        /// <param name="item"></param>
        public void Renew(CacheItem item)
        {
            throw new NotSupportedException();
        }
    }

    public interface ICacheExpiryStrategy
    {
        /// <summary>
        /// 根据缓存策略，更新cache条目
        /// </summary>
        /// <param name="item"></param>
        void Renew(CacheItem item);
    }
}
