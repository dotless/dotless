namespace dotless.Core.Utils
{
    using Parser.Tree;

    public static class NumberExtensions
    {
        public static double Normalize(this Number value)
        {
            return value.Normalize(1d);
        }

        public static double Normalize(this Number value, double max)
        {
            return value.Normalize(0d, max);
        }

        public static double Normalize(this Number value, double min, double max)
        {
            var val = value.ToNumber(max);
            return Normalize(val, min, max);
        }

        public static double Normalize(double value)
        {
            return Normalize(value, 1d);
        }

        public static double Normalize(double value, double max)
        {
            return Normalize(value, 0d, max);
        }

        public static double Normalize(double value, double min, double max)
        {
            return value < min ? min : value > max ? max : value;
        }
    }
}