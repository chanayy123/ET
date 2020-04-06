using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public  class SimplePool
    {
        public static SimplePool Instance { get; } = new SimplePool();
        private readonly Dictionary<Type, Queue<object>> dictionary = new Dictionary<Type, Queue<object>>();

        public object Fetch(Type type)
        {
            if (!this.dictionary.TryGetValue(type, out Queue<object> queue))
            {
                queue = new Queue<object>();
                this.dictionary.Add(type, queue);
            }
            object obj;
	        if (queue.Count > 0)
	        {
		        obj = queue.Dequeue();
	        }
	        else
	        {
		        obj = Activator.CreateInstance(type);	
	        }
	        return obj;
        }

        public T Fetch<T>() where T : class
        {
            T t = (T)this.Fetch(typeof(T));
            return t;
        }

        public void Recycle(object obj)
        {
            Type type = obj.GetType();
            if (!this.dictionary.TryGetValue(type, out Queue<object> queue))
            {
                queue = new Queue<object>();
                this.dictionary.Add(type, queue);
            }
            queue.Enqueue(obj);
        }
    }
}
