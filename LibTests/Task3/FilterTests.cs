using System;
using System.Collections.Generic;
using Lib.Task3;
using Lib.Task3.FilterImpulseResponses;
using Lib.Task3.WindowFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibTests.Task3
{
    [TestClass()]
    public class FilterTests
    {
        [TestMethod()]
        public void FilterOperationTest()
        {
            var filter = new Filter();
            var result = filter.ImpulseResponse(new LowPassImpulseResponse()).WindowFunction(new RectangularWindow()).FilterOperation(new List<double>() { 15, 18 }, 63, 10000, 80000);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void FilterOperationWithoutWindowTest()
        {
            var filter = new Filter();
            var result = filter.ImpulseResponse(new LowPassImpulseResponse()).FilterOperation(new List<double>() { 15, 18 }, 63, 10000, 80000);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception), "Response is null")]
        public void FilterOperationWithoutImpulseResponseTest()
        {
            var filter = new Filter();
            filter.WindowFunction(new RectangularWindow()).FilterOperation(new List<double>() { 15, 18 }, 63, 10000, 80000);
        }
    }
}