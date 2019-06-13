using System;
using System.Threading;

using Kg.Plan;
using static Kg.Plan.PlanItem;

namespace Test_Plan
{
    class Program
    {
        #region functions & apis
        public static void CheckFile(RegisterResultHandle reg,PlanItemResultCollection results, object[] ps)
        {
            Console.WriteLine("check file");

            Thread.Sleep(3000);
        }

        public static void CheckNetwork(RegisterResultHandle reg, PlanItemResultCollection results, object[] ps)
        {
            Console.WriteLine("Check network");
            Thread.Sleep(3000);
            Console.WriteLine(results["if-1"][0].Result);
            Console.WriteLine("network is good");
        }

        #endregion

        static void Main(string[] args)
        {
            var plan = SchedulePlan.Create(3000);
            
            plan.
                Set(new PlanSettings { BreakWhenException = false }).
                Do(CheckFile).
                IfContinue((reg,results) => { reg("if-1", "continue---"); return true; }).
                Do(CheckNetwork).
                Sql(null,null,null);
                
            plan.BeginSchedule();
            while (true) { }
        }


    }
}
