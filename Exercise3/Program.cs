using System;

namespace Exercise3
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== МАТРИЧНЫЙ КАЛЬКУЛЯТОР ===\n");

            bool exit = false;
            SquareMatrix matrixA = null;
            SquareMatrix matrixB = null;

            while (!exit)
            {
                Console.WriteLine("\n=== МЕНЮ ===");
                Console.WriteLine("1. Создать матрицу A (случайно, целые)");
                Console.WriteLine("2. Создать матрицу B (случайно, целые)");
                Console.WriteLine("3. Показать матрицы");
                Console.WriteLine("4. A + B");
                Console.WriteLine("5. A - B");
                Console.WriteLine("6. A * B");
                Console.WriteLine("7. Детерминант A");
                Console.WriteLine("8. Обратная матрица A");
                Console.WriteLine("9. Сравнить матрицы (>, <, ==)");
                Console.WriteLine("10. Клонировать матрицу A");
                Console.WriteLine("11. Выйти");
                Console.Write("\nВаш выбор: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Размер матрицы A: ");
                            int sizeA = int.Parse(Console.ReadLine());
                            matrixA = new SquareMatrix(sizeA, -10, 10);
                            Console.WriteLine("Матрица A создана!");
                            break;

                        case "2":
                            Console.Write("Размер матрицы B: ");
                            int sizeB = int.Parse(Console.ReadLine());
                            matrixB = new SquareMatrix(sizeB, -10, 10);
                            Console.WriteLine("Матрица B создана!");
                            break;

                        case "3":
                            Console.WriteLine("\nМатрица A:");
                            Console.WriteLine(matrixA?.ToString() ?? "Не создана");
                            Console.WriteLine("\nМатрица B:");
                            Console.WriteLine(matrixB?.ToString() ?? "Не создана");
                            break;

                        case "4":
                            if (matrixA == null || matrixB == null)
                                throw new MatrixException("Сначала создайте обе матрицы!");
                            Console.WriteLine("\nA + B =");
                            Console.WriteLine((matrixA + matrixB).ToString());
                            break;

                        case "5":
                            if (matrixA == null || matrixB == null)
                                throw new MatrixException("Сначала создайте обе матрицы!");
                            Console.WriteLine("\nA - B =");
                            Console.WriteLine((matrixA - matrixB).ToString());
                            break;

                        case "6":
                            if (matrixA == null || matrixB == null)
                                throw new MatrixException("Сначала создайте обе матрицы!");
                            Console.WriteLine("\nA * B =");
                            Console.WriteLine((matrixA * matrixB).ToString());
                            break;

                        case "7":
                            if (matrixA == null)
                                throw new MatrixException("Сначала создайте матрицу A!");
                            Console.WriteLine($"\nДетерминант A = {matrixA.Determinant():F2}");
                            break;

                        case "8":
                            if (matrixA == null)
                                throw new MatrixException("Сначала создайте матрицу A!");
                            Console.WriteLine("\nОбратная матрица A:");
                            Console.WriteLine(matrixA.Inverse().ToString());
                            break;

                        case "9":
                            if (matrixA == null || matrixB == null)
                                throw new MatrixException("Сначала создайте обе матрицы!");
                            Console.WriteLine($"\nA > B: {matrixA > matrixB}");
                            Console.WriteLine($"A < B: {matrixA < matrixB}");
                            Console.WriteLine($"A == B: {matrixA == matrixB}");
                            Console.WriteLine($"Детерминант A: {matrixA.Determinant():F2}");
                            Console.WriteLine($"Детерминант B: {matrixB.Determinant():F2}");
                            break;

                        case "10":
                            if (matrixA == null)
                                throw new MatrixException("Сначала создайте матрицу A!");
                            SquareMatrix clone = (SquareMatrix)matrixA.Clone();
                            Console.WriteLine("\nКлон матрицы A:");
                            Console.WriteLine(clone.ToString());
                            Console.WriteLine($"Детерминант клона: {clone.Determinant():F2}");
                            break;

                        case "11":
                            exit = true;
                            Console.WriteLine("До свидания!");
                            break;

                        default:
                            Console.WriteLine("Неверный выбор!");
                            break;
                    }
                }
                catch (MatrixException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: введите корректное число!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
                }
            }
        }
    }
}