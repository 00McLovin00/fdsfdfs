using Exercise4;
using System;
using System.Collections.Generic;
using System.IO;

namespace Exercise4
{
    public class FileEditorEngine
    {
        private TextFile _currentFile;
        private FileHistory _history;
        private bool _isModified;

        public TextFile CurrentFile => _currentFile;
        public bool IsModified => _isModified;

        public FileEditorEngine()
        {
            _history = new FileHistory();
            _isModified = false;
        }

        // Открыть файл
        public void OpenFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"Файл '{filePath}' не найден!");

            _currentFile = new TextFile();
            _currentFile.LoadFromTextFile(filePath);
            _history.SaveState(new FileMemento(_currentFile.Content));
            _isModified = false;
        }

        // Создать новый файл
        public void NewFile(string name = "Новый файл")
        {
            _currentFile = new TextFile(name, "");
            _history = new FileHistory();
            _history.SaveState(new FileMemento(_currentFile.Content));
            _isModified = false;
        }

        // Изменить содержимое
        public void SetContent(string content)
        {
            if (_currentFile == null)
                throw new InvalidOperationException("Сначала создайте или откройте файл!");

            // Сохраняем состояние перед изменением
            if (_isModified)
                _history.SaveState(new FileMemento(_currentFile.Content));

            _currentFile.Content = content;
            _currentFile.ModifiedDate = DateTime.Now;
            _isModified = true;
        }

        // Добавить текст в конец
        public void AppendText(string text)
        {
            if (_currentFile == null)
                throw new InvalidOperationException("Сначала создайте или откройте файл!");

            _history.SaveState(new FileMemento(_currentFile.Content));
            _currentFile.Content += text;
            _currentFile.ModifiedDate = DateTime.Now;
            _isModified = true;
        }

        // Откатить изменения (Undo)
        public void Undo()
        {
            if (!_history.CanUndo)
                throw new InvalidOperationException("Нет изменений для отката!");

            FileMemento memento = _history.Undo();
            _currentFile.Content = memento.Content;
            _currentFile.ModifiedDate = DateTime.Now;
            _isModified = true;
            Console.WriteLine("Откат выполнен!");
        }

        // Повторить изменение (Redo)
        public void Redo()
        {
            if (!_history.CanRedo)
                throw new InvalidOperationException("Нет действий для повтора!");

            FileMemento memento = _history.Redo();
            _currentFile.Content = memento.Content;
            _currentFile.ModifiedDate = DateTime.Now;
            _isModified = true;
            Console.WriteLine("Повтор выполнен!");
        }

        // Сохранить файл
        public void SaveFile(string filePath = null)
        {
            if (_currentFile == null)
                throw new InvalidOperationException("Нет файла для сохранения!");

            string path = filePath ?? _currentFile.Path;
            if (string.IsNullOrEmpty(path))
                throw new InvalidOperationException("Укажите путь для сохранения!");

            _currentFile.SaveToTextFile(path);
            _isModified = false;
            Console.WriteLine($"Файл сохранён: {path}");
        }

        // Получить содержимое файла с номерами строк
        public string GetContentWithLineNumbers()
        {
            if (_currentFile == null)
                return "Нет открытого файла";

            string[] lines = _currentFile.Content.Split('\n');
            string result = "";
            for (int i = 0; i < lines.Length; i++)
            {
                result += $"{i + 1,3}: {lines[i]}";
                if (i < lines.Length - 1)
                    result += "\n";
            }
            return result;
        }

        // Показать информацию о файле
        public string GetFileInfo()
        {
            if (_currentFile == null)
                return "Нет открытого файла";

            int wordCount = _currentFile.Content.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return $@"
Информация о файле:
Имя: {_currentFile.Name}
Путь: {_currentFile.Path}
Размер: {_currentFile.Content.Length} символов
Слов: {wordCount}
Создан: {_currentFile.CreatedDate:dd.MM.yyyy HH:mm}
Изменён: {_currentFile.ModifiedDate:dd.MM.yyyy HH:mm}
Может откатить: {(_history.CanUndo ? "Да" : "Нет")}
Может повторить: {(_history.CanRedo ? "Да" : "Нет")}
";
        }
    }
}
