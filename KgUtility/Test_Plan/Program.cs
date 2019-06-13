using System;
using Kg.Plan;

namespace Test_Plan
{
    class Program
    {
        delegate void Del(string filename);
        public static void DeleteFile(string filename) {  }
        static void Main(string[] args)
        {
            Del delfilehan = DeleteFile;
            Plan.Create.
                Set(new PlanSettings()).
                IfContinue((ojb) => { return false; }).
                Do((reg)=> { }).
                Sql(null, "", null).
                Start();
        }


    }
}
