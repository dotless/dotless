using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.Parser.Functions;
using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;
using dotless.Core.Plugins;
using NUnit.Framework;

namespace dotless.Test.Unit
{
    [TestFixture]
    public class EnvFixture
    {
        [Test]
        public void RegisteringSingleFunctionInDifferentEnvironmentsWorks() {
            var env = new Env();
            env.AddPlugin(new TestPlugin());

            Assert.DoesNotThrow(() => {
                var newEnv = new Env();
                newEnv.AddPlugin(new TestPlugin());
            });
        }
    }

    public class TestPlugin : IFunctionPlugin {
        public Dictionary<string, Type> GetFunctions() {
            return new Dictionary<string, Type>() {
                {"test", typeof (TestFunction)}
            };
        }
    }

    public class TestFunction : Function {
        protected override Node Evaluate(Env env) {
            return new Quoted("test", true);
        }
    }
}
