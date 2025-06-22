using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MatrixCalculator.Tests
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeMatrix()
        {
            var m = new Matrix(2, 2);
            Assert.AreEqual(2, m.Rows);
            Assert.AreEqual(2, m.Columns);
        }

        [TestMethod]
        public void ToString_ShouldReturnFormattedString()
        {
            var m = new Matrix(2, 2);
            m.Value[0, 0] = 1; m.Value[0, 1] = 2;
            m.Value[1, 0] = 3; m.Value[1, 1] = 4;
            string expected = "1\t2\t\n3\t4\t\n";
            Assert.AreEqual(expected, m.ToString());
        }

        [DataTestMethod]
        [DataRow(2, 2)]
        [DataRow(3, 3)]
        public void Addition_ValidMatrices_ShouldAddCorrectly(int rows, int cols)
        {
            var m1 = new Matrix(rows, cols);
            var m2 = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    m1.Value[i, j] = 1;
                    m2.Value[i, j] = 2;
                }
            var result = m1 + m2;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    Assert.AreEqual(3, result.Value[i, j]);
        }

        [TestMethod]
        [ExpectedException(typeof(MatrixSizeMismatchException))]
        public void Addition_DifferentSizes_ShouldThrowException()
        {
            var m1 = new Matrix(2, 2);
            var m2 = new Matrix(3, 3);
            var result = m1 + m2;
        }

        [DataTestMethod]
        [DataRow(2, 2)]
        [DataRow(3, 3)]
        public void Multiplication_ValidMatrices_ShouldMultiplyCorrectly(int rows, int cols)
        {
            var m1 = new Matrix(rows, cols);
            var m2 = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    m1.Value[i, j] = 2;
                    m2.Value[i, j] = 3;
                }
            var result = m1 * m2;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    Assert.AreEqual(6, result.Value[i, j]);
        }

        [TestMethod]
        [ExpectedException(typeof(MatrixSizeMismatchException))]
        public void Multiplication_DifferentSizes_ShouldThrowException()
        {
            var m1 = new Matrix(2, 2);
            var m2 = new Matrix(3, 3);
            var result = m1 * m2;
        }

        [TestMethod]
        public void CompareOperators_ShouldCompareBySum()
        {
            var m1 = new Matrix(2, 2);
            var m2 = new Matrix(2, 2);
            m1.Value[0, 0] = 1; m1.Value[0, 1] = 1;
            m1.Value[1, 0] = 1; m1.Value[1, 1] = 1;
            m2.Value[0, 0] = 2; m2.Value[0, 1] = 2;
            m2.Value[1, 0] = 2; m2.Value[1, 1] = 2;
            Assert.IsTrue(m1 < m2);
            Assert.IsTrue(m2 > m1);
            Assert.IsTrue(m1 <= m2);
            Assert.IsTrue(m2 >= m1);
            Assert.IsFalse(m1 == m2);
            Assert.IsTrue(m1 != m2);
        }

        [TestMethod]
        public void ExplicitConversionToDoubleArray_ShouldPreserveValues()
        {
            var m = new Matrix(2, 2);
            m.Value[0, 0] = 1; m.Value[0, 1] = 2;
            m.Value[1, 0] = 3; m.Value[1, 1] = 4;
            double[,] result = (double[,])m;
            Assert.AreEqual(1.0, result[0, 0]);
            Assert.AreEqual(2.0, result[0, 1]);
            Assert.AreEqual(3.0, result[1, 0]);
            Assert.AreEqual(4.0, result[1, 1]);
        }

        [TestMethod]
        public void TrueFalseOperators_ShouldWorkBasedOnSum()
        {
            var m1 = new Matrix(2, 2);
            var m2 = new Matrix(2, 2);
            m1.Value[0, 0] = 1; m1.Value[0, 1] = 1;
            m1.Value[1, 0] = 1; m1.Value[1, 1] = 1;
            m2.Value[0, 0] = -1; m2.Value[0, 1] = -1;
            m2.Value[1, 0] = -1; m2.Value[1, 1] = -1;
            Assert.IsTrue(Matrix.True(m1));
            Assert.IsFalse(Matrix.False(m1));
            Assert.IsFalse(Matrix.True(m2));
            Assert.IsTrue(Matrix.False(m2));
        }

        [TestMethod]
        public void Clone_ShouldCreateDeepCopy()
        {
            var m = new Matrix(2, 2);
            m.Value[0, 0] = 1; m.Value[0, 1] = 2;
            m.Value[1, 0] = 3; m.Value[1, 1] = 4;
            var clone = (Matrix)m.Clone();
            Assert.AreNotSame(m, clone);
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    Assert.AreEqual(m.Value[i, j], clone.Value[i, j]);
        }

        [TestMethod]
        public void Equals_GetHashCode_ShouldDependOnSum()
        {
            var m1 = new Matrix(2, 2);
            var m2 = new Matrix(2, 2);
            m1.Value[0, 0] = 1; m1.Value[0, 1] = 1;
            m1.Value[1, 0] = 1; m1.Value[1, 1] = 1;
            m2.Value[0, 0] = 2; m2.Value[0, 1] = 0;
            m2.Value[1, 0] = 1; m2.Value[1, 1] = 1;
            Assert.IsTrue(m1.Equals(m2));
            Assert.AreEqual(m1.GetHashCode(), m2.GetHashCode());
        }

        [TestMethod]
        public void Transpose_ShouldSwapRowsAndColumns()
        {
            var m = new Matrix(2, 2);
            m.Value[0, 0] = 1; m.Value[0, 1] = 2;
            m.Value[1, 0] = 3; m.Value[1, 1] = 4;
            var t = m.Transpose();
            Assert.AreEqual(1, m.Value[0, 0]); Assert.AreEqual(2, m.Value[0, 1]);
            Assert.AreEqual(3, m.Value[1, 0]); Assert.AreEqual(4, m.Value[1, 1]);
            Assert.AreEqual(1, t.Value[0, 0]); Assert.AreEqual(3, t.Value[0, 1]);
            Assert.AreEqual(2, t.Value[1, 0]); Assert.AreEqual(4, t.Value[1, 1]);
        }

        [TestMethod]
        public void Determinant_For2x2_ShouldCalculateCorrectly()
        {
            var m = new Matrix(2, 2);
            m.Value[0, 0] = 1; m.Value[0, 1] = 2;
            m.Value[1, 0] = 3; m.Value[1, 1] = 4;
            Assert.AreEqual(-2, m.Determinant());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Determinant_NonSquare_ShouldThrowException()
        {
            var m = new Matrix(2, 3);
            m.Determinant();
        }
    }
}
