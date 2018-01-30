using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Samples.SampleBizLogic
{
    public class SomeFunction
    {
        private MyBizOptions MyBizOptions { get; }

        public SomeFunction(MyBizOptions myBizOptions)
        {
            MyBizOptions = myBizOptions ?? throw new ArgumentNullException(nameof(myBizOptions));
        }

        public (string result,string methodName) SomeMethod()
        {
            return (MyBizOptions.MyBizOptions10, nameof(SomeMethod));
        }
    }
}
