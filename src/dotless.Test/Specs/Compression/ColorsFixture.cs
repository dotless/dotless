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
            AssertExpression("#000", "#111111 - #444444");
            AssertExpression("#fff", "#eee + #fff");
            AssertExpression("#fff", "#aaa * 3");
            AssertExpression("lime", "#00ee00 + #009900");
            AssertExpression("red", "#ee0000 + #990000");
        }

        [Test]
        public void Gray()
        {
            AssertExpression("#888", "rgb(136, 136, 136)");
            AssertExpression("gray", "hsl(50, 0, 50)");
        }

        [Test]
        public void DisableColorCompression()
        {
            var oldEnv = DefaultEnv();

            DefaultEnv = () => new Env(null)
                {
                    Compress = true,
                    DisableColorCompression = false
                };
            AssertExpression("#111", "#111111");

            DefaultEnv = () => new Env(null)
            {
                Compress = true,
                DisableColorCompression = true
            };
            AssertExpression("#111111", "#111111");

            DefaultEnv = () => oldEnv;
        }
    }
}