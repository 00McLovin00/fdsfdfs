using System;
using System.Collections.Generic;

namespace Exercise5
{
    public class ErrorDictionary
    {
        private Dictionary<string, List<string>> _errorWords;

        public ErrorDictionary()
        {
            _errorWords = new Dictionary<string, List<string>>();
            InitializeDefaultDictionary();
        }

        // Заполнение словаря по умолчанию
        private void InitializeDefaultDictionary()
        {
            // Формат: правильное слово -> список ошибочных вариантов
            AddErrorWord("привет", new List<string> { "првиет", "пирвет", "привеет", "превет" });
            AddErrorWord("здравствуйте", new List<string> { "здраствуйте", "здравствуйти", "здравствуйте" });
            AddErrorWord("пока", new List<string> { "покка", "пака", "поко" });
            AddErrorWord("спасибо", new List<string> { "спосибо", "спасибо", "спасиба" });
            AddErrorWord("пожалуйста", new List<string> { "пожалуста", "пажалуйста", "пожалуйсто" });
            AddErrorWord("извините", new List<string> { "извините", "извинити", "извените" });
            AddErrorWord("здорово", new List<string> { "здорова", "здарова", "здорово" });
            AddErrorWord("интернет", new List<string> { "инетрнет", "интернет", "интернед" });
            AddErrorWord("компьютер", new List<string> { "кампютер", "компютер", "компьютир" });
            AddErrorWord("программа", new List<string> { "праграмма", "програма", "программм" });
        }

        // Добавление слова в словарь
        public void AddErrorWord(string correctWord, List<string> errorVariants)
        {
            if (!_errorWords.ContainsKey(correctWord))
            {
                _errorWords[correctWord] = new List<string>();
            }

            foreach (var variant in errorVariants)
            {
                if (!_errorWords[correctWord].Contains(variant))
                {
                    _errorWords[correctWord].Add(variant);
                }
            }
        }

        // Добавление одного ошибочного варианта
        public void AddErrorVariant(string correctWord, string errorVariant)
        {
            if (!_errorWords.ContainsKey(correctWord))
            {
                _errorWords[correctWord] = new List<string>();
            }

            if (!_errorWords[correctWord].Contains(errorVariant))
            {
                _errorWords[correctWord].Add(errorVariant);
            }
        }

        // Получить правильное слово по ошибочному
        public string GetCorrectWord(string errorWord)
        {
            foreach (var pair in _errorWords)
            {
                if (pair.Value.Contains(errorWord))
                {
                    return pair.Key;
                }
            }
            return null;
        }

        // Проверить, есть ли слово в словаре ошибок
        public bool IsErrorWord(string word)
        {
            foreach (var pair in _errorWords)
            {
                if (pair.Value.Contains(word))
                {
                    return true;
                }
            }
            return false;
        }

        // Получить все правильные слова
        public List<string> GetCorrectWords()
        {
            return new List<string>(_errorWords.Keys);
        }

        // Получить все ошибочные варианты для слова
        public List<string> GetErrorVariants(string correctWord)
        {
            if (_errorWords.ContainsKey(correctWord))
            {
                return new List<string>(_errorWords[correctWord]);
            }
            return new List<string>();
        }

        // Показать словарь
        public void PrintDictionary()
        {
            Console.WriteLine("\n=== СЛОВАРЬ ОШИБОЧНЫХ СЛОВ ===\n");
            foreach (var pair in _errorWords)
            {
                Console.WriteLine($"Правильно: {pair.Key}");
                Console.Write($"  Ошибки: ");
                Console.WriteLine(string.Join(", ", pair.Value));
                Console.WriteLine();
            }
        }

        // Количество слов в словаре
        public int Count => _errorWords.Count;

        // Загрузка из файла (дополнительно)
        public void LoadFromFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не найден!");
                return;
            }

            string[] lines = System.IO.File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                string[] parts = line.Split('|');
                if (parts.Length >= 2)
                {
                    string correct = parts[0].Trim();
                    string[] errors = parts[1].Split(',');

                    foreach (string error in errors)
                    {
                        AddErrorVariant(correct, error.Trim());
                    }
                }
            }
        }

        // Сохранение в файл (дополнительно)
        public void SaveToFile(string filePath)
        {
            List<string> lines = new List<string>();
            foreach (var pair in _errorWords)
            {
                lines.Add($"{pair.Key}|{string.Join(", ", pair.Value)}");
            }
            System.IO.File.WriteAllLines(filePath, lines);
        }
    }
}