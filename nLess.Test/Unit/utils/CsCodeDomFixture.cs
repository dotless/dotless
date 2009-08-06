using System;
using nless.Core.engine;
using nless.Core.utils;
using NUnit.Framework;

namespace nLess.Test.Unit.utils
{
    [TestFixture]
    public class CsCodeDomFixture
    {
        [Test]
        public void CanEvaluateSingleProperty()
        {
            var color = new Color(10, 10, 10);
            Console.WriteLine(color.ToCSharp());
            Console.WriteLine(CsEval.Eval(color.ToCSharp()));
        }
    }

 

}



