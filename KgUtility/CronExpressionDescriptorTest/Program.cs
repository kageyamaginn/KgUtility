using System;
using CronExpressionDescriptor;
using Oracle.ManagedDataAccess.Client;
using Quartz;

namespace CronExpressionDescriptorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            OracleConnection conn = new OracleConnection("data source = wwwtst;user id=plm;password=testuser1;");
            OracleCommand cmd = new OracleCommand("begin open :cur_job for select name from report_job where rownum<100;end;",conn);
            OracleParameter result= cmd.Parameters.Add("cur_job", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex0o)
            {

            }
        }

        static void ExplainCronExpress()
        {

            CronExpressionDescriptor.ExpressionDescriptor des = new ExpressionDescriptor("0,2,22 0,2,23 0,3 5,15 FEB ? *", new Options() { });
            Console.WriteLine(des.GetDescription(DescriptionTypeEnum.FULL));
        }
    }
}
