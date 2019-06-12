using System;

namespace Kg.Plan
{
    public class Plan
    {
        public static Plan Create
        {
            get { return new Plan(); }
        }

        public void Start()
        { }

        
    }

    public static class PlanExten
    {
        public static Plan If(this Plan source,string expression)
        {
            return source;
        }

        public static Plan When(this Plan source, string expression)
        {
            return source;
        }


        public static Plan After(this Plan source, string expression)
        {
            return source;
        }
        public static Plan Set(this Plan source, string expression)
        {
            return source;
        }
        public static Plan Do(this Plan source, string expression)
        {
            return source;
        }
    }
}
