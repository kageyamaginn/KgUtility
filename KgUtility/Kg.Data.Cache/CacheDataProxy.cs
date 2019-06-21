using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Data.Cache
{
    /// <summary>
    /// 将一种数据类型 转换为另一种存储的方式，例如将数据转为文件存储。
    /// </summary>
    public class CacheDataProxy
    {
    }
    /// <summary>
    /// 将数据保存到缓存文件中的配置
    /// </summary>
    public class DataToFileProxy:CacheDataProxy
    {

    }

    /// <summary>
    /// 将文件转为数据保存到数据库的配置
    /// </summary>
    public class FileToDataProxy : CacheDataProxy
    {

    }
}
