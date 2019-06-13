using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Plan
{
    public static class PlanExten
    {
        public static Plan IfContinue(this Plan source, IfContinuePlanItem.IfContinuePlanItemHandle ifAction)
        {
            source.Items.Add(new IfContinuePlanItem(ifAction,source));
            return source;
        }

        /// <summary>
        /// not avalible for now
        /// </summary>
        /// <param name="source"></param>
        /// <param name="millisecond"></param>
        /// <returns></returns>
        public static Plan After(this Plan source, int millisecond = 3000)
        {
            return source;
        }
        /// <summary>
        /// not avalible for now
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Plan Wait(this Plan source)
        {
            return source;
        }
        /// <summary>
        /// <para>config behavior of plan</para>
        /// <para>设置plan的行为</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static Plan Set(this Plan source, PlanSettings settings)
        {
            source.Settings = settings;
            return source;
        }

        /// <summary>
        /// <para>invoke a method.</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action">the method be invoked in the plan , you can use the register parameter to store result in plan.</param>
        /// <returns></returns>
        public static Plan Do(this Plan source, DoPlanItem.DoPlanItemHandle action, object[] parameters=null)
        {
            source.Items.Add(new DoPlanItem(action,source,parameters));
            return source;
        }
        


    }
}
