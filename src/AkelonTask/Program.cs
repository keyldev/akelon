using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using AkelonTask.Models;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace AkelonTask
{

    internal class Program
    {
        static DataProcessor? dp;
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу с данными:");
            string? filePath = Console.ReadLine();
            dp = new DataProcessor(filePath ?? AppDomain.CurrentDomain.BaseDirectory + "\\data.xlsx");

            ShowMenu();

            Console.ReadKey();
        }
        private static void ShowMenu()
        {
            Console.Clear();
            
            Console.WriteLine("Выберите интересующее Вас действие:\n1. Получить информацию по наименованию товара\n2. Изменить контактное лицо для организации\n3. Узнать золотого клиента\n4. Выйти из программы");

            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1:
                    {
                        Console.WriteLine("Введите наименование интересующего товара: ");
                        string? productName = Console.ReadLine();
                        dp.FindClientsInfoByProductName(productName);

                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Введите имя организации, в которой Вы хотите изменить контактное лицо: ");

                        var orgzName = Console.ReadLine();
                        Console.WriteLine("Введите новое ФИО: ");
                        var newName = Console.ReadLine();

                        dp.SetContactName(orgzName, newName);

                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("Стало быть Вы хотите узнать золотого клиента. Введите год: ");
                        int year = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("А теперь введите месяц: ");
                        int month = Convert.ToInt32(Console.ReadLine());

                        dp.FindGoldenClient(year, month);

                        break;
                    }
                case 4:
                    {
                        Environment.Exit(0);
                        break;
                    }
                default:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ошибка. Введенного вами кода не существует..\nДля продолжения нажмите любую клавишу.");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.ReadKey();
                        ShowMenu();
                        break;
                    }
            }
            Console.WriteLine("\nДля продолжения нажмите любую клавишу..");
            Console.ReadKey();
            ShowMenu();
        }
    }
}