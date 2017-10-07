using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLibrary
{
    public class SimpleDataBase : DataBase
    {
        public SimpleDataBase(string DBname, int DBindex, int n, string[] keys, string[] values) : base(DBname, DBindex,
            n, keys, values) { }

        public override void Add(string[] values)
        {
            Console.WriteLine("Добавление нового элемента в базу данных");
            base.Add(values);
        }

        public override void Remove(int index)
        {
            Console.WriteLine("Удаление элемента из базы данных");
            base.Remove(index);
        }

        public override void Show()
        {
            Console.WriteLine("Вывод базы данных на экран");
            base.Show();
        }

        public override void Change(int index, string key, string value)
        {
            Console.WriteLine("Изменение элемента базы данных");
            base.Change(index, key, value);
        }

        public override void Find(int index)
        {
            Console.WriteLine("Вывод элемента на экран");
            base.Find(index);
        }
    }
}
