namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class CssFixture : SpecFixtureBase
    {
        [Test]
        public void CharsetDirective()
        {
            var input = "@charset \"utf-8\";";

            AssertLessUnchanged(input);
        }

        [Test]
        public void Directives()
        {
            var input =
                @"
@media print {
  font-size: 3em;
}
@media screen {
  font-size: 10px;
}
@font-face {
  font-family: 'Garamond Pro';
  src: url(""/fonts/garamond-pro.ttf"");
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void DirectiveWithNoSpace()
        {
            // fixes bug in 1.2
            var input =
                @"
@media print{
  font-size: 3em;
}";

            var expected = @"
@media print {
  font-size: 3em;
}";
            AssertLess(input, expected);
        }


        [Test]
        public void TagSelector()
        {
            var input = @"
div {
  color: black;
}
div {
  width: 99%;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void GlobalSelector()
        {
            var input = @"
* {
  min-width: 45em;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void ChildSelector()
        {
            var input = @"
h1,
h2 > a > p,
h3 {
  color: none;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void ClassAndIdSelectors()
        {
            var input =
                @"
div.class {
  color: blue;
}
div#id {
  color: green;
}
.class#id {
  color: purple;
}
.one.two.three {
  color: grey;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void PseudoClass()
        {
            var input = @"
a:hover,
a:link {
  color: black;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void FirstChildPseudoClass()
        {
            var input = @"
p,
p:first-child {
  text-transform: none;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void LangPseudoClass()
        {
            var input = @"
q:lang(no) {
  quotes: none;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void NextSiblingSelector()
        {
            var input = @"
p + h1 {
  font-size: 2.2em;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void PropertyWithNumbers()
        {
            var input = @"
textarea {
  scrollbar-3dlight-color: yellow;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void PropertyWithUnderscore()
        {
            var input = @"
p {
  color: red;
  _color: yellow;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void Shorthands()
        {
            var input =
                @"
#shorthands {
  border: 1px solid black;
  font: 12px/16px Arial;
  margin: 1px 0;
  padding: 0 auto;
  background: url(""http://www.lesscss.org/spec.html"") no-repeat 0 4px;
}
#more-shorthands {
  margin: 0;
  padding: 1px 0 2px 0;
  font: normal small/20px 'Trebuchet MS', Verdana, sans-serif;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void Misc()
        {
            var input =
                @"
.misc {
  -moz-border-radius: 2px;
  display: -moz-inline-stack;
  width: .1em;
  background-color: #009998;
  background-image: url(images/image.jpg);
  background: -webkit-gradient(linear, left top, left bottom, from(red), to(blue));
  margin: ;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void CapitalProperties()
        {
            var input =
                @"
.misc {
  -Moz-Border-Radius: 2Px;
  dISplay: NONE;
  WIDTH: .1EM;
}
";

            AssertLessUnchanged(input);
        }


        [Test]
        public void Important()
        {
            var input =
                @"
#important {
  color: red !important;
  width: 100%!important;
  height: 20px ! important;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void ImportantVariable()
        {
            var input =
                @"
@c: white;
#important {
  color: @c !important;
}
";
            var output =
                @"
#important {
  color: white !important;
}
";
            AssertLess(input, output);
        }

        [Test]
        public void AttributeEquals()
        {
            var input = @"
input[type=""text""] {
  font-weight: normal;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void AttributeExists()
        {
            var input =
                @"
h2[title] {
  font-size: 100%;
}
[disabled] {
  color: transparent;
}
form[data-disabled] {
  color: #123456;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void EmptyUrl()
        {
            AssertExpressionUnchanged("url()");
        }


        [Test]
        public void CheckUrlWithHorizBarCharacterIsAcceptedWithoutQuotes()
        {
            // Note: https://github.com/dotless/dotless/issues/30
            var input = @"body {
  background-image: url(pickture.asp?id=this|thing);
}";

            AssertLessUnchanged(input);
        }

        [Test]
        public void CheckUrlWithData()
        {

            var input = @".help-icon {
  background: url(""data:image/gif;base64,R0lGODlhDAAMAMQAAAAAAP////T4/ujx/ery/UKP7k6W71CX71KY71Sa8Gak8XGr8nWu83ev83qv83ux83yx832y85XA9aHH96rN+KvO+KzO+K3O+LnV+cTc+tDj+9Xm+////wAAAAAAAAAAACH5BAEAABwALAAAAAAMAAwAAAVKIMcRVoMgjUWI3BYVcOxs41tIGqbAEQkvgshkECv9XopAzBQrKASX2KmZyTRRzUYia2lqdkWCrbAwxHqtRxMGoYkIFcbhwKCsOCEAOw=="") center bottom no-repeat;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void CheckUrlWithDataContainingQuoted()
        {

            var input = @".help-icon {
  background: url('data:image/svg+xml, <svg version=""1.1""><g></g></svg>');
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void CheckUrlWithDataNotQuoted()
        {

            var input = @".help-icon {
  background: #eeeeee url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAABlBMVEX///////9VfPVsAAAACklEQVQImWNgAAAAAgAB9HFkpgAAAABJRU5ErkJggg%3D%3D) repeat-y 13em 0;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void HttpUrl()
        {
            AssertExpressionUnchanged(@"url(http://), ""}"", url(""http://}"")");
        }

        [Test]
        public void HttpUrlClosingBraceOnSameLine()
        {
            var input = @"
.trickyurl {
image: url(http://); }";

            var expected = @"
.trickyurl {
  image: url(http://);
}";
            AssertLess(input, expected);
        }

        [Test]
        public void HttpUrl3()
        {
            var input = @"
.trickyurl {
  image: url(http://); 
}";
            var expected = @"
.trickyurl {
  image: url(http://);
}";
            AssertLess(input, expected);
        }

        [Test]
        public void HttpUrl4()
        {
            var input = @"
.trickyurl {
  image: url(""""), url(http://); 
}";
            var expected = @"
.trickyurl {
  image: url(""""), url(http://);
}";
            AssertLess(input, expected);
        }

        [Test]
        public void SupportIEFilters()
        {
            var input = @"
@fat: 0;
@cloudhead: ""#000000"";

.nav {
  filter: progid:DXImageTransform.Microsoft.Alpha(opacity = 20);
  filter: progid:DXImageTransform.Microsoft.Alpha(opacity=@fat);
  filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=""#333333"", endColorstr=@cloudhead, GradientType=@fat);
}";

            var expected = @"
.nav {
  filter: progid:DXImageTransform.Microsoft.Alpha(opacity=20);
  filter: progid:DXImageTransform.Microsoft.Alpha(opacity=0);
  filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=""#333333"", endColorstr=""#000000"", GradientType=0);
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void DuplicatesRemoved1()
        {
            var input = @"
.test {
  background: none;
  background: none;
}";

            var expected = @"
.test {
  background: none;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void DuplicatesRemoved2()
        {
            var input = @"
.test {
  background: ~""none"";
  color: red;
  background: none;
}";

            var expected = @"
.test {
  background: none;
  color: red;
}";
            AssertLess(input, expected);
        }
    }
}