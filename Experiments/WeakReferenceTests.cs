using System;
using System.Threading;
using Xunit;

namespace Experiments
{
    public class WeakReferenceTests
    {
        private class TargetClass
        {
            public void s()
            {
            }
        }

        private static WeakReference WeakReference()
        {
            TargetClass myTestObj = new TargetClass();
            WeakReference w = new WeakReference(myTestObj);
            return w;
        }

        [Fact]
        public void TestBadWeakReferenceUsage1()
        {
            var w = WeakReference();
          
            if (w.IsAlive)
            {
                //suppose we have a collection here
                GCHelper.ForceAndWaitGarbageCollection();

                TargetClass output = (TargetClass)w.Target;
                Assert.NotNull(output);
            }
            else
            {
                throw new Exception("Object was already garbage collect, wait!? what? how?");
            }
        }

        [Fact]
        // Surprize: In this method we don't get a gabage collection
        public void TestBadWeakReferenceUsage2()
        {
            TargetClass myTestObj = new TargetClass();
            WeakReference w = new WeakReference(myTestObj);
            myTestObj = null;

            // This just won't clear the targetClass from memory...
            GCHelper.ForceAndWaitGarbageCollection();
           
            if (w.IsAlive)
            {
                //suppose we have a collection here
                GCHelper.ForceAndWaitGarbageCollection();

                TargetClass output = (TargetClass)w.Target;
                Assert.NotNull(output);
            }
            else
            {
                throw new Exception("Object was already garbage collect, wait!? what? how?");
            }
        }


        [Fact]
        public void TestProperUsageofWeakRef1()
        {
            var w = WeakReference();


            TargetClass output = (TargetClass)w.Target;
            GCHelper.ForceAndWaitGarbageCollection();

            if (output == null)
                throw new Exception("Object was already garbage collect, wait!? what? how?");

            // does not affect us, we already have a strong reference in the 
            // current function, so the object won't be garbage collected
            GCHelper.ForceAndWaitGarbageCollection();
            Assert.NotNull(output);
        }

        [Fact]
        public void TestProperUsageofWeakRef2()
        {
            TargetClass myTestObj = new TargetClass();
            WeakReference w = new WeakReference(myTestObj);
            myTestObj = null;

            // no matter what we do the garbage collection won't happen
            GCHelper.ForceAndWaitGarbageCollection();
            TargetClass output = (TargetClass)w.Target;
            GCHelper.ForceAndWaitGarbageCollection();

            if (output == null)
                throw new Exception("Object was already garbage collect, wait!? what? how?");

            // does not affect us, we already have a strong reference in the 
            // current function, so the object won't be garbage collected
            GCHelper.ForceAndWaitGarbageCollection();
            Assert.NotNull(output);
        }
    }
}
