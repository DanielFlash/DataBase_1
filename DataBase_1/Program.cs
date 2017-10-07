using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseLibrary;

namespace DataBase_1
{
    class Program
    {
        static Dictionary<int, int> DBs = new Dictionary<int, int>();
        static int DBindex = 0;

        static void Main(string[] Args)
        {
            DataBaseManager<DataBase> manager = new DataBaseManager<DataBase>("Manager", AddDataBaseHandler,
                RemoveDataBaseHandler, ShowDataBasesHandler);
            bool alive = true;
            while (alive)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("1. Создать БД \t 2. Удалить БД \t 3. Показать список БД");
                Console.WriteLine("4. Добавить элемент \t 5. Удалить элемент \t 6. Изменить элемент");
                Console.WriteLine("7. Показать БД \t 8. Вывести элемент \t 9. Выйти из программы");
                Console.Write("Введите номер пункта: ");
                Console.ForegroundColor = color;
                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            CreateDB(manager);
                            break;
                        case 2:
                            DeleteDB(manager);
                            break;
                        case 3:
                            ShowDBs(manager);
                            break;
                        case 4:
                            AddMember(manager);
                            break;
                        case 5:
                            RemoveMember(manager);
                            break;
                        case 6:
                            ChangeMember(manager);
                            break;
                        case 7:
                            ShowDB(manager);
                            break;
                        case 8:
                            FindMember(manager);
                            break;
                        case 9:
                            alive = false;
                            continue;
                        default:
                            color = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Неверный номер пункта");
                            Console.ForegroundColor = color;
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
        }

        private static void CreateDB(DataBaseManager<DataBase> manager)
        {
            Console.WriteLine("Создание новой базы данных");
            Console.Write("Введите имя базы данных: ");
            string DBname = Console.ReadLine();
            Console.Write("\nУкажите количество полей базы данных: ");
            int n = Convert.ToInt32(Console.ReadLine());
            DBs.Add(++DBindex, n);
            Console.WriteLine("\nУкажите названия полей: ");
            string[] keys = new string[n];
            for(int i = 0; i < n; i++)
                keys[i] = Console.ReadLine();
            Console.WriteLine("\nУкажите значения полей для первого элемента: ");
            string[] values = new string[n];
            for (int i = 0; i < n; i++)
            {
                Console.Write(keys[i] + ": ");
                values[i] = Console.ReadLine();
            }
            Console.WriteLine();

            manager.Create(DBname, DBindex, n, keys, values, AddMemberHandler, RemoveMemberHandler, ShowDataBaseHandler, ChangeMemberHandler, FindMemberHandler);
            Console.WriteLine();
        }

        private static void DeleteDB(DataBaseManager<DataBase> manager)
        {
            Console.WriteLine("Удаление базы данных");
            Console.Write("Введите номер базы данных, которую вы хотите удалить: ");
            int IdDB = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            manager.Delete(IdDB);
            Console.WriteLine();
        }

        private static void ShowDBs(DataBaseManager<DataBase> manager)
        {
            Console.WriteLine("\nВывод списка баз данных на экран");
            //Console.WriteLine();

            manager.ShowDB();
            Console.WriteLine();
        }

        private static void AddMember(DataBaseManager<DataBase> manager)
        {
            Console.WriteLine("Добавление элемента базы данных");
            Console.Write("Введите номер базы данных, в которую вы хотите добавить новый элемент: ");
            int IdDB = Convert.ToInt32(Console.ReadLine());
            string[] keys = null;
            int n = manager.FindDataBase(IdDB, out keys);
            if (n == -1 || keys == null || (n == -1 && keys == null))
                throw new Exception("Базы данных с данным номером не существует");
            Console.WriteLine("\nУкажите значения полей: ");
            string[] values = new string[n];
            for (int i = 0; i < n; i++)
            {
                Console.Write(keys[i] + ": ");
                values[i] = Console.ReadLine();
            }
            Console.WriteLine();

            manager.AddMember(IdDB, values);
            Console.WriteLine();
        }

        private static void RemoveMember(DataBaseManager<DataBase> manager)
        {
            Console.WriteLine("Удаление элемента базы данных");
            Console.Write("Введите номер базы данных, из которого вы хотите удалить элемент: ");
            int IdDB = Convert.ToInt32(Console.ReadLine());
            Console.Write("\nВведите номер элемента, которого вы хотите удалить: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            manager.RemoveMember(id, IdDB);
            Console.WriteLine();
        }

        private static void ChangeMember(DataBaseManager<DataBase> manager)
        {
            Console.WriteLine("Изменение элемента базы данных");
            Console.Write("Введите номер базы данных, в которой вы хотите изменить элемент: ");
            int IdDB = Convert.ToInt32(Console.ReadLine());
            Console.Write("\nВведите номер элемента, которого вы хотите изменить: ");
            int id = Convert.ToInt32(Console.ReadLine());
            string[] keys = null;
            int n = manager.FindDataBase(IdDB, out keys);
            Console.Write("\nВведите название поля, которое вы хотите изменить: ");
            string key = Console.ReadLine();
            int j = 0;
            for(int i = 0; i < n; i++)
            {
                if (keys[i] == key)
                    j++;
            }
            if (j == 0)
                throw new Exception("Такого поля не существует");
            Console.Write("\nВведите новое значение: ");
            string value = Console.ReadLine();
            Console.WriteLine();

            manager.ChangeMember(IdDB, id, key, value);
            Console.WriteLine();

        }

        private static void ShowDB(DataBaseManager<DataBase> manager)
        {
            Console.WriteLine("Вывод базы данных на экран");
            Console.Write("Введите номер базы данных, которую вы хотите вывести на экран: ");
            short IdDB = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine();

            manager.Show(IdDB);
            Console.WriteLine();
        }

        private static void FindMember(DataBaseManager<DataBase> manager)
        {
            Console.WriteLine("Вывод элемента базы данных");
            Console.Write("Введите номер базы данных, в которой вы хотите искать: ");
            int IdDB = Convert.ToInt32(Console.ReadLine());
            Console.Write("\nВведите номер элемента, которого вы хотите вывести: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            manager.FindMember(id, IdDB);
            Console.WriteLine();
        }


        private static void AddDataBaseHandler(object sender, DataBaseEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private static void RemoveDataBaseHandler(object sender, DataBaseEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private static void ShowDataBasesHandler(object sender, DataBaseEventArgs e)
        {
            Console.WriteLine(e.Message);
        }


        private static void AddMemberHandler(object sender, DataBaseEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private static void RemoveMemberHandler(object sender, DataBaseEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private static void ShowDataBaseHandler(object sender, DataBaseEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private static void ChangeMemberHandler(object sender, DataBaseEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private static void FindMemberHandler(object sender, DataBaseEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
