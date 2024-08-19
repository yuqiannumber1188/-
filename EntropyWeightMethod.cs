using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cjpc.utils
{
    class EntropyWeightMethod
    {
        // 计算权重的方法
        public static List<double> CalculateWeights(double[,] data)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);
            double[,] normalizedData = new double[rows, cols];
            double[] entropy = new double[cols];
            double[] weight = new double[cols];
            List<double> weights = new List<double>();

            // 归一化处理
            for (int j = 0; j < cols; j++)
            {
                double min = double.MaxValue;
                double max = double.MinValue;
                for (int i = 0; i < rows; i++)
                {
                    if (data[i, j] < min) min = data[i, j];
                    if (data[i, j] > max) max = data[i, j];
                }
                for (int i = 0; i < rows; i++)
                {
                    normalizedData[i, j] = (data[i, j] - min) / (max - min);
                }
            }

            // 计算熵值
            for (int j = 0; j < cols; j++)
            {
                double sum = 0;
                double tempentropy = 0;

                for (int i = 0; i < rows; i++)
                {
                    sum += normalizedData[i, j];
                }

                for (int i = 0; i < rows; i++)
                {
                    double pij = normalizedData[i,j] / sum;
                    if (pij != 0)
                        tempentropy += pij * Math.Log(pij);

                }
                entropy[j] = -1 / Math.Log(rows) * tempentropy;
            }

            // 计算权重
            double totalEntropy = 0;
            for (int j = 0; j < cols; j++)
            {
                totalEntropy += (1 - entropy[j]);
            }
            for (int j = 0; j < cols; j++)
            {
                weight[j] = (1 - entropy[j]) / totalEntropy;
                weights.Add(weight[j]);
            }

            return weights;
        }
    }
}
