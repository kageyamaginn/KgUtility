using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using System.Timers;

namespace Kg.Plan
{
    public class Plan
    {
        public PlanSettings Settings { get; set; }

        public static Plan Create
        {
            get { return new Plan() { }; }
        }

        public Plan()
        {
            Items = new List<PlanItem>();
            Results = new PlanItemResultCollection();
            Exceptions = new List<Exception>();
        }

        public void Start()
        {
            ValidatePlanItems();
            for (int itemindex = 0; itemindex < Items.Count; itemindex++)
            {
                try
                {
                    PlanItem curr = Items[itemindex];
                    if (curr is IfContinuePlanItem)//如果当前类型是ifcontinue，执行判断方法，如果返回为false，中断执行队列，并插入一条exception。
                    {
                        var resultColl= (curr as IfContinuePlanItem).Run();
                        if (!(bool)resultColl[curr, -1].Result)
                        {
                            this.Exceptions.Add(new Exception("programmed break; reason:" + resultColl[curr, 1].Result.ToString()));//ifcontinue返回的结果第1条为终止原因
                        break;
                        }
                        
                    }
                    Items[itemindex].Run();
                }
                catch (Exception ex)
                {
                    this.Exceptions.Add(ex);
                    if (Settings.BreakWhenException)
                    {
                        break;
                    }
                    
                }
                
            }
        }

        public Task StartAsync()
        {
            throw new Exception();
        }

        public List<PlanItem> Items { get; set; }
        public List<Exception> Exceptions { get; set; }
        public PlanItemResultCollection Results { get; set; }

        

        /// <summary>
        /// to validate if the plan items is legal
        /// </summary>
        protected void ValidatePlanItems()
        {
            for (int itemIndex = 0; itemIndex < this.Items.Count; itemIndex++)
            {
                PlanItem curr = Items[itemIndex];
                var exception = curr.Valid();
                if (exception != null)
                {
                    throw exception;
                }
            }
        }

    }

    public class SchedulePlan : Plan
    {
        Timer sch = null;
        bool IsAsync = false;
        public SchedulePlan(int millisecond,bool IsAsync=false)
        {
            this.IsAsync = IsAsync;
            sch = new Timer();
            sch.Interval = millisecond;
            sch.Elapsed += Sch_Elapsed;
        }
        public String Status { get; set; }
        public void BeginSchedule()
        {
            Status = "Starting";
            this.Start();
            sch.Start();
            Status = "Running";
        }

        public void EndSchedule()
        {
            Status = "Stoping";
            sch.Stop();
            Status = "Stoped";
        }

        private void Sch_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsAsync)
            {
                this.StartAsync();
            }
            else
            {
                this.Start();
            }
        }

        new public static SchedulePlan Create(int millisecond, bool IsAsync = false)
        {
            return new SchedulePlan(millisecond, IsAsync);
        }
    }

    public class EventPlan : Plan
    {
    }

   
 
    public class PlanSettings
    {
        public bool IsDynamic { get; set; }
        /// <summary>
        /// 如果某个Item出现exception就中断整个Plan
        /// </summary>
        public bool BreakWhenException { get; set; }
    }


}
