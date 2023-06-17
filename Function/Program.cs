using System;
using System.Collections.Generic;
using System.IO;

namespace DoubleBinary
{
    class Program
    {
        public static double QuadraticFunction(double x, double a)
        {
            return a * x * x;
        }

        public static double SinusFunction(double x, double a)
        {
            return a * Math.Sin(x);
        }

        public static void SaveFunc(string fileName, double a, double b, double h, Func<double, double, double> function)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            double x = a;

            while (x <= b)
            {
                double result = function(x, a);
                bw.Write(result);
                x += h;
            }

            bw.Close();
            fs.Close();
        }

        public static double Load(string fileName, out double[] values)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            List<double> valueList = new List<double>();

            double min = double.MaxValue;
            double d;

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                d = br.ReadDouble();
                valueList.Add(d);

                if (d < min)
                    min = d;
            }

            values = valueList.ToArray();

            br.Close();
            fs.Close();

            return min;
        }

        static void Main(string[] args)
        {
            Func<double, double, double>[] functions = { QuadraticFunction, SinusFunction };
            string[] functionNames = { "Квадратичная функция", "Синусоидальная функция" };
            int functionChoice;
            double a, b, h;

            Console.WriteLine("Меню функций:");

            for (int i = 0; i < functions.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {functionNames[i]}");
            }

            Console.Write("Выберите функцию (1-" + functions.Length + "): ");
            while (!int.TryParse(Console.ReadLine(), out functionChoice) || functionChoice < 1 || functionChoice > functions.Length)
            {
                Console.WriteLine("Неверный ввод. Выберите функцию (1-" + functions.Length + "): ");
            }

            Console.Write("Введите значение 'a': ");
            while (!double.TryParse(Console.ReadLine(), out a))
            {
                Console.WriteLine("Неверный ввод. Введите значение 'a': ");
            }

            Console.Write("Введите значение 'b': ");
            while (!double.TryParse(Console.ReadLine(), out b))
            {
                Console.WriteLine("Неверный ввод. Введите значение 'b': ");
            }

            Console.Write("Введите значение 'h': ");
            while (!double.TryParse(Console.ReadLine(), out h))
            {
                Console.WriteLine("Неверный ввод. Введите значение 'h': ");
            }

            SaveFunc("data_function.bin", a, b, h, functions[functionChoice - 1]);

            double[] loadedValues;
            double minValue = Load("data_function.bin", out loadedValues);
            Console.WriteLine("Минимальное значение (" + functionNames[functionChoice - 1] + "): " + minValue);

            Console.ReadKey();
        }
    }
}