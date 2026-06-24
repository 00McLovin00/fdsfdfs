using System;
using System.IO;

namespace Exercise5
{
    class Program
    {
        private static TextCorrector _corrector = new TextCorrector();

        static void Main()
        {
            Console.Title = "Корректор текстов";
            Console.WriteLine("=== КОРРЕКТОР ТЕКСТОВ И ФОРМАТТЕР ТЕЛЕФОНОВ ===\n");

            bool exit = false;
            while (!exit)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": ProcessDirectoryMenu(); break;
                        case "2": ProcessSingleFileMenu(); break;
                        case "3": ShowDictionaryMenu(); break;
                        case "4": EditDictionaryMenu(); break;
                        case "5": TestCorrectionMenu(); break;
                        case "6": exit = true; Console.WriteLine("До свидания!"); break;
                        default: Console.WriteLine("Неверный выбор!"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Обработать все файлы в директории");
            Console.WriteLine("2. Обработать один файл");
            Console.WriteLine("3. Показать словарь ошибочных слов");
            Console.WriteLine("4. Редактировать словарь");
            Console.WriteLine("5. Тестовое исправление текста");
            Console.WriteLine("6. Выйти");
            Console.Write("\nВаш выбор: ");
        }

        static void ProcessDirectoryMenu()
        {
            Console.Write("Путь к директории: ");
            string directoryPath = Console.ReadLine();

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Директория не найдена!");
                return;
            }

            Console.Write("Создавать резервные копии? (да/нет): ");
            bool saveBackup = Console.ReadLine().ToLower() == "да";

            Console.WriteLine("\nНачинаю обработку...\n");
            _corrector.ProcessDirectoryWithProgress(directoryPath, "*.txt", saveBackup);
        }

        static void ProcessSingleFileMenu()
        {
            Console.Write("Путь к файлу: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не найден!");
                return;
            }

            Console.Write("Создать резервную копию? (да/нет): ");
            bool saveBackup = Console.ReadLine().ToLower() == "да";

            // Показываем статистику до обработки
            _corrector.ShowFileStats(filePath);

            Console.WriteLine("\nНачинаю обработку...");
            _corrector.ProcessFile(filePath, saveBackup);

            // Показываем статистику после обработки
            Console.WriteLine("\nПосле обработки:");
            _corrector.ShowFileStats(filePath);
        }

        static void ShowDictionaryMenu()
        {
            _corrector.ErrorDictionary.PrintDictionary();

            Console.WriteLine($"\nВсего правильных слов: {_corrector.ErrorDictionary.Count}");
            Console.WriteLine("\nВведите слово для поиска правильного варианта (или Enter для выхода):");
            string input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                string correct = _corrector.ErrorDictionary.GetCorrectWord(input.ToLower());
                if (correct != null)
                {
                    Console.WriteLine($"Правильный вариант: {correct}");
                }
                else
                {
                    Console.WriteLine("Слово не найдено в словаре ошибок!");
                }
            }
        }

        static void EditDictionaryMenu()
        {
            Console.WriteLine("=== РЕДАКТИРОВАНИЕ СЛОВАРЯ ===\n");
            Console.WriteLine("1. Добавить правильное слово с ошибками");
            Console.WriteLine("2. Добавить ошибочный вариант к существующему слову");
            Console.WriteLine("3. Вернуться");
            Console.Write("\nВаш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Правильное слово: ");
                    string correct = Console.ReadLine();
                    Console.Write("Ошибочные варианты (через запятую): ");
                    string errorsInput = Console.ReadLine();
                    string[] errors = errorsInput.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var errorList = new System.Collections.Generic.List<string>();
                    foreach (string err in errors)
                    {
                        errorList.Add(err.Trim());
                    }
                    _corrector.ErrorDictionary.AddErrorWord(correct, errorList);
                    Console.WriteLine($"Слово '{correct}' добавлено!");
                    break;

                case "2":
                    Console.Write("Правильное слово: ");
                    string word = Console.ReadLine();
                    Console.Write("Новый ошибочный вариант: ");
                    string error = Console.ReadLine();
                    _corrector.ErrorDictionary.AddErrorVariant(word, error);
                    Console.WriteLine($"Вариант '{error}' добавлен к слову '{word}'!");
                    break;

                case "3":
                    break;

                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }

        static void TestCorrectionMenu()
        {
            Console.WriteLine("=== ТЕСТОВОЕ ИСПРАВЛЕНИЕ ТЕКСТА ===\n");
            Console.WriteLine("Введите текст для исправления (Enter - закончить ввод):");

            string input = "";
            string line;
            while ((line = Console.ReadLine()) != "")
            {
                input += line + "\n";
            }

            if (!string.IsNullOrEmpty(input))
            {
                Console.WriteLine("\n=== ИСХОДНЫЙ ТЕКСТ ===");
                Console.WriteLine(input);

                string corrected = _corrector.CorrectText(input);

                Console.WriteLine("\n=== ИСПРАВЛЕННЫЙ ТЕКСТ ===");
                Console.WriteLine(corrected);

                Console.WriteLine("\n=== РАЗНИЦА ===");
                if (input != corrected)
                {
                    Console.WriteLine("Текст был изменён!");
                    Console.WriteLine($"Длина исходного: {input.Length} символов");
                    Console.WriteLine($"Длина исправленного: {corrected.Length} символов");
                }
                else
                {
                    Console.WriteLine("Текст не изменился.");
                }
            }
            else
            {
                Console.WriteLine("Текст не был введён.");
            }
        }
    }
}
