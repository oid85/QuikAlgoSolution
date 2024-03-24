using System.Collections.Generic;

namespace AlgoSolution.Indicators
{
    public static class IListExtension
    {
        /// <summary>
        /// Сдвиг вправо
        /// </summary>
        /// <param name="values"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static IList<double> Shift(this IList<double> values, int shift)
        {
            var result = new List<double>();

            result.Add(0.0);

            for (int i = 0; i < values.Count - 1; i++)
                result.Add(values[i]);

            return result;
        }
    }
}
