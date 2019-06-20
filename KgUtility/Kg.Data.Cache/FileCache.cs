using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Data.Cache
{
    public class FileCache : CacheItem
    {
        public string FilePath { get; set; }

        public override void Renew()
        {
            throw new NotImplementedException();
        }
    }
}
