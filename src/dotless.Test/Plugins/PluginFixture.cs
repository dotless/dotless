namespace dotless.Test.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core.Parser.Infrastructure;
    using Core.Parser.Tree;
    using Core.Plugins;
    using NUnit.Framework;
    using System.Globalization;
    using System.Threading;
    using System.ComponentModel;
    using System.Reflection;
    using dotless.Core.Parser.Infrastructure.Nodes;
    using dotless.Core.Parser;
    using dotless.Core.Importers;

    [DisplayName("Plugin A"), System.ComponentModel.Description("Plugs into A")]
    public class TestPlugin1 : IFunctionPlugin
    {
        public Dictionary<string, Type> GetFunctions()
        {
            return new Dictionary<string,Type>();
        }
    }

    [DisplayName("Plugin B"), System.ComponentModel.Description("Plugs into B")]
    public class TestPlugin2 : IFunctionPlugin
    {
        public TestPlugin2()
        {
        }

        public string One { get; set; }
        public int Two { get; set; }
        public decimal Three { get; set; }
        public bool Four { get; set; }
        public double Five { get; set; }

        public TestPlugin2(string one, int two, decimal three, bool four, double five)
        {
            One = one;
            Two = two;
            Three = three;
            Four = four;
            Five = five;
        }

        public Dictionary<string, Type> GetFunctions()
        {
            return new Dictionary<string,Type>();
        }
    }

    public class TestPluginConfiguratorB : GenericPluginConfigurator<TestPlugin2>
    {
    }

    public abstract class PassThroughPlugin : VisitorPlugin
    {
        public PassThroughPlugin()
        {
            Nodes = new Dictionary<string, int>();
        }

        public Dictionary<string, int> Nodes { get; private set; }

        public override Node Execute(Node node, out bool visitDeeper)
        {
            Assert.NotNull(node, "We should not try to visit null nodes");

            string nodeType = node.GetType().ToString();

            if (Nodes.ContainsKey(nodeType))
            {
                Nodes[nodeType]++;
            }
            else
            {
                Nodes[nodeType] = 1;
            }

            visitDeeper = true;
            return node;
        }
    }

    public class PassThroughAfterPlugin : PassThroughPlugin
    {
        public override VisitorPluginType AppliesTo
        {
            get { return VisitorPluginType.AfterEvaluation; }
        }
    }

    public class PassThroughBeforePlugin : PassThroughPlugin
    {
        public override VisitorPluginType AppliesTo
        {
            get { return VisitorPluginType.BeforeEvaluation; }
        }
    }

    public class PluginFixture : SpecFixtureBase
    {
        private static Parser GetParser()
        {
            var imports = new Dictionary<string, string>();

            imports["file.less"] = @"
.foo {
  color: red;
}
";
            return new Parser { Importer = new Importer(new DictionaryReader(imports)) };
        }

        [Test]
        public void TestLoadingAndConfigurating()
        {
            IEnumerable<IPluginConfigurator> plugins = PluginFinder.GetConfigurators(Assembly.GetAssembly(typeof(PluginFixture)));

            Assert.AreEqual(4, plugins.Count());

            IPluginConfigurator plugin1 = plugins.ElementAt(0);
            IPluginConfigurator plugin2 = plugins.ElementAt(1);
            IPluginConfigurator plugin3 = plugins.ElementAt(2);
            IPluginConfigurator plugin4 = plugins.ElementAt(3);

            Assert.IsInstanceOf<GenericPluginConfigurator<TestPlugin1>>(plugin1);
            Assert.IsInstanceOf<GenericPluginConfigurator<PassThroughAfterPlugin>>(plugin2);
            Assert.IsInstanceOf<GenericPluginConfigurator<PassThroughBeforePlugin>>(plugin3);
            Assert.IsInstanceOf<TestPluginConfiguratorB>(plugin4);

            Assert.AreEqual("Plugin A", plugin1.Name);
            Assert.AreEqual("Plugin B", plugin4.Name);
            Assert.AreEqual("Plugs into A", plugin1.Description);
            Assert.AreEqual("Plugs into B", plugin4.Description);
        }

        [Test]
        public void TestGenericConfiguratorParams1()
        {
            IPluginConfigurator plugin1 = new GenericPluginConfigurator<TestPlugin1>();
            Assert.AreEqual(0, plugin1.GetParameters().Count());
            plugin1.SetParameterValues(plugin1.GetParameters());
            Assert.IsInstanceOf<TestPlugin1>(plugin1.CreatePlugin());
        }

        [Test]
        public void TestGenericConfiguratorParams2()
        {
            IPluginConfigurator plugin2 = new GenericPluginConfigurator<TestPlugin2>();
            var parameters = plugin2.GetParameters();
            Assert.AreEqual(5, parameters.Count());
            //string one, int two, decimal three, bool four, double five
            TestParam(parameters.ElementAt(0), "one", "String", false);
            TestParam(parameters.ElementAt(1), "two", "Int32", false);
            TestParam(parameters.ElementAt(2), "three", "Decimal", false);
            TestParam(parameters.ElementAt(3), "four", "Boolean", false);
            TestParam(parameters.ElementAt(4), "five", "Double", false);
            plugin2.SetParameterValues(null);
            Assert.IsInstanceOf<TestPlugin2>(plugin2.CreatePlugin());
        }

        private void TestParam(IPluginParameter param, string name, string typeDescription, bool isMandatory)
        {
            Assert.AreEqual(name, param.Name);
            Assert.AreEqual(typeDescription, param.TypeDescription);
            Assert.AreEqual(isMandatory, param.IsMandatory);
        }

        [Test]
        public void TestGenericConfiguratorParams3()
        {
            IPluginConfigurator plugin2 = new GenericPluginConfigurator<TestPlugin2>();
            var parameters = plugin2.GetParameters();
            //string one, int two, decimal three, bool four, double five
            parameters.ElementAt(0).SetValue("string");
            parameters.ElementAt(1).SetValue("2");
            parameters.ElementAt(2).SetValue("3.45");
            parameters.ElementAt(3).SetValue("true");
            parameters.ElementAt(4).SetValue("4.567");
            plugin2.SetParameterValues(parameters);
            Assert.IsInstanceOf<TestPlugin2>(plugin2.CreatePlugin());

            TestPlugin2 plugin = plugin2.CreatePlugin() as TestPlugin2;
            Assert.AreEqual("string", plugin.One);
            Assert.AreEqual(2, plugin.Two);
            Assert.AreEqual(3.45m, plugin.Three);
            Assert.AreEqual(true, plugin.Four);
            Assert.AreEqual(4.567d, plugin.Five);
        }

        [Test]
        public void CanNavigateThrough()
        {
            var input = @"
.cl(@a) {
  prop: alpha(opacity=56);
  propb: bar(45px);
  propc: progid:Blah.Blah(blah=78);
  propd: @a;
}

@b: 4;

.clb:nth-of-type(@b) {
 .cl(89px);
 prope: red;
 // comment 1
 &:hover {
   /* comment 2*/
   propf: red #fd0,keyword;
   propg: (8 + 4) / 2;
 }
}
@-webkit-keyframes fontbulger {
  0% {
    font-size: 10px;
  }
  30.5% {
    font-size: 15px;
  }
  100% {
    font-size: 12px;
  }
}
#box {
  -webkit-animation: fontbulger 2s infinite;
}
@import ""file.less"";
.notCalled(@d) {
  proph: red;
}
";
            var expected = @"
.clb:nth-of-type(4) {
  prop: alpha(opacity=56);
  propb: bar(45px);
  propc: progid:Blah.Blah(blah=78);
  propd: 89px;
  prope: red;
}
.clb:nth-of-type(4):hover {
  /* comment 2*/
  propf: red #ffdd00, keyword;
  propg: 6;
}
@-webkit-keyframes fontbulger {
  0% {
    font-size: 10px;
  }
  30.5% {
    font-size: 15px;
  }
  100% {
    font-size: 12px;
  }
}
#box {
  -webkit-animation: fontbulger 2s infinite;
}
.foo {
  color: red;
}
";
            AssertLess(input, expected, GetParser());

            // TODO: Investigate these numbers
            Assert.AreEqual(16, PassThroughBeforePlugin.Nodes[typeof(Rule).ToString()]);
            Assert.AreEqual(20, PassThroughAfterPlugin.Nodes[typeof(Rule).ToString()]);

            // Some rules are preference parsed into quoted text rather than expressions and values
            Assert.AreEqual(1, PassThroughBeforePlugin.Nodes[typeof(Color).ToString()]);
            Assert.AreEqual(4, PassThroughAfterPlugin.Nodes[typeof(Color).ToString()]);

            Assert.AreEqual(11, PassThroughBeforePlugin.Nodes[typeof(Number).ToString()]);
            Assert.AreEqual(9, PassThroughAfterPlugin.Nodes[typeof(Number).ToString()]);
        }
    }
}
