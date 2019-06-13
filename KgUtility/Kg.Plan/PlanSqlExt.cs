using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Kg.Plan
{
    public static class PlanSqlExt
    {
        public static Plan Sql(this Plan source, DbConnection conn, string commandText, object[] sqlParameters)
        {
            
            return source;
        }
    }
}
