using Exercise4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercise4
{
    public class FileSearchEngine
    {
        private List<TextFile> _files = new List<TextFile>();
        private string _currentDirectory;

        // Загрузить все текстовые файлы из директории
        public void LoadDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Директория '{directoryPath}' не найдена!");

            _currentDirectory = directoryPath;
            _files.Clear();

            string[] txtFiles = Directory.GetFiles(directoryPath, "*.txt");
            foreach (string filePath in txtFiles)
            {
                try
                {
                    TextFile file = new TextFile();
                    file.LoadFromTextFile(filePath);
                    _files.Add(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось загрузить {filePath}: {ex.Message}");
                }
            }

            Console.WriteLine($"Загружено {_files.Count} текстовых файлов из '{directoryPath}'");
        }

        // Поиск файлов по одному ключевому слову
        public List<TextFile> SearchByKeyword(string keyword)
        {
            return _files.Where(f => f.ContainsKeyword(keyword)).ToList();
        }

        // Поиск файлов по нескольким ключевым словам (AND)
        public List<TextFile> SearchByKeywords(string[] keywords)
        {
            return _files.Where(f => f.ContainsAllKeywords(keywords)).ToList();
        }

        // Поиск файлов по ключевому слову с учётом регистра
        public List<TextFile> SearchByKeywordCaseSensitive(string keyword)
        {
            return _files.Where(f => f.Content.Contains(keyword)).ToList();
        }

        // Получить все загруженные файлы
        public List<TextFile> GetAllFiles()
        {
            return _files;
        }

        // Получить статистику по файлам
        public (int totalFiles, int totalWords, int totalCharacters) GetStatistics()
        {
            int totalFiles = _files.Count;
            int totalWords = 0;
            int totalChars = 0;

            foreach (var file in _files)
            {
                var words = file.Content.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                totalWords += words.Length;
                totalChars += file.Content.Length;
            }

            return (totalFiles, totalWords, totalChars);
        }
    }
}