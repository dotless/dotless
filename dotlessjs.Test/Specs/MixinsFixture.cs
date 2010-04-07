using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class MixinsFixture : SpecFixtureBase
  {
    [Test]
    public void Mixins()
    {
      var input =
        @"
.mixin { border: 1px solid black; }
.mixout { border-color: orange; }
.borders { border-style: dashed; }

#namespace {
  .borders {
    border-style: dotted;
  }
  .biohazard {
    content: ""death"";
    .man {
      color: transparent;
    }
  }
}
#theme {
  > .mixin {
    background-color: grey;
  }
}
#container {
  color: black;
  .mixin;
  .mixout;
  #theme > .mixin;
}

#header {
  .milk {
    color: white;
    .mixin;
    #theme > .mixin;
  }
  #cookie {
    .chips {
      #namespace .borders;
      .calories {
        #container;
      }
    }
    .borders;
  }
}
.secure-zone { #namespace .biohazard .man; }
.direct {
  #namespace > .borders;
}

";

      var expected = @"
.mixin {
  border: 1px solid black;
}
.mixout {
  border-color: orange;
}
.borders {
  border-style: dashed;
}
#namespace .borders {
  border-style: dotted;
}
#namespace .biohazard {
  content: ""death"";
}
#namespace .biohazard .man {
  color: transparent;
}
#theme > .mixin {
  background-color: grey;
}
#container {
  color: black;
  border: 1px solid black;
  border-color: orange;
  background-color: grey;
}
#header .milk {
  color: white;
  border: 1px solid black;
  background-color: grey;
}
#header #cookie {
  border-style: dashed;
}
#header #cookie .chips {
  border-style: dotted;
}
#header #cookie .chips .calories {
  color: black;
  border: 1px solid black;
  border-color: orange;
  background-color: grey;
}
.secure-zone {
  color: transparent;
}
.direct {
  border-style: dotted;
}
";

      AssertLess(input, expected);
    }
  }
}