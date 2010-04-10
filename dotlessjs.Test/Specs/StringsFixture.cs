using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class StringsFixture : SpecFixtureBase
  {
    [Test]
    public void Strings()
    {
      var input =
        @"
#strings {
  background-image: url(""http://son-of-a-banana.com"");
  quotes: ""~"" ""~"";
  content: ""#*%:&^,)!.(~*})"";
  empty: """";
}
";

      var expected =
        @"
#strings {
  background-image: url(""http://son-of-a-banana.com"");
  quotes: ""~"" ""~"";
  content: ""#*%:&^,)!.(~*})"";
  empty: """";
}
";

      AssertLess(input, expected);
    }
    [Test]
    public void Comments()
    {
      var input =
        @"
#comments {
  content: ""/* hello */ // not-so-secret"";
}
";

      var expected =
        @"
#comments {
  content: ""/* hello */ // not-so-secret"";
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void SingleQuotes()
    {
      var input =
        @"
#single-quote {
  quotes: ""'"" ""'"";
  content: '""""#!&""""';
  empty: '';
}
";

      var expected =
        @"
#single-quote {
  quotes: ""'"" ""'"";
  content: '""""#!&""""';
  empty: '';
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void EscapingQuotes()
    {
      var input =
        @"
#escaping-quotes {
  content: ""\"""";
  content: '\'';
  content: '\'\""';
}
";

      var expected =
        @"
#escaping-quotes {
  content: ""\"""";
  content: '\'';
  content: '\'\""';
}";

      AssertLess(input, expected);
    }


    [Test]
    public void BracesInQuotes()
    {
      var input =
        @"
#single-quote {
  quotes: ""{"" ""}"";
}
";

      var expected =
        @"
#single-quote {
  quotes: ""{"" ""}"";
}
";

      AssertLess(input, expected);
    }
  }
}