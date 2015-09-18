using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Experiments
{
    class GCHelper
    {
        public static void ForceAndWaitGarbageCollection()
        {
            GC.Collect();
            // we wait for the garbage collection to be executed, since it's not on our
            // thread and it tries to stop us as littlle as possible
            Thread.Sleep(100);
        }
    }
}
