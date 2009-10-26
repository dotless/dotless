namespace dotless.Core.engine
{
    public class String : Literal
    {
        public string Content { get; set; }
        public string Quotes { get; set; }
        
        public String(string str)
        {
            
            switch(str.Substring(0,1))
            {
                case "\"":
                    Quotes = "\"";
                    Content = str.Replace("\"", "");
                    break;
                case "'":
                    Quotes = "'";
                    Content = str.Replace("'", "");
                    break;
                default:
                    Quotes = string.Empty;
                    Content = string.Empty;
                    break;
            }
            Value = Content;
            //TODO: learn bloody RegEx
            //var pattern = new Regex(@"('|"")(.*?)()");
            //var match = pattern.Matches(str);
            //if (match.Count <= 1) return;
            //Quotes = match[0].Value;
            //Content = match[1].Value;
        }

        public override string ToString()
        {
            return Content;
        }
        public override string ToCss()
        {
            return string.Format("{0}{1}{0}", Quotes, Content);
        }
    }
}