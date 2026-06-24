using System;
using System.Collections.Generic;

namespace Exercise4
{
    // Хранитель состояния (Memento)
    public class FileMemento
    {
        public string Content { get; private set; }
        public DateTime Timestamp { get; private set; }

        public FileMemento(string content)
        {
            Content = content;
            Timestamp = DateTime.Now;
        }
    }

    // История изменений (Caretaker)
    public class FileHistory
    {
        private Stack<FileMemento> _history = new Stack<FileMemento>();
        private Stack<FileMemento> _redoStack = new Stack<FileMemento>();
        private int _maxHistorySize = 50;

        // Сохранить состояние
        public void SaveState(FileMemento memento)
        {
            _history.Push(memento);
            _redoStack.Clear();

            // Ограничиваем размер истории
            if (_history.Count > _maxHistorySize)
            {
                var temp = new Stack<FileMemento>();
                for (int i = 0; i < _maxHistorySize / 2; i++)
                    temp.Push(_history.Pop());
                _history = temp;
            }
        }

        // Откат (Undo)
        public FileMemento Undo()
        {
            if (_history.Count <= 1)
                throw new InvalidOperationException("Нет изменений для отката!");

            FileMemento current = _history.Pop();
            _redoStack.Push(current);
            return _history.Peek();
        }

        // Повтор (Redo)
        public FileMemento Redo()
        {
            if (_redoStack.Count == 0)
                throw new InvalidOperationException("Нет действий для повтора!");

            FileMemento memento = _redoStack.Pop();
            _history.Push(memento);
            return memento;
        }

        // Можно ли откатить?
        public bool CanUndo => _history.Count > 1;

        // Можно ли повторить?
        public bool CanRedo => _redoStack.Count > 0;

        // Получить текущее состояние
        public FileMemento GetCurrentState()
        {
            return _history.Peek();
        }
    }
}