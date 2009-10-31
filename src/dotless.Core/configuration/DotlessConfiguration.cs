namespace dotless.Core.configuration
{
    public class DotlessConfiguration
    {
        public bool MinifyOutput { get; set; }
        public bool CacheEnabled { get; set; }

        public static DotlessConfiguration Default
        {
            get
            {
                return new DotlessConfiguration
                           {
                               MinifyOutput = false,
                               CacheEnabled = true
                           };
            }
        }
    }
}