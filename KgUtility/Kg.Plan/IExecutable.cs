using System;
using System.Collections.Generic;
using static Kg.Plan.Plan;

namespace Kg.Plan
{
    public interface IExecutable
    {
        PlanItemResultCollection Run();
    }
}