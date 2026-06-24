using Exercise4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercise4
{
    public class FileIndexer
    {
        private Dictionary<string, List<TextFile>> _index = new Dictionary<string, List<TextFile>>();
        private string _indexedDirectory;

        // Индексация всех текстовых файлов в директории
        public void IndexDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Директория '{directoryPath}' не найдена!");

            _indexedDirectory = directoryPath;
            _index.Clear();

            string[] txtFiles = Directory.GetFiles(directoryPath, "*.txt", SearchOption.AllDirectories);

            foreach (string filePath in txtFiles)
            {
                try
                {
                    TextFile file = new TextFile();
                    file.LoadFromTextFile(filePath);

                    // Индексация каждого слова
                    var words = file.Content.Split(new char[] { ' ', '\n', '\r', '\t', '.', ',', '!', '?', ';', ':' },
                                                   StringSplitOptions.RemoveEmptyEntries);

                    foreach (var word in words)
                    {
                        string key = word.ToLower();
                        if (!_index.ContainsKey(key))
                            _index[key] = new List<TextFile>();

                        if (!_index[key].Any(f => f.Path == file.Path))
                            _index[key].Add(file);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось проиндексировать {filePath}: {ex.Message}");
                }
            }

            Console.WriteLine($"Проиндексировано {txtFiles.Length} файлов, {_index.Count} уникальных слов");
        }

        // Поиск файлов по ключевому слову
        public List<TextFile> SearchByKeyword(string keyword)
        {
            string key = keyword.ToLower();
            if (_index.ContainsKey(key))
                return _index[key];
            return new List<TextFile>();
        }

        // Поиск файлов по нескольким ключевым словам (AND)
        public List<TextFile> SearchByKeywords(string[] keywords)
        {
            if (keywords.Length == 0)
                return new List<TextFile>();

            var results = SearchByKeyword(keywords[0]);

            for (int i = 1; i < keywords.Length; i++)
            {
                var nextResults = SearchByKeyword(keywords[i]);
                results = results.Intersect(nextResults).ToList();
                if (results.Count == 0)
                    break;
            }

            return results;
        }

        // Получить статистику индекса
        public (int totalFiles, int uniqueWords, int totalOccurrences) GetIndexStats()
        {
            int totalFiles = _index.Values.SelectMany(f => f).Distinct().Count();
            int uniqueWords = _index.Count;
            int totalOccurrences = _index.Values.Sum(f => f.Count);
            return (totalFiles, uniqueWords, totalOccurrences);
        }

        // Показать наиболее частые слова
        public List<(string word, int count)> GetTopWords(int top = 10)
        {
            return _index
                .OrderByDescending(kvp => kvp.Value.Count)
                .Take(top)
                .Select(kvp => (kvp.Key, kvp.Value.Count))
                .ToList();
        }
    }
}