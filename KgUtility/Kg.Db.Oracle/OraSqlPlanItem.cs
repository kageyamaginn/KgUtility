using Kg.Plan;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kg.Db.Oracle
{
    public class OraSqlPlanItem : PlanItem
    {

        public string ConnectionString { get; set; }
        public string CommandString { get; set; }
       
        public OraSqlPlanItem(String connectionString,string cmdString,params OracleParameter[] parameters)
        {
            throw new Exception("this method is not completed");
        }
        public override PlanItemResultCollection Run()
        {
            throw new Exception("this method is not completed");
        }

        public override PlanItemNotValidException Valid()
        {
            return null;
        }
    }

    public static class SqlPlanItemExt
    {
        public static Kg.Plan.Plan OracleExecute(this Kg.Plan.Plan source, String connectionString, string commandText, bool bindByName = true,params OracleParameter[] parameters)
        {

            
            return source;
        }

        public static Kg.Plan.Plan OracleQuery(this Kg.Plan.Plan source, String connectionString, string commandText, dynamic parameters, bool bindByName = true)
        {
            
            return source;
        }
    }
}
