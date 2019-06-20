using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Data.Cache
{
    /// <summary>
    /// 配置信息： 缓存失效（固定日期失效）
    /// </summary>
    public class PriodCacheStrategy: ICacheStrategy
    {
        /// <summary>
        ///指定数据失效的时间长度millonsecond 
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

    public interface ICacheStrategy
    {
        /// <summary>
        /// 根据缓存策略，更新cache条目
        /// </summary>
        /// <param name="item"></param>
        void Renew(CacheItem item);
    }
}
