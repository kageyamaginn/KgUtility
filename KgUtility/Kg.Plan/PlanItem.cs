using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Plan
{
    /// <summary>
    /// 是一个抽象类型，其中定义了Plan中执行的子项的必要元素
    /// </summary>
    public abstract class PlanItem : IExecutable
    {
        /// <summary>
        /// 执行Plan项目
        /// </summary>
        /// <returns>返回Plan的所有结果的集合</returns>
        public abstract PlanItemResultCollection Run();
        /// <summary>
        /// 验证Plan Item collection是否合法
        /// </summary>
        /// <returns></returns>
        public abstract PlanItemNotValidException Valid();
        /// <summary>
        /// Planitem所属的Plan
        /// </summary>
        public Plan Parent { get; set; }
        public delegate void RegisterResultHandle(string resultName, object data);
        /// <summary>
        /// 将Planitem运行结果添加到Plan的result集合中
        /// </summary>
        /// <param name="resultName">结果的名称</param>
        /// <param name="data">结果的值</param>
        public void RegisterResult(string resultName, object data)
        {
            this.Parent.Results.Add(new PlanItemResult(this, Parent.Items.IndexOf(this)) {  Result=data, ResultName=resultName});
        }

    }
    /// <summary>
    /// <para>Planitem执行一个返回值为Boolean的方法，返回值表示是否将Plan继续执行下去。</para>
    /// <para>这个返回值存储在item中的最后一个结果中，如果为true则继续执行接下来的PlanItem，如果是false，中断Plan，并抛出一个中断的异常</para>
    /// <para>**需要在判断方法中使用注册</para>
    /// </summary>
    public class IfContinuePlanItem : PlanItem
    {
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public delegate bool IfContinuePlanItemHandle(RegisterResultHandle reg,PlanItemResultCollection results);
        public IfContinuePlanItemHandle Action { get; set; }
        public IfContinuePlanItem(IfContinuePlanItemHandle ifAction,Plan parent)
        {
            this.Action = ifAction;
            this.Parent = parent;
        }
        public override PlanItemResultCollection Run()
        {
            RegisterResult("if-result", this.Action(this.RegisterResult,this.Parent.Results));
            return this.Parent.Results;
        }

        public override PlanItemNotValidException Valid()
        {
            return null;
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
            return null;
        }
    }
    

    public class DoPlanItem : PlanItem
    {
        public DoPlanItem(DoPlanItemHandle action,Plan parent, object[] parameters)
        {
            this.action = action;
            this.Parent = parent;
            this.Parameters = parameters;
        }
        public object[] Parameters { get; set; }

        public delegate void DoPlanItemHandle(RegisterResultHandle reg, PlanItemResultCollection results, object[] parameters);
        DoPlanItemHandle action { get; set; }

        public override PlanItemResultCollection Run()
        {
            action(RegisterResult,this.Parent.Results, Parameters);
            return this.Parent.Results;
        }

        public override PlanItemNotValidException Valid()
        {
            return null;
        }
    }

    public class SqlPlanItem : DoPlanItem
    {
        public SqlPlanItem(DoPlanItemHandle action, Plan parent,object[] parameters) : base(action,parent, parameters)
        {

        }
        new public PlanItemResultCollection Run()
        {
            return this.Parent.Results;
        }
    }
}
