using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.Parser.Functions;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;
using dotless.Core.Plugins;
using dotless.Core.Test.Plugins;
using NUnit.Framework;

namespace dotless.Core.Test.Unit
{
    [TestFixture]
    public class EnvFixture
    {
        [Test]
        public void RegisteringSingleFunctionInDifferentEnvironmentsWorks() {
            var env = new Env();
            env.AddPlugin(new TestPlugin1());

            Assert.DoesNotThrow(() => {
                var newEnv = new Env();
                newEnv.AddPlugin(new TestPlugin1());
            });
        }
    }
}
