using System;
using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NCapsulateExtensions.TsLint.Tests
{
    [TestClass]
    public class TsLintLogAnalyzerTests
    {
        private MockRepository repo;
        private Mock<IBuildEngine> mockBuildEngine;

        [TestInitialize]
        public void SetUp()
        {
            repo = new MockRepository(MockBehavior.Strict);
            mockBuildEngine = repo.Create<IBuildEngine>();
        }

        [TestCleanup]
        public void TearDown()
        {
            repo.VerifyAll();
        }

        [TestMethod]
        public void TestLogMessageLogsABuildErrorWhenATsLintWarningOccurs()
        {
            mockBuildEngine.Setup(engine => engine.LogErrorEvent(It.IsAny<BuildErrorEventArgs>()));
            mockBuildEngine.Setup(engine => engine.LogMessageEvent(It.IsAny<BuildMessageEventArgs>()));
            BuildMessageEventArgs message = new BuildMessageEventArgs(
                @"(quotemark) Private\Welcome\Welcome.ts[4, 20]: ' should be """,
                string.Empty,
                string.Empty,
                MessageImportance.High);
            TsLintLogAnalyzer.LogMessage(mockBuildEngine.Object, message);
        }
    }
}
