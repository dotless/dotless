using System;
using NUnit.Framework;

namespace nLess.Test.Unit.utils
{
    using dotless.Core.engine;
    using dotless.Core.utils;

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



