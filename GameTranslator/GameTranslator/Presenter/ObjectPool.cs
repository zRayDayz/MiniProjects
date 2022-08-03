using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTranslator
{
    class ObjectPool<T>
    {
        public ObjectPool(int initPoolSize)
        {
            pool = new Queue<T>(initPoolSize);
        }

        public int Count { get { return pool.Count; } }

        Queue<T> pool;

        public T GetObject()
        {
            return pool.Dequeue();
        }
        public void ReturnObject(T obj)
        {
            pool.Enqueue(obj);
        }
    }

    class ListOfStringPool
    {
        static ListOfStringPool instance;
        public static ListOfStringPool Instance
        {
            get
            {
                if (instance == null) instance = new ListOfStringPool();
                return instance;
            }
        }

        ObjectPool<List<string>> objectPool;
        ListOfStringPool()
        {
            int initCount = 4;
            objectPool = new ObjectPool<List<string>>(initCount);
            for (int i = 0; i < initCount; i++)
            {
                objectPool.ReturnObject(new List<string>(32));
            }
        }

        public List<string> GetObject()
        {
            List<string> list;
            if (objectPool.Count == 0) list = new List<string>(32);
            else list = objectPool.GetObject();

            return list;
        }

        public void ReturnObject(List<string> obj, bool clearList = true)
        {
            if (clearList) obj.Clear();
            objectPool.ReturnObject(obj);
        }

    }
}
