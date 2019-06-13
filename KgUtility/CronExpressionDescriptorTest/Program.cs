using System;
using CronExpressionDescriptor;
using Quartz;
using Quartz.
namespace CronExpressionDescriptorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        static void ExplainCronExpress()
        {

            CronExpressionDescriptor.ExpressionDescriptor des = new ExpressionDescriptor("0,2,22 0,2,23 0,3 5,15 FEB ? *", new Options() { });
            Console.WriteLine(des.GetDescription(DescriptionTypeEnum.FULL));
        }
    }
}
