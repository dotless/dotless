namespace dotless.Core
{
    public static class Less
    {
        public static string Parse(string less)
        {
            return new EngineFactory().GetEngine().TransformToCss(less, null);
        }
    }
}
