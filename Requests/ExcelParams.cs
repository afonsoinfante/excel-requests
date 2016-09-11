using System;
using System.Collections.Generic;


namespace Requests
{
    class ExcelParams
    {
        public static Dictionary<string, T> AsDictionary<T>(object[,] values)
        {
            var dict = new Dictionary<string, T>();

            int rows = values.GetLength(0);
            int columns = values.GetLength(1);

            if (columns != 2)
                throw new Exception("Expecting two columns");

            for (int i = 0; i < rows; i++)
                dict.Add(values[i, 0].ToString(), (T)values[i, 1]);

            return dict;
        }
    }
}
