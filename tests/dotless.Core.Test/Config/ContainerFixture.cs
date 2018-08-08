using dotless.Core.configuration;
using dotless.Core.Importers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotless.Core.Test.Config
{
    public class ContainerFixture
    {
        /// <summary>
        /// To ensure https://github.com/dotless/dotless/issues/555 is fixed
        /// </summary>
        [Test]
        public void Components_Are_Correct_Registerd_To_Suppress_Import_Error()
        {
            var factory = new ContainerFactory();
            var services = factory.GetServices(DotlessConfiguration.GetDefault());

            var lessEngineDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ILessEngine));
            var importerDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IImporter));
            var parserDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(Parser.Parser));

            Assert.NotNull(lessEngineDescriptor);
            Assert.NotNull(importerDescriptor);
            Assert.NotNull(parserDescriptor);

            Assert.AreEqual(lessEngineDescriptor.Lifetime, ServiceLifetime.Transient);
            Assert.AreEqual(importerDescriptor.Lifetime, lessEngineDescriptor.Lifetime);
            Assert.AreEqual(importerDescriptor.Lifetime, lessEngineDescriptor.Lifetime);
        }
    }
}
