namespace dotless.Core.Response
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents a weighted value (or quality value) from an http header e.g. gzip=0.9; deflate; x-gzip=0.5;
    /// </summary>
    /// <remarks>
    /// accept-encoding spec:
    ///    http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html
    /// </remarks>
    /// <example>
    /// Accept:          text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5
    /// Accept-Encoding: gzip,deflate
    /// Accept-Charset:  ISO-8859-1,utf-8;q=0.7,*;q=0.7
    /// Accept-Language: en-us,en;q=0.5
    /// </example>
    [DebuggerDisplay("QValue[{Name}, {Weight}]")]
    public struct QValue : IComparable<QValue>
    {
        static char[] delimiters = { ';', '=' };
        const float defaultWeight = 1;

        #region Fields

        string _name;
        float _weight;
        int _ordinal;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new QValue by parsing the given value
        /// for name and weight (qvalue)
        /// </summary>
        /// <param name="value">The value to be parsed e.g. gzip=0.3</param>
        public QValue(string value)
            : this(value, 0)
        { }

        /// <summary>
        /// Creates a new QValue by parsing the given value
        /// for name and weight (qvalue) and assigns the given
        /// ordinal
        /// </summary>
        /// <param name="value">The value to be parsed e.g. gzip=0.3</param>
        /// <param name="ordinal">The ordinal/index where the item
        /// was found in the original list.</param>
        public QValue(string value, int ordinal)
        {
            _name = null;
            _weight = 0;
            _ordinal = ordinal;

            ParseInternal(ref this, value);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the value part
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// The weighting (or qvalue, quality value) of the encoding
        /// </summary>
        public float Weight
        {
            get { return _weight; }
        }

        /// <summary>
        /// Whether the value can be accepted
        /// i.e. it's weight is greater than zero
        /// </summary>
        public bool CanAccept
        {
            get { return _weight > 0; }
        }

        /// <summary>
        /// Whether the value is empty (i.e. has no name)
        /// </summary>
        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(_name); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parses the given string for name and
        /// weigth (qvalue)
        /// </summary>
        /// <param name="value">The string to parse</param>
        public static QValue Parse(string value)
        {
            QValue item = new QValue();
            ParseInternal(ref item, value);
            return item;
        }

        /// <summary>
        /// Parses the given string for name and
        /// weigth (qvalue)
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <param name="ordinal">The order of item in sequence</param>
        /// <returns></returns>
        public static QValue Parse(string value, int ordinal)
        {
            QValue item = Parse(value);
            item._ordinal = ordinal;
            return item;
        }

        /// <summary>
        /// Parses the given string for name and
        /// weigth (qvalue)
        /// </summary>
        /// <param name="value">The string to parse</param>
        static void ParseInternal(ref QValue target, string value)
        {
            string[] parts = value.Split(delimiters, 3);
            if (parts.Length > 0)
            {
                target._name = parts[0].Trim();
                target._weight = defaultWeight;
            }

            if (parts.Length == 3)
            {
                float.TryParse(parts[2], out target._weight);
            }
        }

        #endregion

        #region IComparable<QValue> Members

        /// <summary>
        /// Compares this instance to another QValue by
        /// comparing first weights, then ordinals.
        /// </summary>
        /// <param name="other">The QValue to compare</param>
        /// <returns></returns>
        public int CompareTo(QValue other)
        {
            int value = _weight.CompareTo(other._weight);
            if (value == 0)
            {
                int ord = -_ordinal;
                value = ord.CompareTo(-other._ordinal);
            }
            return value;
        }

        #endregion

        #region CompareByWeight

        /// <summary>
        /// Compares two QValues in ascending order.
        /// </summary>
        /// <param name="x">The first QValue</param>
        /// <param name="y">The second QValue</param>
        /// <returns></returns>
        public static int CompareByWeightAsc(QValue x, QValue y)
        {
            return x.CompareTo(y);
        }

        /// <summary>
        /// Compares two QValues in descending order.
        /// </summary>
        /// <param name="x">The first QValue</param>
        /// <param name="y">The second QValue</param>
        /// <returns></returns>
        public static int CompareByWeightDesc(QValue x, QValue y)
        {
            return -x.CompareTo(y);
        }

        #endregion

    }
}