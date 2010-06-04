namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class ColorsFixture : SpecFixtureBase
    {
        [Test]
        public void Colors()
        {
            // Todo: split into separate atomic tests.
            var input =
                @"
#yelow {
  #short {
    color: #fea;
  }
  #long {
    color: #ffeeaa;
  }
  #rgba {
    color: rgba(255, 238, 170, 0.1);
  }
}

#blue {
  #short {
    color: #00f;
  }
  #long {
    color: #0000ff;
  }
  #rgba {
    color: rgba(0, 0, 255, 0.1);
  }
}

#overflow {
  .a { color: #111111 - #444444; } // black
  .b { color: #eee + #fff; }       // white
  .c { color: #aaa * 3; }          // white
  .d { color: #00ee00 + #009900; } // #00ff00
}

#grey {
  color: rgb(200, 200, 200);
  background-color: hsl(50, 0, 50);
}
";

            var expected =
                @"
#yelow #short {
  color: #fea;
}
#yelow #long {
  color: #ffeeaa;
}
#yelow #rgba {
  color: rgba(255, 238, 170, 0.1);
}
#blue #short {
  color: #00f;
}
#blue #long {
  color: #0000ff;
}
#blue #rgba {
  color: rgba(0, 0, 255, 0.1);
}
#overflow .a {
  color: black;
}
#overflow .b {
  color: white;
}
#overflow .c {
  color: white;
}
#overflow .d {
  color: lime;
}
#grey {
  color: #c8c8c8;
  background-color: gray;
}
";

            AssertLess(input, expected);
        }
    }
}