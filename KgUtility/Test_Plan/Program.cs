using System;
using System.Threading;
using System.Collections.Generic;
using Kg.Plan;
using static Kg.Plan.PlanItem;
using Newtonsoft.Json;
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

        public static void RunPlan()
        {
            var plan = SchedulePlan.Create(3000);

            plan.
                Set(new PlanSettings { BreakWhenException = false }).
                Do(CheckFile).
                IfContinue((reg, results) => { reg("if-1", "continue---"); return true; }).
                Do(CheckNetwork);

            plan.BeginSchedule();
            while (true) { }
        }

        static void Main(string[] args)
        {
            List<TestObject> list = new List<TestObject>();
            list.Add(new TestObject() { name = "test1" });
            list.Add(new TestObject() { name = "test1" });
            list.Add(new TestObject() { name = "test1" });
            list.Add(new TestObject() { name = "test1" });
            list.Add(new TestObject() { name = "test1" });
            var list1 = list[0];
            list.RemoveAt(0);
            
        }



    }
    public class TestObject
    {
        public string name { get; set; }
    }
}
