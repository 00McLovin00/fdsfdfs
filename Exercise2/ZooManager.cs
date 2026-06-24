using System;
using System.Collections.Generic;

namespace Exercise2
{
    /// Класс ZooManager (Singleton) для управления животными
    public class ZooManager
    {
        // ===== Singleton =====
        private static ZooManager _instance;

        public static ZooManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ZooManager();
                return _instance;
            }
        }

        private ZooManager() { }

        // ===== Хранение животных =====
        private List<Animal> _animals = new List<Animal>();

        // ===== Методы =====
        public void AddAnimal(Animal animal)
        {
            _animals.Add(animal);
            Console.WriteLine($" Животное '{animal.Name}' добавлено!");
        }

        public void ShowAllAnimals()
        {
            if (_animals.Count == 0)
            {
                Console.WriteLine(" В зоопарке пока нет животных.");
                return;
            }

            Console.WriteLine("\n=== Список всех животных ===");
            for (int i = 0; i < _animals.Count; i++)
            {
                Console.WriteLine($"[{i}] {_animals[i].GetInfo()}");
            }
            Console.WriteLine($"Всего: {_animals.Count} животных\n");
        }

        public void ShowAnimalByName(string name)
        {
            foreach (var animal in _animals)
            {
                if (animal.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\n=== Найдено животное: {name} ===");
                    Console.WriteLine(animal.GetInfo());
                    Console.WriteLine();
                    return;
                }
            }
            Console.WriteLine($" Животное '{name}' не найдено!");
        }

        public int GetAnimalsCount()
        {
            return _animals.Count;
        }
    }
}