using System.Collections.Generic;
using System.Threading;

namespace RedGTR_VLBL
{
    public class FixedSizeLIFO<T>
    {
        #region Properties

        List<T> items;
        int synLock = 0;

        public int Size { get; private set; }

        #endregion

        #region Constructor

        public FixedSizeLIFO(int size)
        {
            Size = size;
            items = new List<T>(size);            
        }

        #endregion

        #region Methods

        public void Lock()
        {
            while (Interlocked.CompareExchange(ref synLock, 1, 0) != 0)
                Thread.SpinWait(1);
        }

        public void Unlock()
        {
            Interlocked.Decrement(ref synLock);
        }

        public T this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public void Add(T item)
        {
            while (Interlocked.CompareExchange(ref synLock, 1, 0) != 0)
                Thread.SpinWait(1);

            if ((items.Count + 1) > Size)
            {
                items.RemoveAt(items.Count - 1);
            }
            
            items.Insert(0, item);

            Interlocked.Decrement(ref synLock);
        }

        public void Clear()
        {
            while (Interlocked.CompareExchange(ref synLock, 1, 0) != 0)
                Thread.SpinWait(1);

            items.Clear();

            Interlocked.Decrement(ref synLock);
        }

        public bool Contains(T item)
        {
            while (Interlocked.CompareExchange(ref synLock, 1, 0) != 0)
                Thread.SpinWait(1);

            bool result = items.Contains(item);

            Interlocked.Decrement(ref synLock);

            return result;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            while (Interlocked.CompareExchange(ref synLock, 1, 0) != 0)
                Thread.SpinWait(1);

            items.CopyTo(array, arrayIndex);

            Interlocked.Decrement(ref synLock);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public T[] ToArray()
        {
            T[] result;

            while (Interlocked.CompareExchange(ref synLock, 1, 0) != 0)
                Thread.SpinWait(1);

            result = items.ToArray();

            Interlocked.Decrement(ref synLock);

            return result;
        }

        #endregion
    }
}
