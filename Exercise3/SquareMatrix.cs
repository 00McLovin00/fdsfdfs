using System;

namespace Exercise3
{

    /// Класс "Квадратная матрица" с перегрузкой операторов (на double)

    public class SquareMatrix : ICloneable, IComparable<SquareMatrix>
    {
        // ===== ПОЛЯ =====
        private double[,] _data;
        private int _size;

        // ===== СВОЙСТВА =====
        public int Size => _size;

        public double this[int i, int j]
        {
            get => _data[i, j];
            set => _data[i, j] = value;
        }

        // ===== КОНСТРУКТОРЫ =====


        /// Конструктор с заданным размером (все элементы = 0)

        public SquareMatrix(int size)
        {
            if (size <= 0)
                throw new MatrixException("Размер матрицы должен быть больше 0!");

            _size = size;
            _data = new double[size, size];
        }


        /// Конструктор с заданным массивом

        public SquareMatrix(double[,] data)
        {
            if (data.GetLength(0) != data.GetLength(1))
                throw new MatrixException("Матрица должна быть квадратной!");

            _size = data.GetLength(0);
            _data = (double[,])data.Clone();
        }


        /// Конструктор для целочисленного массива (удобство)

        public SquareMatrix(int[,] data)
        {
            if (data.GetLength(0) != data.GetLength(1))
                throw new MatrixException("Матрица должна быть квадратной!");

            _size = data.GetLength(0);
            _data = new double[_size, _size];

            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                    _data[i, j] = data[i, j];
        }


        /// Конструктор для случайной генерации (целые числа)

        public SquareMatrix(int size, int minValue = -10, int maxValue = 10)
            : this(size)
        {
            Random rand = new Random();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    _data[i, j] = rand.Next(minValue, maxValue + 1);
        }


        /// Конструктор для случайной генерации (дробные числа)

        public SquareMatrix(int size, double minValue, double maxValue)
            : this(size)
        {
            Random rand = new Random();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    _data[i, j] = minValue + (maxValue - minValue) * rand.NextDouble();
        }


        /// Конструктор копирования (для Прототипа)

        private SquareMatrix(SquareMatrix other)
        {
            _size = other._size;
            _data = (double[,])other._data.Clone();
        }

        // ===== ПАТТЕРН "ПРОТОТИП" =====

        public object Clone()
        {
            return new SquareMatrix(this);
        }

        public SquareMatrix DeepCopy()
        {
            return new SquareMatrix(this);
        }

        // ===== МЕТОДЫ =====


        /// Вычисление детерминанта (методом Гаусса)

        public double Determinant()
        {
            if (_size == 1)
                return _data[0, 0];

            if (_size == 2)
                return _data[0, 0] * _data[1, 1] - _data[0, 1] * _data[1, 0];

            double det = 0;
            for (int j = 0; j < _size; j++)
            {
                det += (j % 2 == 0 ? 1 : -1) * _data[0, j] * Minor(0, j).Determinant();
            }
            return det;
        }


        /// Получение минора (удаление строки и столбца)

        private SquareMatrix Minor(int row, int col)
        {
            int newSize = _size - 1;
            double[,] newData = new double[newSize, newSize];

            for (int i = 0, newI = 0; i < _size; i++)
            {
                if (i == row) continue;
                for (int j = 0, newJ = 0; j < _size; j++)
                {
                    if (j == col) continue;
                    newData[newI, newJ] = _data[i, j];
                    newJ++;
                }
                newI++;
            }

            return new SquareMatrix(newData);
        }


        /// Нахождение обратной матрицы

        public SquareMatrix Inverse()
        {
            double det = Determinant();
            if (Math.Abs(det) < 1e-10)
                throw new SingularMatrixException("Матрица вырожденная! Обратной матрицы не существует.");

            if (_size == 1)
                return new SquareMatrix(new double[,] { { 1.0 / _data[0, 0] } });

            // Матрица алгебраических дополнений
            double[,] adjugate = new double[_size, _size];
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    int sign = ((i + j) % 2 == 0) ? 1 : -1;
                    adjugate[j, i] = sign * Minor(i, j).Determinant(); // Транспонируем сразу
                }
            }

            // Делим каждый элемент на детерминант
            double[,] result = new double[_size, _size];
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                    result[i, j] = adjugate[i, j] / det;

            return new SquareMatrix(result);
        }


        /// Транспонирование матрицы

        public SquareMatrix Transpose()
        {
            double[,] result = new double[_size, _size];
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                    result[j, i] = _data[i, j];
            return new SquareMatrix(result);
        }

        // ===== ПЕРЕГРУЗКА ОПЕРАТОРОВ =====

        // Сложение матриц
        public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
        {
            if (a._size != b._size)
                throw new MatrixSizeException("Матрицы должны быть одинакового размера!");

            SquareMatrix result = new SquareMatrix(a._size);
            for (int i = 0; i < a._size; i++)
                for (int j = 0; j < a._size; j++)
                    result[i, j] = a[i, j] + b[i, j];

            return result;
        }

        // Вычитание матриц
        public static SquareMatrix operator -(SquareMatrix a, SquareMatrix b)
        {
            if (a._size != b._size)
                throw new MatrixSizeException("Матрицы должны быть одинакового размера!");

            SquareMatrix result = new SquareMatrix(a._size);
            for (int i = 0; i < a._size; i++)
                for (int j = 0; j < a._size; j++)
                    result[i, j] = a[i, j] - b[i, j];

            return result;
        }

        // Умножение матриц
        public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
        {
            if (a._size != b._size)
                throw new MatrixSizeException("Матрицы должны быть одинакового размера!");

            SquareMatrix result = new SquareMatrix(a._size);
            for (int i = 0; i < a._size; i++)
                for (int j = 0; j < a._size; j++)
                    for (int k = 0; k < a._size; k++)
                        result[i, j] += a[i, k] * b[k, j];

            return result;
        }

        // Умножение на число
        public static SquareMatrix operator *(SquareMatrix a, double scalar)
        {
            SquareMatrix result = new SquareMatrix(a._size);
            for (int i = 0; i < a._size; i++)
                for (int j = 0; j < a._size; j++)
                    result[i, j] = a[i, j] * scalar;

            return result;
        }

        public static SquareMatrix operator *(double scalar, SquareMatrix a)
        {
            return a * scalar;
        }

        // Деление на число
        public static SquareMatrix operator /(SquareMatrix a, double scalar)
        {
            if (Math.Abs(scalar) < 1e-10)
                throw new MatrixException("Деление на ноль!");

            SquareMatrix result = new SquareMatrix(a._size);
            for (int i = 0; i < a._size; i++)
                for (int j = 0; j < a._size; j++)
                    result[i, j] = a[i, j] / scalar;

            return result;
        }

        // Сравнение по детерминанту
        public static bool operator >(SquareMatrix a, SquareMatrix b)
        {
            return a.Determinant() > b.Determinant();
        }

        public static bool operator <(SquareMatrix a, SquareMatrix b)
        {
            return a.Determinant() < b.Determinant();
        }

        public static bool operator >=(SquareMatrix a, SquareMatrix b)
        {
            return a.Determinant() >= b.Determinant();
        }

        public static bool operator <=(SquareMatrix a, SquareMatrix b)
        {
            return a.Determinant() <= b.Determinant();
        }

        // Сравнение на равенство (поэлементно)
        public static bool operator ==(SquareMatrix a, SquareMatrix b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            if (a._size != b._size) return false;

            for (int i = 0; i < a._size; i++)
                for (int j = 0; j < a._size; j++)
                    if (Math.Abs(a[i, j] - b[i, j]) > 1e-10)
                        return false;

            return true;
        }

        public static bool operator !=(SquareMatrix a, SquareMatrix b)
        {
            return !(a == b);
        }

        // Операторы true/false (проверка на вырожденность)
        public static bool operator true(SquareMatrix a)
        {
            return Math.Abs(a.Determinant()) > 1e-10;
        }

        public static bool operator false(SquareMatrix a)
        {
            return Math.Abs(a.Determinant()) < 1e-10;
        }

        // Явное приведение к double (детерминант)
        public static explicit operator double(SquareMatrix a)
        {
            return a.Determinant();
        }

        // Неявное приведение от int (создание диагональной матрицы)
        public static implicit operator SquareMatrix(int scalar)
        {
            SquareMatrix result = new SquareMatrix(1);
            result[0, 0] = scalar;
            return result;
        }

        // ===== ПЕРЕОПРЕДЕЛЕНИЕ МЕТОДОВ =====

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    // Форматируем числа: если целое - без .0, иначе с 2 знаками
                    double value = _data[i, j];
                    if (Math.Abs(value - Math.Round(value)) < 1e-10)
                        result += $"{value,8:F0} ";
                    else
                        result += $"{value,8:F2} ";
                }
                result += "\n";
            }
            return result;
        }

        public int CompareTo(SquareMatrix other)
        {
            if (other == null) return 1;
            return Determinant().CompareTo(other.Determinant());
        }

        public override bool Equals(object obj)
        {
            if (obj is SquareMatrix other)
                return this == other;
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + _size.GetHashCode();
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                    hash = hash * 23 + _data[i, j].GetHashCode();
            return hash;
        }
    }
}