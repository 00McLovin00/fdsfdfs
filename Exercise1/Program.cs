using System;

namespace Exercise1
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Задания 1-2";

            bool exit = false;
            while (!exit)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Task1();
                        break;
                    case "2":
                        Task2();
                        break;
                    case "3":
                        exit = true;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Попробуйте снова.");
                        Console.ReadKey();
                        break;
                }

                if (!exit)
                {
                    Console.Clear();
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Задание 1 - Возведение в степень");
            Console.WriteLine("2. Задание 2 - Перестановка цифр в числе");
            Console.WriteLine("3. Выйти");
            Console.Write("\nВыберите задание: ");
        }

        // ===== ЗАДАНИЕ 1 =====
        static void Task1()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАДАНИЕ 1: ВОЗВЕДЕНИЕ В СТЕПЕНЬ ===\n");

            Console.Write("Введите число A: ");
            int a = Convert.ToInt32(Console.ReadLine());

            if (a < 0)
            {
                Console.WriteLine("Число " + a + " не является натуральным!");
                WaitForKey();
                return;
            }

            Console.Write("Введите число N: ");
            int n = Convert.ToInt32(Console.ReadLine());

            if (n < 0)
            {
                Console.WriteLine("Число " + n + " не является натуральным!");
                WaitForKey();
                return;
            }

            int result = 1;
            for (int i = 0; i < n; i++)
            {
                result = result * a;
            }

            Console.WriteLine(a + "^" + n + " = " + result);
            WaitForKey();
        }

        // ===== ЗАДАНИЕ 2 =====
        static void Task2()
        {
            Console.Clear();
            Console.WriteLine("=== ЗАДАНИЕ 2: ПЕРЕСТАНОВКА ЦИФР В ЧИСЛЕ ===\n");

            Console.Write("Введите число x, помните, число должно быть больше или равно 100: ");
            int x = Convert.ToInt32(Console.ReadLine());

            if (x < 100)
            {
                Console.WriteLine("Число x не подходит под условия задачи!");
                WaitForKey();
                return;
            }

            string xToString = x.ToString();
            char[] xToMassive = xToString.ToCharArray();

            char tempSecondNumber = xToMassive[1];
            char tempLastNumber = xToMassive[xToMassive.Length - 1];

            char[] N = (char[])xToMassive.Clone();

            N[1] = tempLastNumber;
            N[N.Length - 1] = tempSecondNumber;

            string nToString = new string(N);
            int n = Convert.ToInt32(nToString);

            Console.WriteLine("Найденное число N = " + n);
            WaitForKey();
        }


        static void WaitForKey()
        {
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }
}