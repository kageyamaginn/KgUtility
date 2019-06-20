using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Kg.Data.Cache
{
    public class DataCache<T> : CacheItem
    {
        /// <summary>
        /// 原始的数据
        /// </summary>
        public T Data { get; set; }
        public ICacheStrategy Strategy { get; set; }
        
        /// <summary>
        /// 被json序列化后的数据
        /// </summary>
        public string SerializedContent { get { return JsonConvert.SerializeObject(Data); } }

        private string serializedContent;
        public bool Valid()
        {
            throw new Exception();
        }

        public override void Renew()
        {
            Strategy.Renew(this);
        }
    }
}
