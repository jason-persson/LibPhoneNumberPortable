using java.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.lang
{
    public class Iterable<T> : IEnumerable<T> where T : class
    {
        private Func<Iterator<T>> _iterabel;

        public Iterable(Func<Iterator<T>> iterabel)
        {
            this._iterabel = iterabel;
        }

        public Iterator<T> iterator()
        {
            return _iterabel();
        }

        // IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            return iterator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return iterator();
        }
    }
}
