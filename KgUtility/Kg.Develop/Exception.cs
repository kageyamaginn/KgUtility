using System;
using System.Collections.Generic;
using System.Text;

namespace Kg.Develop
{
    public class NotCompeleteException:Exception
    {
        public NotCompeleteException():base("this function not support yet")
        {
            
        }
    }
}
