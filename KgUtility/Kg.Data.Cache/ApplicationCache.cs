using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Data.Cache
{
    public class ApplicationCache
    {
        public string Name { get; set; }
        public int Version { get; set; }
        public ICacheStrategy[] Strategies { get; set; }
         
        public List<ModuleCache> Caches { get; set; }
    }
}
