using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowArgs;
using PowArgs.Attributes;

namespace UnitTests
{
    [TestClass]
    public class TestParser
    {
        private class ArgsConfig
        {
            [Argument("Int argument")]
            public int IntArg { get; set; } = 10;

            [Argument("Float argument")]
            public float FloatArg { get; set; } = 11f;

            [Argument("Decimal argument")]
            public decimal DecimalArg { get; set; } = 12M;

            [Argument("String argument")]
            public string StringArg { get; set; } = "string'";

            [Argument("Boolean argument")]
            public bool BoolArg { get; set; } = false;
        }

        private class ArgsConfigWithRequired
        {
            [Argument("Int argument", required:true)]
            public int IntArg { get; set; } = 10;

            [Argument("String argument")]
            public string StringArg { get; set; } = "string'";
        }

        [TestMethod]
        public void TestParsingDifferentValues()
        {
            ArgsConfig args = Parser<ArgsConfig>.Parse(new[] { "2", "3", "4", "5", "true" });

            Assert.IsTrue(args.IntArg == 2);
            Assert.IsTrue(args.FloatArg == 3f);
            Assert.IsTrue(args.DecimalArg == 4M);
            Assert.IsTrue(args.StringArg == "5");
            Assert.IsTrue(args.BoolArg == true);
        }

        [TestMethod]
        public void TestNamedArguments()
        {
            ArgsConfig args = Parser<ArgsConfig>.Parse(new[] 
            {
                "-BoolArg", "true",
                "-StringArg", "2",
                "-DecimalArg", "3",
                "-FloatArg", "4",
                "-IntArg", "5"
            });

            Assert.IsTrue(args.IntArg == 5);
            Assert.IsTrue(args.FloatArg == 4f);
            Assert.IsTrue(args.DecimalArg == 3M);
            Assert.IsTrue(args.StringArg == "2");
            Assert.IsTrue(args.BoolArg == true);
        }

        [TestMethod]
        public void TestWrongAssignment()
        {
            bool exceptionThrown = false;

            try
            {
                ArgsConfig args = Parser<ArgsConfig>.Parse(new[] { "string" });
            }
            catch (Exception e)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void TestWrongAssignmentForNamedArgument()
        {
            bool exceptionThrown = false;

            try
            {
                ArgsConfig args = Parser<ArgsConfig>.Parse(new[] { "-IntArg", "string", "-BoolArg", "string" });
            }
            catch (Exception e)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void TestUnknownArgument()
        {
            bool exceptionThrown = false;

            try
            {
                ArgsConfig args = Parser<ArgsConfig>.Parse(new[] { "-SomeArg", "value" });
            }
            catch (Exception e)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void TestRequiredArguments()
        {
            bool exceptionThrown = false;

            try
            {
                ArgsConfigWithRequired args = Parser<ArgsConfigWithRequired>.Parse(new[] { "-StringArg", "string" });
            }
            catch (Exception e)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void TestBooleanAtTheEnd()
        {
            ArgsConfig args = Parser<ArgsConfig>.Parse(new[]
            {
                "-IntArg", "5",
                "-BoolArg"
            });

            Assert.IsTrue(args.IntArg == 5);
            Assert.IsTrue(args.BoolArg == true);
        }
    }
}
