using System;
using Kg.Plan;

namespace Test_Plan
{
    class Program
    {
        static void Main(string[] args)
        {
            Plan.Create.
                Set("").
                When("result='app'").
                Do("").
                Start();
        }

        static void TestMethod()
        {

        }
    }
}
