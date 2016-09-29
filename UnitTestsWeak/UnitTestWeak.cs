using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeakDelegate;

namespace UnitTestsWeak
{
    [TestClass]
    public class UnitTestWeak
    {
        private event Action<int,int> testEvent;
        [TestMethod]
        public void TestMethod_ActionWithTwoParams()
        {
            var testMethods = new ClassWithMethodsForTest();
            var weakRef=new WeakDelegateClass((Action<int,int>)(testMethods.Sum));
            testEvent += (Action<int,int>)weakRef;
            testEvent.Invoke(3, 7);
            Assert.AreEqual(10,testMethods.resultIntValue);
        }

        private event Action<int, int, byte> testEventThreeParams;
        [TestMethod]
        public void TestMethod_ActionWithThreeDifferentParams()
        {
            var testMethods = new ClassWithMethodsForTest();
            var weakRef = new WeakDelegateClass((Action<int, int,byte>)(testMethods.Mul));
            testEventThreeParams += (Action<int, int, byte>)weakRef;
            testEventThreeParams.Invoke(5, 7, 10);
            Assert.AreEqual(350, testMethods.resultIntValue);
        }
    }
}
