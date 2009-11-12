namespace dotless.Core.engine
{
    public static class Functions
    {
        public static INode ADD(float a, float b)
        {
            return new Number(a + b);
        }
        public static INode RGB(int r, int g, int b)
        {
            return new Color(r, g, b);
        }
        public static INode URL(string url)
        {
            return new Literal(string.Format("url(\"{0}\")",url));   
        }
        
    }
}