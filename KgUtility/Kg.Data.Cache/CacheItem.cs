using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Kg.Data.Cache
{
    /// <summary>
    /// <para>cache数据类的基类</para>
    /// <para>在这各类中定义所有关于缓存信息的属性及方法</para>
    /// </summary>
    public abstract class CacheItem
    {
        public CacheItem()
        {
            Indexes = new CacheIndexCollection(); 
        }
        /// <summary>
        /// 缓存管理Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 缓存创建日期
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 缓存失效日期
        /// </summary>
        public DateTime ExpirDate { get; set; }
        /// <summary>
        /// 缓存数据的状态
        /// </summary>
        public CacheItemState CacheState { get; set; }
       
        /// <summary>
        /// 根据缓存策略更新缓存失效的时间
        /// </summary>
        public abstract void Renew();
        /// <summary>
        /// 将缓存数据指定为失效
        /// </summary>
        public void Abandon()
        {
            CacheState = CacheItemState.Abandoned;
        }
        /// <summary>
        /// 延迟抛弃缓存的任务实例
        /// </summary>
        private Task DelayAbandonTask;
        /// <summary>
        /// 延迟n秒将缓存指定为失效
        /// </summary>
        /// <param name="delay">延迟的秒数</param>
        public void Abandon(int delay)
        {
            if (!DelayAbandonTask.IsCompleted)
            {
                return;
            }
            DelayAbandonTask= new Task(()=> 
            {
                System.Threading.Thread.Sleep(delay * 1000);
                CacheState = CacheItemState.Abandoned;
            });
            DelayAbandonTask.Start();
            
            
        }
        /// <summary>
        /// 缓存的索引， 程序使用这个list来判断是否命中索引
        /// </summary>
        private CacheIndexCollection Indexes { get; set; }

    }
    /// <summary>
    /// 定义缓存数据的状态
    /// </summary>
    public enum CacheItemState
    {
        /// <summary>
        /// 有效
        /// </summary>
        Valid,
        /// <summary>
        /// 被丢弃
        /// </summary>
        Abandoned

    }

    public class CacheIndex
    {
        public CacheIndex(string property, string value)
        {
            this.Property = property;
            this.Value = value;

        }
        public string Property { get; set; }
        public string Value { get; set; }
    }

    public class CacheIndexCollection:List<CacheIndex>
    {
        public List<CacheIndex> this[string property]
        {
            get
            {
                return this.FindAll(ci => ci.Property == property);
            }
        }

        public List<CacheIndex> this[params string[] values]
        {
            get
            {
                List<CacheIndex> results = new List<CacheIndex>();
                foreach (String v in values)
                {
                    results.AddRange( this.FindAll(ci => ci.Value == v));
                }

                return results;
            }
        }


        public CacheIndex this[string property, string value]
        {
            get
            {
                return this.Find(ci => ci.Property == property && ci.Value == value);
            }
        }
    }
}
