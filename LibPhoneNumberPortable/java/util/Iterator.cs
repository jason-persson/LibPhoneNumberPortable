using java.lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace java.util
{
    public class Iterator<T> : IEnumerator<T> where T : class 
    {
        private T _current = null;
        private List<T> enumerable;

        public Iterator()
        {
        }

        public Iterator(List<T> enumerable)
        {
            this.enumerable = enumerable;
        }

        int index = -1;
        public virtual boolean hasNext()
        {
            return index + 1 < enumerable.Count;
        }

        public virtual T next()
        {
            return enumerable[++index];
        }

        public virtual void remove()
        {
            enumerable.RemoveAt(index);
            index--;
        }

        // IEnumerator<T>
        public T Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool MoveNext()
        {
            if (!hasNext())
                return false;
            _current = next();
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

    }
}
