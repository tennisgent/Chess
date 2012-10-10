using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentAI
{
    class PriorityQueue
    {

        public class PriorityQueue<TValue> : PriorityQueue<TValue, int> { }

        public class PriorityQueue<TValue, TPriority> where TPriority : IComparable
        {
            private SortedDictionary<TPriority, Queue<TValue>> dict = new SortedDictionary<TPriority, Queue<TValue>>();

            public int Count { get; private set; }
            public bool Empty { get { return Count == 0; } }

            public void Add(TValue val)
            {
                Add(val, default(TPriority));
            }

            public void Add(TValue val, TPriority pri)
            {
                ++Count;
                if (!dict.ContainsKey(pri)) dict[pri] = new Queue<TValue>();
                dict[pri].Enqueue(val);
            }

            public TValue Pop()
            {
                --Count;
                var item = dict.Last();
                if (item.Value.Count == 1) dict.Remove(item.Key);
                return item.Value.Dequeue();
            }
        }

    }
}
