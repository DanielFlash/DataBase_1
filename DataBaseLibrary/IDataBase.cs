using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLibrary
{
    interface IDataBase
    {   
        void Add(string[] values);
        void Remove(int index);
        void Show();
        void Change(int index, string key, string value);
        void Find(int index);
    }
}