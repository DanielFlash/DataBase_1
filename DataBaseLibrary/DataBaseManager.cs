using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLibrary
{
    public class DataBaseManager<T> where T : DataBase
    {
        T[] DataBases;

        internal string Name { get; }

        private event DataBaseStateHandler AddedDB;
        private event DataBaseStateHandler RemovedDB;
        private event DataBaseStateHandler ShownDBs;

        private void CallEvent(DataBaseEventArgs e, DataBaseStateHandler handler) 
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        private void OnAdded(DataBaseEventArgs e)
        {
            CallEvent(e, AddedDB);
        }
        private void OnRemoved(DataBaseEventArgs e)
        {
            CallEvent(e, RemovedDB);
        }
        private void OnShown(DataBaseEventArgs e)
        {
            CallEvent(e, ShownDBs);
        }

        public DataBaseManager(string name, DataBaseStateHandler AddDataBaseHandler,
            DataBaseStateHandler RemoveDataBaseHandler, DataBaseStateHandler ShowDataBaseHandler)
        {
            Name = name;
            AddedDB += AddDataBaseHandler;
            RemovedDB += RemoveDataBaseHandler;
            ShownDBs += ShowDataBaseHandler;
        }

        public void Create(string DBname, int DBindex, int n, string[] keys, string[] values, 
            DataBaseStateHandler AddMemberHandler, DataBaseStateHandler RemoveMemberHandler, 
            DataBaseStateHandler ShowMembersHandler, DataBaseStateHandler ChangeMemberHandler,
            DataBaseStateHandler FindMemberHandler)
        {
            T NewDataBase = new SimpleDataBase(DBname, DBindex, n, keys, values) as T;

            if (NewDataBase == null)
                throw new Exception("Ошибка создания базы данных");

            OnAdded(new DataBaseEventArgs("Создана новая база данных"));

            if (DataBases == null)
            {
                DataBases = new T[] { NewDataBase };
            }
            else
            {
                T[] tempDataBases = new T[DataBases.Length + 1];
                for (int i = 0; i < DataBases.Length; i++)
                {
                    tempDataBases[i] = DataBases[i];
                }
                tempDataBases[tempDataBases.Length - 1] = NewDataBase;
                DataBases = tempDataBases;
            }

            NewDataBase.AddedMember += AddMemberHandler;
            NewDataBase.RemovedMember += RemoveMemberHandler;
            NewDataBase.ShownMembers += ShowMembersHandler;
            NewDataBase.Changed += ChangeMemberHandler;
            NewDataBase.Found += FindMemberHandler;
        }

        public void Delete(int IdDB)
        {
            int index;
            T tempDataBase = FindDataBase(IdDB, out index);
            if (tempDataBase == null)
                throw new Exception("Базы данных с данным id не существует");

            if (DataBases.Length <= 1)
            {
                DataBases = null;
            }
            else
            {
                T[] tempDataBases = new T[DataBases.Length - 1];
                for (int i = 0, j = 0; i < DataBases.Length; i++)
                {
                    if (i != index)
                        tempDataBases[j++] = DataBases[i];
                }
                DataBases = tempDataBases;
            }
            OnRemoved(new DataBaseEventArgs("База данных удалена"));
        }

        public void ShowDB()
        {
            for(int i = 0; i < DataBases.Length; i++)
            {
                Console.WriteLine((i+1) + "\t" + DataBases[i].Name);
            }
            OnShown(new DataBaseEventArgs("Список баз данных выведен на экран"));
        }

        public void AddMember(int IdDB, string[] values)
        {
            T tempDataBase = FindDataBase(IdDB);
            if (tempDataBase == null)
                throw new Exception("Базы данных с данным id не существует");
            tempDataBase.Add(values);
        }

        public void RemoveMember(int index, int IdDB)
        {
            T tempDataBase = FindDataBase(IdDB);
            if (tempDataBase == null)
                throw new Exception("Базы данных с данным id не существует");
            tempDataBase.Remove(index);
        }

        public void Show(int IdDB)
        {
            T tempDataBase = FindDataBase(IdDB);
            if (tempDataBase == null)
                throw new Exception("Базы данных с данным id не существует");
            tempDataBase.Show();
        }

        public void ChangeMember(int IdDB, int index, string key, string value)
        {
            T tempDataBase = FindDataBase(IdDB);
            if (tempDataBase == null)
                throw new Exception("Базы данных с данным id не существует");
            tempDataBase.Change(index, key, value);
        }

        public void FindMember(int index, int IdDB)
        {
            T tempDataBase = FindDataBase(IdDB);
            if (tempDataBase == null)
                throw new Exception("Базы данных с данным id не существует");
            tempDataBase.Find(index);
        }

        private T FindDataBase(int id)
        {
            for (int i = 0; i < DataBases.Length; i++)
            {
                if (i == (id - 1))
                {
                    return DataBases[i];
                }
            }
            return null;
        }

        private T FindDataBase(int id, out int index)
        {
            for (int i = 0; i < DataBases.Length; i++)
            {
                if (i == (id - 1))
                {
                    index = i;
                    return DataBases[i];
                }
            }
            index = -1;
            return null;
        }

        public int FindDataBase(int id, out string[] str)
        {
            for (int i = 0; i < DataBases.Length; i++)
            {
                if (i == (id - 1))
                {
                    T tempDataBase = DataBases[i];
                    str = tempDataBase.keys;
                    return tempDataBase.N;
                }
            }
            str = null;
            return -1;
        }
    }
}