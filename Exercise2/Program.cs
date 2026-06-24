using System;

namespace Exercise2
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Добро пожаловать в зоопарк! ===\n");

            ZooManager zoo = ZooManager.Instance;

            // Добавим несколько животных для примера
            SeedData(zoo);

            bool exit = false;

            while (!exit)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddAnimalMenu(zoo);
                        break;
                    case "2":
                        zoo.ShowAllAnimals();
                        break;
                    case "3":
                        FindAnimalMenu(zoo);
                        break;
                    case "4":
                        exit = true;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Ошибка: неверный выбор! Попробуйте снова.\n");
                        break;
                }
            }

            Console.ReadKey();
        }

        // ===== МЕНЮ =====
        static void ShowMenu()
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Добавить животное");
            Console.WriteLine("2. Показать всех животных");
            Console.WriteLine("3. Найти животное по кличке");
            Console.WriteLine("4. Выйти");
            Console.Write("\nВаш выбор: ");
        }

        // ===== ДОБАВЛЕНИЕ ЖИВОТНОГО =====
        static void AddAnimalMenu(ZooManager zoo)
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ ЖИВОТНОГО ===");

            // Выбор типа
            Console.WriteLine("Выберите тип животного:");
            Console.WriteLine("1. Млекопитающее");
            Console.WriteLine("2. Птица");
            Console.WriteLine("3. Рыба");
            Console.WriteLine("4. Пресмыкающееся");
            Console.WriteLine("5. Земноводное");
            Console.Write("Ваш выбор: ");

            string typeChoice = Console.ReadLine();

            // Ввод общих данных
            Console.Write("Кличка: ");
            string name = Console.ReadLine();

            Console.Write("Возраст (целое число): ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Ошибка: введите корректное число!\n");
                return;
            }

            Console.Write("Среда обитания (лес, водоём, пустыня и т.д.): ");
            string habitat = Console.ReadLine();

            Console.Write("Тип питания (хищник, травоядное, всеядное): ");
            string diet = Console.ReadLine();

            Console.Write("Вес (целое число, кг): ");
            if (!int.TryParse(Console.ReadLine(), out int weight))
            {
                Console.WriteLine("Ошибка: введите корректное число!\n");
                return;
            }

            // Создание животного в зависимости от типа
            Animal newAnimal = null;

            switch (typeChoice)
            {
                case "1":
                    Console.Write("Есть ли шерсть? (да/нет): ");
                    bool hasFur = Console.ReadLine().ToLower() == "да";
                    newAnimal = new Mammal(name, age, habitat, diet, weight, hasFur);
                    break;

                case "2":
                    Console.Write("Размах крыльев (в метрах, например 2,5): ");
                    if (float.TryParse(Console.ReadLine(), out float wingSpan))
                    {
                        newAnimal = new Bird(name, age, habitat, diet, weight, wingSpan);
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: введите корректное число!\n");
                        return;
                    }
                    break;

                case "3":
                    Console.Write("Тип воды (пресная/морская): ");
                    string waterType = Console.ReadLine();
                    newAnimal = new Fish(name, age, habitat, diet, weight, waterType);
                    break;

                case "4":
                    Console.Write("Ядовитое? (да/нет): ");
                    bool isVenomous = Console.ReadLine().ToLower() == "да";
                    newAnimal = new Reptile(name, age, habitat, diet, weight, isVenomous);
                    break;

                case "5":
                    Console.Write("Влажность кожи (низкая/средняя/высокая): ");
                    string skinMoisture = Console.ReadLine();
                    newAnimal = new Amphibian(name, age, habitat, diet, weight, skinMoisture);
                    break;

                default:
                    Console.WriteLine("Ошибка: неверный тип животного!\n");
                    return;
            }

            // Добавляем животное
            if (newAnimal != null)
            {
                zoo.AddAnimal(newAnimal);
            }
        }

        // ===== ПОИСК ЖИВОТНОГО =====
        static void FindAnimalMenu(ZooManager zoo)
        {
            Console.Write("\nВведите кличку животного: ");
            string name = Console.ReadLine();
            zoo.ShowAnimalByName(name);
        }

        // ===== НАЧАЛЬНЫЕ ДАННЫЕ =====
        static void SeedData(ZooManager zoo)
        {
            zoo.AddAnimal(new Mammal("Барсик", 5, "лес", "хищник", 5, true));
            zoo.AddAnimal(new Bird("Орел", 3, "горы", "хищник", 4, 2.5f));
            zoo.AddAnimal(new Fish("Акула", 10, "океан", "хищник", 500, "морская"));
        }
    }
}