using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace NCapsulateExtensions.TsLint
{
    public sealed class TsLintBuildEngine : IBuildEngine
    {
        private readonly IBuildEngine buildEngine;

        public TsLintBuildEngine(IBuildEngine buildEngine)
        {
            this.buildEngine = buildEngine;
        }

        public bool HasError { get; private set; }

        public bool BuildProjectFile(
            string projectFileName,
            string[] targetNames,
            IDictionary globalProperties,
            IDictionary targetOutputs)
        {
            return buildEngine.BuildProjectFile(projectFileName, targetNames, globalProperties, targetOutputs);
        }

        public int ColumnNumberOfTaskNode
        {
            get { return buildEngine.ColumnNumberOfTaskNode; }
        }

        public bool ContinueOnError
        {
            get { return buildEngine.ContinueOnError; }
        }

        public int LineNumberOfTaskNode
        {
            get { return buildEngine.LineNumberOfTaskNode; }
        }

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            buildEngine.LogCustomEvent(e);
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            buildEngine.LogErrorEvent(e);
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            HasError = !TsLintLogAnalyzer.LogMessage(buildEngine, e);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            buildEngine.LogWarningEvent(e);
        }

        public string ProjectFileOfTaskNode
        {
            get { return buildEngine.ProjectFileOfTaskNode; }
        }
    }
}
