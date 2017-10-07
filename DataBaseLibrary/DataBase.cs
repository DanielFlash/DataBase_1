using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLibrary
{
    public abstract class DataBase : IDataBase //professor web
    {
        public LinkedList<Member> Members = new LinkedList<Member>();
        public string Name { get; }
        public int Index { get; } //////////////лишнее поле////////////////////
        public int MaxIndex { get; set; } //количество элементов в базе данных
        public string[] keys;
        public int N { get; }

        protected internal event DataBaseStateHandler AddedMember;
        protected internal event DataBaseStateHandler RemovedMember;
        protected internal event DataBaseStateHandler ShownMembers;
        protected internal event DataBaseStateHandler Changed;
        protected internal event DataBaseStateHandler Found;

        private void CallEvent(DataBaseEventArgs e, DataBaseStateHandler handler) //не будет наследоваться
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        protected internal virtual void OnAdded(DataBaseEventArgs e)
        {
            CallEvent(e, AddedMember);
        }
        protected internal virtual void OnRemoved(DataBaseEventArgs e)
        {
            CallEvent(e, RemovedMember);
        }
        protected internal virtual void OnShown(DataBaseEventArgs e)
        {
            CallEvent(e, ShownMembers);
        }
        protected internal virtual void OnChanged(DataBaseEventArgs e)
        {
            CallEvent(e, Changed);
        }
        protected internal virtual void OnFound(DataBaseEventArgs e)
        {
            CallEvent(e, Found);
        }

        public DataBase(string DBname, int DBindex, int n, string[] keys, string[] values)
        {
            Name = DBname;
            Index = DBindex;
            MaxIndex = 1;
            N = n;
            this.keys = keys;
            Members.AddLast(new Member(N, MaxIndex, keys, values));
            OnAdded(new DataBaseEventArgs("Создан новый элемент базы данных"));
        }

        public virtual void Add(string[] values)
        {
            Members.AddLast(new Member(N, ++MaxIndex, keys, values));
            OnAdded(new DataBaseEventArgs("Создан новый элемент базы данных"));
        }

        public virtual void Remove(int index)
        {
            bool flag = false;
            Member m = null;
            foreach (Member member in Members)
            {
                if (member.index == index)
                {
                    m = member;
                    flag = true;
                    OnRemoved(new DataBaseEventArgs($"Удален элемент базы данных под номером: {member.index}"));
                }
                
                if(member.index >= index)
                {
                    member.index--;
                }
            }
            if (!flag)
            {
                OnRemoved(new DataBaseEventArgs("Ошибка удаления элемента"));
                throw new Exception("Элемента с данным id не существует");
            }
            else
            {
                Members.Remove(m);
                MaxIndex--;
            }
        }

        public virtual void Show()
        {
            Console.WriteLine();
            Console.Write("id\t");
            foreach(string k in Members.First.Value.Poles.Keys)
            {
                Console.Write("{0}\t", k);
            }
            Console.WriteLine();
            foreach (Member member in Members)
            {
                Console.Write("{0}\t", member.index);
                foreach (string v in member.Poles.Values) 
                {
                    Console.Write("{0}\t", v);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            OnShown(new DataBaseEventArgs("База данных выведена на экран"));
        }

        public virtual void Change(int index, string key, string value)
        {
            Member m = null;
            bool flag = false;
            foreach (Member member in Members)
            {
                if (member.index == index)
                {
                    m = member;
                    flag = true;
                    OnChanged(new DataBaseEventArgs($"Изменен элемент базы данных под номером: {member.index}"));
                    break;
                }
            }
            if (!flag)
            {
                OnChanged(new DataBaseEventArgs("Ошибка изменения элемента"));
                throw new Exception("Элемента с данным id не существует");
            }
            else
                m.Change(key, value);
        }

        public virtual void Find(int index)
        {
            bool flag = false;
            foreach (Member member in Members)
            {
                if (member.index == index)
                {
                    Console.WriteLine();
                    Console.Write("id\t");
                    foreach (string k in Members.First.Value.Poles.Keys)
                    {
                        Console.Write("{0}\t", k);
                    }
                    Console.WriteLine();
                    Console.Write("{0}\t", member.index);
                    foreach (string v in member.Poles.Values)
                    {
                        Console.Write("{0}\t", v);
                    }
                    Console.WriteLine();
                    flag = true;
                    OnFound(new DataBaseEventArgs($"Элемент базы данных под номером {member.index} " +
                        "выведен на экран"));
                    break;
                }
            }
            if (!flag)
            {
                OnFound(new DataBaseEventArgs("Ошибка поиска элемента"));
                throw new Exception("Элемента с данным id не существует");
            }
        }
    }
}
