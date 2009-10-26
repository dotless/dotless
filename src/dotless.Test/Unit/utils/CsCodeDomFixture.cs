namespace dotless.Test.Unit.utils
{
    using System;
    using NUnit.Framework;
    using Core.engine;
    using Core.utils;

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