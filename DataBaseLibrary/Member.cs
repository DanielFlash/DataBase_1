using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLibrary
{
    public class Member : IMember
    {
        public Dictionary<string, string> Poles = new Dictionary<string, string>();
        public int index;

        public Member(int n, int index, string[] keys, string[] values)
        {
            for (int i = 0; i < n; i++)
            {
                Poles.Add(keys[i], values[i]);
                if (!Poles.ContainsKey(keys[i]))
                {
                    throw new Exception("Ошибка создания элемента");
                }
            }
            this.index = index;
        }

        public void Change(string key, string value)
        {
            foreach (KeyValuePair<string, string> pair in Poles)
            {
                if (pair.Key == key)
                {
                    Poles.Remove(key);
                    Poles.Add(key, value);
                    if (!Poles.ContainsKey(key))
                    {
                        throw new Exception("Ошибка изменения элемента");
                    }
                    break;
                }
            }
        }
    }
}