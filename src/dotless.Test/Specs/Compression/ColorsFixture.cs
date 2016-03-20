using System;
using dotless.Core.Parser;
using dotless.Core.Parser.Infrastructure;

namespace dotless.Test.Specs.Compression
{
    using NUnit.Framework;

    public class ColorsFixture : CompressedSpecFixtureBase
    {
        [Test]
        public void Colors()
        {
            AssertExpression("#fea", "#fea");
            AssertExpression("#0000ff", "#0000ff");
            AssertExpression("blue", "blue");
        }

        [Test]
        public void Should_not_compress_IE_ARGB()
        {
            AssertExpressionUnchanged("#ffaabbcc");
            AssertExpressionUnchanged("#aabbccdd");
        }
        
        [Test]
        public void Overflow()
        {
            AssertExpression("#000000", "#111111 - #444444");
            AssertExpression("#ffffff", "#eee + #fff");
            AssertExpression("#ffffff", "#aaa * 3");
            AssertExpression("#00ff00", "#00ee00 + #009900");
            AssertExpression("#ff0000", "#ee0000 + #990000");
        }

        [Test]
        public void Gray()
        {
            AssertExpression("#888888", "rgb(136, 136, 136)");
            AssertExpression("#808080", "hsl(50, 0, 50)");
        }
    }
}