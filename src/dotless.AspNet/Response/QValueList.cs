namespace dotless.Core.Response
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Provides a collection for working with qvalue http headers
    /// </summary>
    /// <remarks>
    /// accept-encoding spec:
    ///    http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html
    /// </remarks>
    [DebuggerDisplay("QValue[{Count}, {AcceptWildcard}]")]
    public sealed class QValueList : List<QValue>
    {
        static char[] delimiters = { ',' };

        #region Fields

        bool _acceptWildcard;
        bool _autoSort;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an QValueList list from
        /// the given string of comma delimited values
        /// </summary>
        /// <param name="values">The raw string of qvalues to load</param>
        public QValueList(string values)
            : this(null == values ? new string[0] : values.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
        { }

        /// <summary>
        /// Creates a new instance of an QValueList from
        /// the given string array of qvalues
        /// </summary>
        /// <param name="values">The array of qvalue strings
        /// i.e. name(;q=[0-9\.]+)?</param>
        /// <remarks>
        /// Should AcceptWildcard include */* as well?
        /// What about other wildcard forms?
        /// </remarks>
        public QValueList(string[] values)
        {
            int ordinal = -1;
            foreach (string value in values)
            {
                QValue qvalue = QValue.Parse(value.Trim(), ++ordinal);
                if (qvalue.Name.Equals("*")) // wildcard
                    _acceptWildcard = qvalue.CanAccept;
                Add(qvalue);
            }

            /// this list should be sorted by weight for
            /// methods like FindPreferred to work correctly
            DefaultSort();
            _autoSort = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whether or not the wildcarded encoding is available and allowed
        /// </summary>
        public bool AcceptWildcard
        {
            get { return _acceptWildcard; }
        }

        /// <summary>
        /// Whether, after an add operation, the list should be resorted
        /// </summary>
        public bool AutoSort
        {
            get { return _autoSort; }
            set { _autoSort = value; }
        }

        /// <summary>
        /// Synonym for FindPreferred
        /// </summary>
        /// <param name="candidates">The preferred order in which to return an encoding</param>
        /// <returns>An QValue based on weight, or null</returns>
        public QValue this[params string[] candidates]
        {
            get { return FindPreferred(candidates); }
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds an item to the list, then applies sorting
        /// if AutoSort is enabled.
        /// </summary>
        /// <param name="item">The item to add</param>
        public new void Add(QValue item)
        {
            base.Add(item);

            applyAutoSort();
        }

        #endregion

        #region AddRange

        /// <summary>
        /// Adds a range of items to the list, then applies sorting
        /// if AutoSort is enabled.
        /// </summary>
        /// <param name="collection">The items to add</param>
        public new void AddRange(IEnumerable<QValue> collection)
        {
            bool state = _autoSort;
            _autoSort = false;

            base.AddRange(collection);

            _autoSort = state;
            applyAutoSort();
        }

        #endregion

        #region Find

        /// <summary>
        /// Finds the first QValue with the given name (case-insensitive)
        /// </summary>
        /// <param name="name">The name of the QValue to search for</param>
        /// <returns></returns>
        public QValue Find(string name)
        {
            Predicate<QValue> criteria = delegate(QValue item) { return item.Name.Equals(name, StringComparison.OrdinalIgnoreCase); };
            return Find(criteria);
        }

        #endregion

        #region FindHighestWeight

        /// <summary>
        /// Returns the first match found from the given candidates
        /// </summary>
        /// <param name="candidates">The list of QValue names to find</param>
        /// <returns>The first QValue match to be found</returns>
        /// <remarks>Loops from the first item in the list to the last and finds
        /// the first candidate - the list must be sorted for weight prior to
        /// calling this method.</remarks>
        public QValue FindHighestWeight(params string[] candidates)
        {
            Predicate<QValue> criteria = delegate(QValue item)
            {
                return isCandidate(item.Name, candidates);
            };
            return Find(criteria);
        }

        #endregion

        #region FindPreferred

        /// <summary>
        /// Returns the first match found from the given candidates that is accepted
        /// </summary>
        /// <param name="candidates">The list of names to find</param>
        /// <returns>The first QValue match to be found</returns>
        /// <remarks>Loops from the first item in the list to the last and finds the
        /// first candidate that can be accepted - the list must be sorted for weight
        /// prior to calling this method.</remarks>
        public QValue FindPreferred(params string[] candidates)
        {
            Predicate<QValue> criteria = delegate(QValue item)
            {
                return isCandidate(item.Name, candidates) && item.CanAccept;
            };
            return Find(criteria);
        }

        #endregion

        #region DefaultSort

        /// <summary>
        /// Sorts the list comparing by weight in
        /// descending order
        /// </summary>
        public void DefaultSort()
        {
            Sort(QValue.CompareByWeightDesc);
        }

        #endregion

        #region applyAutoSort

        /// <summary>
        /// Applies the default sorting method if
        /// the autosort field is currently enabled
        /// </summary>
        void applyAutoSort()
        {
            if (_autoSort)
                DefaultSort();
        }

        #endregion

        #region isCandidate

        /// <summary>
        /// Determines if the given item contained within the applied array
        /// (case-insensitive)
        /// </summary>
        /// <param name="item">The string to search for</param>
        /// <param name="candidates">The array to search in</param>
        /// <returns></returns>
        static bool isCandidate(string item, params string[] candidates)
        {
            foreach (string candidate in candidates)
            {
                if (candidate.Equals(item, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        #endregion

    }
}
