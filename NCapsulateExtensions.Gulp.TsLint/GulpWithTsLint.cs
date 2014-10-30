using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using NCapsulateExtensions.TsLint;
using GulpTask = Ncapsulate.Gulp.Tasks.Gulp;

namespace NCapsulateExtensions.Gulp.TsLint
{
    public class GulpWithTsLint : GulpTask
    {
        public override bool Execute()
        {
            IBuildEngine buildEngine = BuildEngine;
            TsLintBuildEngine tsLintBuildEngine = new TsLintBuildEngine(buildEngine);
            BuildEngine = tsLintBuildEngine;
            bool result = base.Execute();
            BuildEngine = buildEngine;
            return result && !tsLintBuildEngine.HasError;
        }
    }
}
