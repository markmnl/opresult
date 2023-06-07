using FalconWare.ErrorHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FalconWare.ErrorHandling.Tests
{
    class Phobia
    {
        public string Name { get; private set; } = "Hippopotomonstrosesquippedaliophobia";
    }

    [TestClass()]
    public class OpResultTests
    {
        [TestMethod()]
        public void FailureTest_WasSuccessValue()
        {
            var result = OpResultFactory.CreateFailure<string>("foo");

            Assert.IsFalse(result.WasSuccess);
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.NonSuccessMessage));
        }

        [TestMethod()]
        public void FailureTest_Exception()
        {
            var ex = new ApplicationException("some error");
            var result = OpResultFactory.CreateFailure<string>(ex);

            Assert.IsFalse(result.WasSuccess);
            Assert.IsNotNull(result.Exception);
            Assert.IsFalse(String.IsNullOrWhiteSpace(result.NonSuccessMessage));
        }

        [TestMethod()]
        public void SuccessTest_WasSuccessValue()
        {
            var returnValue = new Phobia();
            var result = OpResultFactory.CreateSuccess(returnValue);

            Assert.IsTrue(result.WasSuccess);
        }

#if DEBUG
        [TestMethod()]
        public void OpResultAccessExceptionTest_IsThrownWhenWasSuccessNotChecked()
        {
            var returnValue = new Phobia();
            var result = OpResultFactory.CreateSuccess(returnValue);

            Assert.ThrowsException<OpResultAccessException>(() => result.Value);
        }

        [TestMethod()]
        public void OpResultAccessExceptionTest_IsNotThrownWhenWasSuccessNotChecked()
        {
            var returnValue = new Phobia();
            var result = OpResultFactory.CreateSuccess(returnValue);

            Assert.IsTrue(result.WasSuccess);
            Assert.IsNotNull(result.Value);
        }
#endif
    }
}