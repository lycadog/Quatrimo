using FlatRedBall;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Data
{
    public class ObjPool<T>
    {
        public List<(T, int)> pool = [];
        List<int> poolEntryIndex = []; //Holds the index of the entry corresponding to the same index in pool
        public List<weightedEntry> entries = [];

        /// <summary>
        /// Provide a list of values and a same-length list of weights
        /// </summary>
        /// <param name="values"></param>
        /// <param name="weights"></param>
        /// <exception cref="ArgumentException"></exception>
        public ObjPool(T[] values, int[] weights)
        {
            if (values.Length != weights.Length)
            {
                throw new ArgumentException($"objPool constructor expects 2 arrays of equal length. Array 1 length: {values.Length}, Array 2 length: {weights.Length}");
            }

            for (int i = 0; i < values.Length; i++)
            {
                addNewEntry(values[i], weights[i], i);
            }
        }

        /// <summary>
        /// Provide a list of values and they will all share the same specified weight
        /// </summary>
        /// <param name="values"></param>
        /// <param name="weight"></param>
        public ObjPool(T[] values, int weight)
        {
            for (int i = 0; i < values.Length; i++)
            {
                addNewEntry(values[i], weight, i);
            }
        }

        /// <summary>
        /// Use for single element pools
        /// </summary>
        /// <param name="value"></param>
        public ObjPool(T value)
        {
            addNewEntry(value, 1, 0);
        }

        /// <summary>
        /// Use to create an empty pool and add elements later
        /// </summary>
        public ObjPool() { }

        public T getRandom(out weightedEntry entry)
        {
            int index = FlatRedBallServices.Random.Next(pool.Count);

            var randomTuple = pool[index];

            T obj = randomTuple.Item1;
            entry = entries[randomTuple.Item2];
            return obj;
        }

        public T getRandom()
        {
            int index = FlatRedBallServices.Random.Next(pool.Count);

            var randomTuple = pool[index];

            T obj = randomTuple.Item1;
            return obj;
        }


        public weightedEntry addNewEntry(T entry, int weight, int index)
        {
            weightedEntry newEntry = new weightedEntry(this, entry, weight, index);
            entries.Add(newEntry);
            return newEntry;
        }

        public void clearPool()
        {
            foreach (var entry in entries)
            {
                entry.removeEntry();
            }
        }

        public class weightedEntry
        {
            ObjPool<T> parent;

            T entry;
            public int weight;
            int index;

            internal weightedEntry(ObjPool<T> parent, T entry, int weight, int index)
            {
                this.parent = parent;
                this.entry = entry;
                this.index = index;
                setWeight(weight);
            }

            public void addWeight(int value)
            {
                for (int i = 0; i < value; i++)
                {
                    weight += 1;
                    if (weight > 0)
                    {
                        parent.pool.Add((entry, index));
                    }

                }
            }

            public void subtractWeight(int value)
            {
                for (int i = 0; i < value; i++)
                {
                    if (weight > 0) { parent.pool.Remove((entry, index)); }
                    weight -= 1;
                }
            }
            public void setWeight(int value)
            {
                for (int i = 0; i < weight; i++)
                {
                    parent.pool.Remove((entry, index));
                }

                for (int i = 0; i < value; i++)
                {
                    parent.pool.Add((entry, index));
                }
                weight = value;
            }

            public void removeEntry()
            {
                for (int i = 0; i < weight; i++)
                {
                    parent.pool.Remove((entry, index));
                }
                parent.entries.Remove(this);
                weight = 0;
            }
        }
    }
}
