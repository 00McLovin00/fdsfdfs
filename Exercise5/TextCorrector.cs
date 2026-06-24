using Exercise5;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Exercise5
{
    public class TextCorrector
    {
        private ErrorDictionary _errorDictionary;
        private PhoneNumberFormatter _phoneFormatter;

        public TextCorrector()
        {
            _errorDictionary = new ErrorDictionary();
            _phoneFormatter = new PhoneNumberFormatter();
        }

        public ErrorDictionary ErrorDictionary => _errorDictionary;
        public PhoneNumberFormatter PhoneFormatter => _phoneFormatter;

        // Исправление одного текста
        public string CorrectText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string result = text;

            // 1. Исправление ошибочных слов
            result = CorrectWords(result);

            // 2. Форматирование номеров телефонов
            result = _phoneFormatter.FormatPhoneNumbers(result);

            return result;
        }

        // Исправление ошибочных слов в тексте
        private string CorrectWords(string text)
        {
            string[] words = text.Split(new char[] { ' ', '\n', '\r', '\t', '.', ',', '!', '?', ';', ':', '(', ')', '"', '\'' },
                                        StringSplitOptions.RemoveEmptyEntries);

            string result = text;

            foreach (string word in words)
            {
                string lowerWord = word.ToLower();
                string correctWord = _errorDictionary.GetCorrectWord(lowerWord);

                if (correctWord != null)
                {
                    // Сохраняем регистр исходного слова
                    string replacement = correctWord;
                    if (char.IsUpper(word[0]))
                    {
                        replacement = char.ToUpper(replacement[0]) + replacement.Substring(1);
                    }

                    // Заменяем слово в тексте (с учётом границ слова)
                    string pattern = @"\b" + Regex.Escape(word) + @"\b";
                    result = Regex.Replace(result, pattern, replacement);
                }
            }

            return result;
        }

        // Обработка одного файла
        public void ProcessFile(string filePath, bool saveBackup = true)
        {
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Файл {filePath} не найден!");
                    return;
                }

                try
                {
                    // Пробуем разные кодировки
                    string content = "";

                    try
                    {
                        // Сначала пробуем UTF-8
                        content = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
                    }
                    catch
                    {
                        // Если не получилось, пробуем ANSI (Windows-1251)
                        content = File.ReadAllText(filePath, System.Text.Encoding.Default);
                    }

                    // Проверяем, что файл не пустой
                    if (string.IsNullOrEmpty(content))
                    {
                        Console.WriteLine($"Файл {filePath} пустой!");
                        return;
                    }

                    Console.WriteLine($"Прочитано {content.Length} символов");

                    // Создание резервной копии
                    if (saveBackup)
                    {
                        string backupPath = filePath + ".bak";
                        File.Copy(filePath, backupPath, true);
                        Console.WriteLine($"Создана резервная копия: {backupPath}");
                    }

                    // Исправление текста
                    string correctedContent = CorrectText(content);

                    // Сохраняем с UTF-8
                    File.WriteAllText(filePath, correctedContent, System.Text.Encoding.UTF8);

                    Console.WriteLine($"Обработан файл: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке {filePath}: {ex.Message}");
                }
            }
        }

        // Обработка всех файлов в директории
        public void ProcessDirectory(string directoryPath, string pattern = "*.txt", bool saveBackup = true)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Директория {directoryPath} не найдена!");
                return;
            }

            string[] files = Directory.GetFiles(directoryPath, pattern, SearchOption.AllDirectories);
            Console.WriteLine($"Найдено файлов: {files.Length}");

            foreach (string file in files)
            {
                ProcessFile(file, saveBackup);
            }

            Console.WriteLine($"Обработка завершена. Обработано файлов: {files.Length}");
        }

        // Обработка файлов с отображением прогресса
        public void ProcessDirectoryWithProgress(string directoryPath, string pattern = "*.txt", bool saveBackup = true)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Директория {directoryPath} не найдена!");
                return;
            }

            string[] files = Directory.GetFiles(directoryPath, pattern, SearchOption.AllDirectories);
            Console.WriteLine($"Найдено файлов: {files.Length}\n");

            int processed = 0;
            int errors = 0;
            int phoneNumbersFound = 0;

            foreach (string file in files)
            {
                try
                {
                    processed++;
                    string content = File.ReadAllText(file);

                    // Проверяем наличие номеров телефонов
                    if (_phoneFormatter.ContainsPhoneNumber(content))
                    {
                        phoneNumbersFound++;
                    }

                    if (saveBackup)
                    {
                        string backupPath = file + ".bak";
                        File.Copy(file, backupPath, true);
                    }

                    string correctedContent = CorrectText(content);
                    File.WriteAllText(file, correctedContent);

                    Console.WriteLine($"[{processed}/{files.Length}] Обработан: {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    errors++;
                    Console.WriteLine($"[{processed}/{files.Length}] Ошибка в {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            Console.WriteLine($"\n=== ОТЧЁТ ===");
            Console.WriteLine($"Обработано файлов: {processed}");
            Console.WriteLine($"Ошибок: {errors}");
            Console.WriteLine($"Найдено номеров телефонов: {phoneNumbersFound}");
        }

        // Статистика по файлу
        public void ShowFileStats(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не найден!");
                return;
            }

            string content = File.ReadAllText(filePath);
            int wordCount = content.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int charCount = content.Length;
            int lineCount = content.Split('\n').Length;

            Console.WriteLine($"\n=== СТАТИСТИКА ФАЙЛА ===");
            Console.WriteLine($"Имя: {Path.GetFileName(filePath)}");
            Console.WriteLine($"Путь: {filePath}");
            Console.WriteLine($"Размер: {charCount} символов");
            Console.WriteLine($"Слов: {wordCount}");
            Console.WriteLine($"Строк: {lineCount}");
            Console.WriteLine($"Номеров телефонов: {_phoneFormatter.ContainsPhoneNumber(content)}");
        }
    }
}