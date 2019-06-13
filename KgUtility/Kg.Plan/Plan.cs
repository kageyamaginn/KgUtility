using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Kg.Plan
{
    public class Plan
    {
        public PlanSettings Settings { get; set; }

        public static Plan Create
        {
            get { return new Plan(); }
        }

        public void Start()
        {
            for (int itemindex = Items.FindLastIndex(i=> {return i is SetPlanItem; })+1/*start from first executable item*/ ; itemindex < Items.Count; itemindex++)
            {
                try
                {
                    PlanItem curr = Items[itemindex];
                    if (curr is IfContinuePlanItem)//如果当前类型是ifcontinue，执行判断方法，如果返回为false，中断执行队列，并插入一条exception。
                    {
                        var resultColl= (curr as IfContinuePlanItem).Run();
                        if (!(bool)resultColl[curr, 0].Result)
                        {
                            this.Exceptions.Add(new Exception("programmed break; reason:" + resultColl[curr, 1].Result.ToString()));//ifcontinue返回的结果第二条为终止原因
                        break;
                        }
                        
                    }
                    Items[itemindex].Run();
                }
                catch (Exception ex)
                {
                    this.Exceptions.Add(ex);
                    
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

        public delegate void RegisterResult(string resultName, object data);

        /// <summary>
        /// to validate if the plan items is legal
        /// </summary>
        public void ValidatePlanItems()
        {
            bool settingsFinish = false;
            for (int itemIndex = 0; itemIndex < this.Items.Count; itemIndex++)
            {
                PlanItem curr = Items[itemIndex];

                if (itemIndex == 0 && !(curr is SetPlanItem)) { new PlanItemNotValidException("the first item must be typeof SetPlanItem. for setting plan."); }
                if (settingsFinish == false && !(curr is SetPlanItem)) { settingsFinish = true; }
                if (settingsFinish && curr is SetPlanItem) { new PlanItemNotValidException("settings must set before any executable items"); }

                var exception = curr.Valid();
                if (exception != null)
                {
                    throw exception;
                }
            }
        }

        
        
    }
    public class PlanItemNotValidException : Exception
    {
        public PlanItemNotValidException(String content) { }
    }


    public abstract class  PlanItem : IExecutable
    {
        public abstract PlanItemResultCollection Run();
        public abstract PlanItemNotValidException Valid();

        public Plan Parent { get; set; }

    }

    public class PlanItemResult
    {
        public PlanItemResult(PlanItem item, int itemIndex)
        {
            this.Item = item;
            this.ItemIndex = ItemIndex;
        }

        public string ResultName { get; set; }
        public int ItemIndex { get;  }
        public PlanItem Item { get; }
        public object Result { get; set;}

    }

    public class BooleanPlanItemResult : PlanItemResult
    {
        public BooleanPlanItemResult(PlanItem item, int itemindex) : base(item, itemindex) { }
        public new bool Result { get; set; }
    }

    public class PlanItemResultCollection : List<PlanItemResult>
    {
        public List<PlanItemResult> this[PlanItem item]
        {
            get { return this.FindAll(i => i.Item == item); }
        }

        public PlanItemResult this[PlanItem item, int index]
        {
            get
            {
                var results = this[item];
                if (results.Count == 0||index<0||index>results.Count-1) return null;
                return results[index];
            }
        }
    }

    public class IfContinuePlanItem : PlanItem
    {
        public delegate bool IfContinuePlanItemHandle(object state);
        public IfContinuePlanItemHandle Action { get; set; }
        public IfContinuePlanItem(IfContinuePlanItemHandle ifAction)
        {
            
        }
        public override PlanItemResultCollection Run()
        {
            return this.Parent.Results;
        }

        public override PlanItemNotValidException Valid()
        {
            throw new NotImplementedException();
        }
    }
    public class AfterPlanItem : PlanItem
    {
        public override PlanItemResultCollection Run()
        {
            return this.Parent.Results;
        }

        public override PlanItemNotValidException Valid()
        {
            throw new NotImplementedException();
        }
    }
    public class SetPlanItem : PlanItem
    {
        public override PlanItemResultCollection Run()
        {
            return this.Parent.Results;
        }

        public override PlanItemNotValidException Valid()
        {
            throw new NotImplementedException();
        }
    }

    public class DoPlanItem : PlanItem
    {
        public DoPlanItem(DoPlanItemHandle action)
        {
            this.action = action;
        }

        public delegate void DoPlanItemHandle(Plan.RegisterResult reg);
        DoPlanItemHandle action { get; set; }

        public override PlanItemResultCollection Run()
        {

            return this.Parent.Results;
        }

        public override PlanItemNotValidException Valid()
        {
            throw new NotImplementedException();
        }
    }

    public class SqlPlanItem : DoPlanItem
    {
        public SqlPlanItem(DoPlanItemHandle action):base(action)
        {

        }
        new public PlanItemResultCollection Run()
        {
            return this.Parent.Results;
        }
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
