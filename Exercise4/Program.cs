using Exercise4;
using System;
using System.Collections.Generic;
using System.IO;

namespace Exercise4
{
    class Program
    {
        private static FileEditorEngine _editor = new FileEditorEngine();
        private static FileSearchEngine _searchEngine = new FileSearchEngine();
        private static FileIndexer _indexer = new FileIndexer();

        static void Main()
        {
            Console.Title = "Текстовый редактор с индексацией";
            Console.WriteLine("=== ТЕКСТОВЫЙ РЕДАКТОР И ИНДЕКСАТОР ===\n");

            bool exit = false;
            while (!exit)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": EditorMenu(); break;
                        case "2": SearchMenu(); break;
                        case "3": IndexerMenu(); break;
                        case "4": exit = true; Console.WriteLine("До свидания!"); break;
                        default: Console.WriteLine("Неверный выбор!"); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Редактор файлов");
            Console.WriteLine("2. Поиск по ключевым словам");
            Console.WriteLine("3. Индексация файлов");
            Console.WriteLine("4. Выйти");
            Console.Write("\nВаш выбор: ");
        }

        static void EditorMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== РЕДАКТОР ФАЙЛОВ ===\n");
                Console.WriteLine($"Текущий файл: {(_editor.CurrentFile?.Name ?? "Не открыт")}");
                Console.WriteLine($"Изменён: {(_editor.IsModified ? "Да" : "Нет")}");
                Console.WriteLine();

                Console.WriteLine("1. Открыть файл");
                Console.WriteLine("2. Создать новый");
                Console.WriteLine("3. Показать содержимое");
                Console.WriteLine("4. Редактировать");
                Console.WriteLine("5. Добавить текст");
                Console.WriteLine("6. Откатить (Undo)");
                Console.WriteLine("7. Повторить (Redo)");
                Console.WriteLine("8. Сохранить");
                Console.WriteLine("9. Информация о файле");
                Console.WriteLine("10. Назад");
                Console.Write("\nВаш выбор: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Путь к файлу: ");
                            string path = Console.ReadLine();
                            if (File.Exists(path))
                            {
                                _editor.OpenFile(path);
                                Console.WriteLine($"Файл '{path}' открыт!");
                            }
                            else
                            {
                                Console.WriteLine("Файл не найден!");
                            }
                            break;

                        case "2":
                            Console.Write("Имя нового файла: ");
                            string name = Console.ReadLine();
                            _editor.NewFile(name);
                            Console.WriteLine($"Создан файл '{name}'");
                            break;

                        case "3":
                            if (_editor.CurrentFile != null)
                            {
                                Console.WriteLine("\n--- Содержимое файла ---");
                                Console.WriteLine(_editor.GetContentWithLineNumbers());
                                Console.WriteLine("--- Конец ---");
                            }
                            else
                            {
                                Console.WriteLine("Нет открытого файла!");
                            }
                            break;

                        case "4":
                            if (_editor.CurrentFile != null)
                            {
                                Console.WriteLine("Введите новое содержимое (Enter - сохранить, пустая строка - отмена):");
                                string content = "";
                                string line;
                                while ((line = Console.ReadLine()) != "")
                                {
                                    content += line + "\n";
                                }
                                if (!string.IsNullOrEmpty(content))
                                {
                                    _editor.SetContent(content.TrimEnd('\n'));
                                    Console.WriteLine("Содержимое обновлено!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Нет открытого файла!");
                            }
                            break;

                        case "5":
                            if (_editor.CurrentFile != null)
                            {
                                Console.Write("Введите текст для добавления: ");
                                string text = Console.ReadLine();
                                _editor.AppendText(text + "\n");
                                Console.WriteLine("Текст добавлен!");
                            }
                            else
                            {
                                Console.WriteLine("Нет открытого файла!");
                            }
                            break;

                        case "6":
                            _editor.Undo();
                            break;

                        case "7":
                            _editor.Redo();
                            break;

                        case "8":
                            if (_editor.CurrentFile != null)
                            {
                                Console.Write("Путь для сохранения (Enter - использовать текущий): ");
                                string savePath = Console.ReadLine();
                                _editor.SaveFile(string.IsNullOrEmpty(savePath) ? null : savePath);
                            }
                            else
                            {
                                Console.WriteLine("Нет файла для сохранения!");
                            }
                            break;

                        case "9":
                            Console.WriteLine(_editor.GetFileInfo());
                            break;

                        case "10":
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                if (!back)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void SearchMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== ПОИСК ПО КЛЮЧЕВЫМ СЛОВАМ ===\n");

                Console.WriteLine("1. Загрузить директорию");
                Console.WriteLine("2. Поиск по одному слову");
                Console.WriteLine("3. Поиск по нескольким словам (AND)");
                Console.WriteLine("4. Показать все файлы");
                Console.WriteLine("5. Статистика");
                Console.WriteLine("6. Назад");
                Console.Write("\nВаш выбор: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Путь к директории: ");
                            string dir = Console.ReadLine();
                            _searchEngine.LoadDirectory(dir);
                            break;

                        case "2":
                            Console.Write("Ключевое слово: ");
                            string keyword = Console.ReadLine();
                            var results1 = _searchEngine.SearchByKeyword(keyword);
                            Console.WriteLine($"\nНайдено файлов: {results1.Count}");
                            foreach (var file in results1)
                            {
                                Console.WriteLine($"  - {file.Name} ({file.Path})");
                            }
                            break;

                        case "3":
                            Console.Write("Ключевые слова (через запятую): ");
                            string keywordsInput = Console.ReadLine();
                            string[] keywords = keywordsInput.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < keywords.Length; i++)
                                keywords[i] = keywords[i].Trim();
                            var results2 = _searchEngine.SearchByKeywords(keywords);
                            Console.WriteLine($"\nНайдено файлов: {results2.Count}");
                            foreach (var file in results2)
                            {
                                Console.WriteLine($"  - {file.Name} ({file.Path})");
                            }
                            break;

                        case "4":
                            var allFiles = _searchEngine.GetAllFiles();
                            Console.WriteLine($"\nВсего файлов: {allFiles.Count}");
                            foreach (var file in allFiles)
                            {
                                Console.WriteLine($"  - {file}");
                            }
                            break;

                        case "5":
                            var stats = _searchEngine.GetStatistics();
                            Console.WriteLine($"\nСтатистика:");
                            Console.WriteLine($"  Всего файлов: {stats.totalFiles}");
                            Console.WriteLine($"  Всего слов: {stats.totalWords}");
                            Console.WriteLine($"  Всего символов: {stats.totalCharacters}");
                            break;

                        case "6":
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                if (!back)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        static void IndexerMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== ИНДЕКСАЦИЯ ФАЙЛОВ ===\n");

                Console.WriteLine("1. Индексировать директорию");
                Console.WriteLine("2. Поиск по слову в индексе");
                Console.WriteLine("3. Поиск по нескольким словам");
                Console.WriteLine("4. Статистика индекса");
                Console.WriteLine("5. Топ слов");
                Console.WriteLine("6. Назад");
                Console.Write("\nВаш выбор: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Путь к директории: ");
                            string dir = Console.ReadLine();
                            _indexer.IndexDirectory(dir);
                            break;

                        case "2":
                            Console.Write("Ключевое слово: ");
                            string keyword = Console.ReadLine();
                            var results1 = _indexer.SearchByKeyword(keyword);
                            Console.WriteLine($"\nНайдено файлов: {results1.Count}");
                            foreach (var file in results1)
                            {
                                Console.WriteLine($"  - {file.Name} ({file.Path})");
                            }
                            break;

                        case "3":
                            Console.Write("Ключевые слова (через запятую): ");
                            string keywordsInput = Console.ReadLine();
                            string[] keywords = keywordsInput.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < keywords.Length; i++)
                                keywords[i] = keywords[i].Trim();
                            var results2 = _indexer.SearchByKeywords(keywords);
                            Console.WriteLine($"\nНайдено файлов: {results2.Count}");
                            foreach (var file in results2)
                            {
                                Console.WriteLine($"  - {file.Name} ({file.Path})");
                            }
                            break;

                        case "4":
                            var stats = _indexer.GetIndexStats();
                            Console.WriteLine($"\nСтатистика индекса:");
                            Console.WriteLine($"  Всего файлов: {stats.totalFiles}");
                            Console.WriteLine($"  Уникальных слов: {stats.uniqueWords}");
                            Console.WriteLine($"  Всего вхождений: {stats.totalOccurrences}");
                            break;

                        case "5":
                            var topWords = _indexer.GetTopWords(10);
                            Console.WriteLine("\nТоп-10 наиболее частых слов:");
                            int rank = 1;
                            foreach (var (word, count) in topWords)
                            {
                                Console.WriteLine($"  {rank,2}. {word,-15} - {count} вхождений");
                                rank++;
                            }
                            break;

                        case "6":
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                if (!back)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }
    }
}