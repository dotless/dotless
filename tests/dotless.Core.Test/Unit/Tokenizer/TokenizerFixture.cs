
namespace dotless.Core.Test.Unit.Tokenizer
{
    using Core.Parser;
    using NUnit.Framework;

    internal class TokenizerFixture
    {
        [Test]
        public void MatchSetsNodeIndex()
        {
            var tok = new Tokenizer(0);

            var expression = "abc - def";

            tok.SetupInput(expression, "testfile.less");

            var match1 = tok.Match(@"\w*");
            var match2 = tok.Match('-');
            var match3 = tok.Match(@"\w*");

            Assert.That(match1, Is.Not.Null);
            Assert.That(match2, Is.Not.Null);
            Assert.That(match3, Is.Not.Null);

            Assert.That(match1.Location.Index, Is.EqualTo(0));
            Assert.That(match2.Location.Index, Is.EqualTo(4));
            Assert.That(match3.Location.Index, Is.EqualTo(6));
        }
    }
}