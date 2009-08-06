namespace nless.Core.engine
{
    public class Number : Literal
    {
        //Have to wrap float instead of extending
        new internal float Value { get; set; }

        public Number(float value)
        {
            Value = value;
        }

        public Number(string unit, float value)
        {
            Unit = unit;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Value, Unit);
        }

        public override string ToCSharp()
        {
            return Value.ToString();
        }
        public override string ToCss()
        {
            return string.Format("{0}{1}", Value, Unit ?? "");
        }
      //TODO: Dont get this
      //def to_css
      //  "#{(self % 1).zero?? "#{self.to_i}#@unit" : self}"
      //end

        #region operator overrides
        public static Number operator +(Number number1, Number number2)
        {
            number1.Value += number2.Value;
            return number1;
        }
        public static Number operator +(Number number1, int number2)
        {
            number1.Value += number2;
            return number1;
        }
        public static Number operator -(Number number1, Number number2)
        {
            number1.Value -= number2.Value;
            return number1;
        }
        public static Number operator -(Number number1, int number2)
        {
            number1.Value -= number2;
            return number1;
        }
        public static Number operator *(Number number1, Number number2)
        {
            number1.Value *= number2.Value;
            return number1;
        }
        public static Number operator *(Number number1, int number2)
        {
            number1.Value *= number2;
            return number1;
        }
        public static Number operator /(Number number1, Number number2)
        {
            number1.Value /= number2.Value;
            return number1;
        }
        public static Number operator /(Number number1, int number2)
        {
            number1.Value /= number2;
            return number1;
        }
        #endregion
    }

}
