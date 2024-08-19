using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;


namespace cjpc.utils
{
    class HierarchicalAnalysis
    {
        // 计算权重向量
        static public double[] CalculateWeights(double[,] pairwiseMatrix)
        {
            int n = pairwiseMatrix.GetLength(0);
            double[] weights = new double[n];
            for (int i = 0; i < n; i++)
            {
                double product = 1;
                for (int j = 0; j < n; j++)
                {
                    product *= pairwiseMatrix[i, j];
                }
                weights[i] = Math.Pow(product, 1.0 / n);
            }

            // 归一化处理
            double sum = 0;
            foreach (var weight in weights)
            {
                sum += weight;
            }
            for (int i = 0; i < n; i++)
            {
                weights[i]= Math.Round(weights[i]/sum,3);
            }

            return weights;

        }

        // 计算最大特征值根
        static public double CalculateMaxEigenvalue(double[,] pairwiseMatrix, double[] weights)
        {
            int n = pairwiseMatrix.GetLength(0);
            double maxEigenvalue = 0;
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += pairwiseMatrix[i, j] * weights[j];
                }
                maxEigenvalue += sum;
            }
            return maxEigenvalue;
        }

        // 获取随机一致性指标
        static public double GetRandomIndex(int n)
        {
            // 根据提供的 RI 表，获取对应的 RI 值
            double[] riValues = {
            0, 0, 0.52, 0.89, 1.12, 1.26, 1.36, 1.41, 1.46, 1.49,
            1.52, 1.54, 1.56, 1.58, 1.59, 1.5943, 1.6064, 1.6133,
            1.6207, 1.6292, 1.6358, 1.6403, 1.6462, 1.6497, 1.6556,
            1.6587, 1.6631, 1.667, 1.6693, 1.6724
        };

            if (n < 1 || n > riValues.Length)
                throw new ArgumentOutOfRangeException("n", "n 应在 1 到 " + riValues.Length + " 之间。");

            return riValues[n];
        }
    }
}
