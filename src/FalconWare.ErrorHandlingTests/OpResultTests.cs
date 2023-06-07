﻿using FalconWare.ErrorHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}