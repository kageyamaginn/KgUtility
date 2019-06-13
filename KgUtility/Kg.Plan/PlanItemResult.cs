using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Plan
{
    public class PlanItemResult
    {
        public PlanItemResult(PlanItem item, int itemIndex)
        {
            this.Item = item;
            this.ItemIndex = ItemIndex;
        }

        public string ResultName { get; set; }
        public int ItemIndex { get; }
        public PlanItem Item { get; }
        public object Result { get; set; }

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

        public List<PlanItemResult> this[String resultName]
        {
            get { return this.FindAll(i => i.ResultName == resultName); }
        }
        /// <summary>
        /// 获取指定PlanItem的指定下标的结果
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index">item大于等于0时，正向查找； item小于0时逆向查找</param>
        /// <returns></returns>
        public PlanItemResult this[PlanItem item, int index]
        {
            get
            {
                var results = this[item];
                if (results.Count == 0 || index > results.Count - 1) return null;
                if (index < 0) { return results[results.Count + index]; }
                return results[index];
            }
        }
    }
}
