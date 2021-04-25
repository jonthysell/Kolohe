// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kolohe.Test
{
    [TestClass]
    public class MathExtTests
    {
        [TestMethod]
        public void MathExt_RemapValueTest()
        {
            TestUtils.LoadAndExecuteTestCases<RemapValueTestCase>();
        }
    }

    class RemapValueTestCase : ITestCase
    {
        public double Value;
        public double InMin;
        public double InMax;
        public double OutMin;
        public double OutMax;
        public bool Clamp;
        public double ExpectedResult;

        public double ActualResult;

        public void Parse(string s)
        {
            var split = s.Split(';');
            Value = double.Parse(split[0]);
            InMin = double.Parse(split[1]);
            InMax = double.Parse(split[2]);
            OutMin = double.Parse(split[3]);
            OutMax = double.Parse(split[4]);
            Clamp = bool.Parse(split[5]);
            ExpectedResult = double.Parse(split[6]);
        }

        public void Execute()
        {
            ActualResult = MathExt.RemapValue(Value, InMin, InMax, OutMin, OutMax, Clamp);
            Assert.AreEqual(ExpectedResult, ActualResult);
        }
    }
}
