namespace dotless.Test.Unit.minifier
{
    using System.Text;
    using Core.minifier;
    using NUnit.Framework;
    public class ForcedCommentsFixture
    {

        [Test]
        public void MinifierIgnoresForcedComments()
        {
            string input = "body {background: red; /*! Hello */}";
            string desiredOutput = "body{background:red;/*! Hello */}";

            var processor = new Processor(input);
            StringBuilder builder = new StringBuilder();
            builder.Append(processor.Output);
            string output = builder.ToString();

            Assert.AreEqual(desiredOutput, output);
        }
    }
}