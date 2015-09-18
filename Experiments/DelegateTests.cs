using System;
using System.Threading;
using Xunit;

namespace Experiments
{
    public class DelegateTests
    {
        private const String ExpectedText = "SomeText";

        private delegate String SomeDelegate();

        private class TargetClass
        {
            private string TargetMethod()
            {
                return ExpectedText;
            }

            private void InstanceMethod()
            {
                
            }

            public SomeDelegate GetStrongReferenceDelegate()
            {
                return TargetMethod;
            }

            // This method has the disadvantage that you can easily use call can instance method in the anonymous delegate
            // if the coder isn't paying attention
            public SomeDelegate GetWeakReferenceDelegate()
            {
                WeakReference<TargetClass> weakThis = new WeakReference<TargetClass>(this);
                return delegate
                {
                    TargetClass strongThis;

                    if (!weakThis.TryGetTarget(out strongThis))
                        return null;

                    //UPS, this creates a strong reference to the object
                    //InstanceMethod();
                    return strongThis.TargetMethod();
                };
            }

            // The safer way to write
            public SomeDelegate GetWeakReferenceDelegateSafe()
            {
                return GetWeakReferenceDelegateSafeStatic(new WeakReference<TargetClass>(this));
            }

            private static SomeDelegate GetWeakReferenceDelegateSafeStatic(WeakReference<TargetClass> weakThis)
            {
                return delegate
                {
                    TargetClass strongThis;

                    if (!weakThis.TryGetTarget(out strongThis))
                        return null;

                    return strongThis.TargetMethod();
                };
            }
        }

        [Fact]
        public void TestObjectIsNormallyGarbageCollected()
        {
            TargetClass targetClass = new TargetClass();
            WeakReference weakRefTargetClass = new WeakReference(targetClass);

            targetClass = null;
            GCHelper.ForceAndWaitGarbageCollection();

            Assert.False(weakRefTargetClass.IsAlive);
        }

        [Fact]
        public void TestGetStrongReferenceDelegateKeepsObject()
        {
            TargetClass targetClass = new TargetClass();
            WeakReference weakRefTargetClass = new WeakReference(targetClass);

            SomeDelegate d = targetClass.GetStrongReferenceDelegate();
            string actualTextResultFromCall = d();
            targetClass = null;
            GCHelper.ForceAndWaitGarbageCollection();

            Assert.False(weakRefTargetClass.IsAlive);
            //we also make sure the call to the method is done
            Assert.Equal(ExpectedText, actualTextResultFromCall);
        }

        [Fact]
        public void TestGetWeakReferenceDelegateAllowsObjectToBeGarabageCollected()
        {
            TargetClass targetClass = new TargetClass();
            WeakReference weakRefTargetClass = new WeakReference(targetClass);

            SomeDelegate d = targetClass.GetWeakReferenceDelegate();
            string actualTextResultFromCall = d();
            targetClass = null;
            GCHelper.ForceAndWaitGarbageCollection();
            
            Assert.False(weakRefTargetClass.IsAlive);
            //we also make sure the call to the method is done
            Assert.Equal(ExpectedText, actualTextResultFromCall);
        }

        [Fact]
        public void TestGetWeakReferenceDelegateSafeAllowsObjectToBeGarabageCollected()
        {
            TargetClass targetClass = new TargetClass();
            WeakReference weakRefTargetClass = new WeakReference(targetClass);

            SomeDelegate d = targetClass.GetWeakReferenceDelegateSafe();
            string actualTextResultFromCall = d();
            targetClass = null;
            GCHelper.ForceAndWaitGarbageCollection();

            Assert.False(weakRefTargetClass.IsAlive);
            //we also make sure the call to the method is done
            Assert.Equal(ExpectedText, actualTextResultFromCall);
        }

    }
}
