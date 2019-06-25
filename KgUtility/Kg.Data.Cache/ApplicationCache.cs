using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kg.Data.Cache
{
    public class ApplicationCache : IDisposable
    {
        /// <summary>
        /// 是应用程序的名称，该名称用于缓存的数据层级。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表示应用程序的版本，不同版本的应用程序可能造成缓存数据结构不一致。
        /// 如果可以保证各个版本之间数据结构兼容的情况下，可以忽略此属性。
        /// </summary>
        public int Version { get; set; }
       
        /// <summary>
        /// 应用程序级的缓存清理策略
        /// </summary>
        public ICacheExpiryStrategy[] GCStrategies { get; set; }
        /// <summary>
        /// 应用程序级数据缓存策略（允许多个策略并存）
        /// </summary>
        public DataCacheStrategy[] DataStrategies { get; set; }

        public void Dispose()
        {

        }

        public delegate void CacheAppInitializeHandle();
        public CacheAppInitializeHandle InitializeHandle { get; set; }
        public void Initialize()
        {
            if (InitializeHandle != null)
            { InitializeHandle(); }
            CheckMainDb();
        }
        /// <summary>
        /// 检查缓存的sqlite db是否存在。如果不存在的话，创建。
        /// </summary>
        private void CheckMainDb()
        {
            Db.Ini(()=> {
                Db.SysDb.CreateTable<ApplicationCache>();
                Db.SysDb.CreateTable<CacheLog>();
                Db.SysDb.CreateTable<CacheItem>();
            });
            
        }
    }
}
