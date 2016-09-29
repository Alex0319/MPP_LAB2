using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeakDelegate;

namespace UnitTestsWeak
{
    [TestClass]
    public class UnitTestWeak
    {
        [TestMethod]
        public void TestMethod_ActionWithTwoParams()
        {
            var testMethods = new ClassWithMethodsForTest();
            Delegate weakRef=new WeakDelegateClass((Action<int,int>)testMethods.Sum);
            weakRef.DynamicInvoke(3, 7);
            Assert.AreEqual(10,testMethods.resultIntValue);
        }
    }
}
