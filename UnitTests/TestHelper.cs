using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowArgs;
using PowArgs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestHelper
    {
        private class ArgsConfig
        {
            [Argument("Int argument", required: true)]
            public int IntArg { get; set; } = 10;

            [Argument("Float argument")]
            public float FloatArg { get; set; } = 11f;

            [Argument("Decimal argument")]
            public decimal DecimalArg { get; set; } = 12M;

            [Argument("String argument")]
            public string StringArg { get; set; } = "string";

            [Argument("Boolean argument")]
            public bool BoolArg { get; set; } = false;
        }

        [TestMethod]
        public void TestHelperGetHelp()
        {
            string[] help = Helper<ArgsConfig>.GetHelpText();

            Assert.IsTrue(help.Length == 5);
            Assert.AreEqual(help[0], "-IntArg <int> (10)                      Int argument");
            Assert.AreEqual(help[1], "[-FloatArg <float> (11)]                Float argument");
            Assert.AreEqual(help[2], "[-DecimalArg <decimal> (12)]            Decimal argument");
            Assert.AreEqual(help[3], "[-StringArg <string> (string)]          String argument");
            Assert.AreEqual(help[4], "[-BoolArg <bool> (False)]               Boolean argument");
        }
    }
}
