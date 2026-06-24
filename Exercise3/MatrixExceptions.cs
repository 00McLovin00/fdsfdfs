using System;

namespace Exercise3
{
    /// Базовое исключение для матричных операций

    public class MatrixException : Exception
    {
        public MatrixException() : base() { }
        public MatrixException(string message) : base(message) { }
        public MatrixException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// Исключение при несовместимых размерах матриц

    public class MatrixSizeException : MatrixException
    {
        public MatrixSizeException() : base("Неверный размер матрицы!") { }
        public MatrixSizeException(string message) : base(message) { }
    }


    /// Исключение при вырожденной матрице

    public class SingularMatrixException : MatrixException
    {
        public SingularMatrixException() : base("Матрица вырожденная! Обратной матрицы не существует.") { }
        public SingularMatrixException(string message) : base(message) { }
    }
}