using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acme.Collections
{
    public class Stack
    {
        Entry top;
        public void Push(object data)
        {
            top = new Entry(top, data);
        }
        public object Pop()
        {
            if (top == null) throw new InvalidOperationException();
            object result = top.data_;
            top = top.next_;
            return result;
        }
    }
    class Entry
    {
        #region data members
        public Entry next_;
        public object data_;
        #endregion data members
        #region Constructors
        public Entry(Entry next, object data)
        {
            next_ = next;
            data_ = data;
        }
        #endregion Constructors
    }
}
