using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calki
{
    class Program
    {
        static void Main(string[] args)
        {
            double wynik = ProstokatyLewe(0, 10, 10);
            Console.WriteLine($"Prostokat lewy wynik = {wynik}");

            wynik = ProstokatyLewe(0, 10, 1000);
            Console.WriteLine($"Prostokat lewy wynik = {wynik}");
            Console.WriteLine();

            wynik = Trapez(0, 10, 10);
            Console.WriteLine($"Trapez wynik = {wynik}");

            wynik = Trapez(0, 10, 1000);
            Console.WriteLine($"Trapez wynik = {wynik}");
            Console.WriteLine();

            wynik = Simson(0, 10, 10);
            Console.WriteLine($"Simson wynik = {wynik}");

            wynik = Simson(0, 10, 1000);
            Console.WriteLine($"Simson wynik = {wynik}");


            wynik = prostokat2(0, 10, 1000);
            Console.WriteLine($"prostokat2 wynik = {wynik}");

            wynik = trapezow2(0, 10, 1000);
            Console.WriteLine($"trapezow2 wynik = {wynik}");

            wynik = simpsona2(0, 10, 1000);
            Console.WriteLine($"simpsona2 wynik = {wynik}");
            Console.WriteLine();
            Console.WriteLine();

            Mother mother = ReadTableFromFile();

            WriteMother(mother);

            double[] wynikGaus = GaussElimination(mother);

            int i = 1;
            foreach (double x in wynikGaus)
            {
                Console.WriteLine($"{i} = {x}");
                i++;
            }

            double[] wynikGausJordan = GaussJordanElimination(mother);
            i = 1;
            foreach (double x in wynikGaus)
            {
                Console.WriteLine($"{i} = {x}");
                i++;
            }

            Console.ReadKey();
        }

        #region calki

        public static double f(double x)
        {
            return x * x * x;
        }

        public static double ProstokatyLewe(double a, double b, double n)
        {
            double h = (b - a) / n;
            double obliczane = a;
            double wynik = 0;

            for (int i = 0; i < n; i++)
            {
                wynik += f(obliczane) * h;
                obliczane += h;
            }
            
            return wynik;
        }

        public static double Trapez(double a, double b, double n)
        {
            double h = (b - a) / n;
            double obliczane = a;
            double bokA = f(obliczane);
            double bokB = 0;
            double wynik = 0;

            for (int i = 0; i < n; i++)
            {
                obliczane += h;
                bokB = f(obliczane);
                wynik += (bokA + bokB) / 2 * h;

                bokA = bokB;
            }

            return wynik;
        }

        public static double Simson(double a, double b, double n)
        {
            double h = (b - a) / n ;
            double srodekH = h / 6;
            double y = a;
            double bokA = f(y);
            double srodek = 0;
            double bokB = 0;
            double wynik = 0;

            for (int i = 0; i < n; i++)
            {
                double x = y;
                y += h;
                srodek = f((x + y) / 2);

                bokB = f(y);
                wynik += srodekH * (bokA + 4* srodek + bokB);

                bokA = bokB;
            }

            return wynik;
        }

        static double prostokat2(double min, double max, int n)
        {
            double h = (max - min) / n;
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                double pole = h * f(min + i * h);
                sum += pole;
            }
            return sum;
        }

        static double trapezow2(double min, double max, int n)
        {
            double suma, h;
            suma = 0;
            h = (max - min) / n;
            for (int i = 0; i < n; i++)
            {
                double p = h / 2 * (f(min + i * h) + f(min + (i + 1) * h));
                suma += p;
            }
            return suma;
        }

        static double simpsona2(double min, double max, int n)
        {
            double h = (max - min) / n;
            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                double pole = (h / 6) * (f(min) + f(max) + 4 * f((max - min) / 2));
                sum += pole;
            }
            return sum;
        }

        #endregion

        static Mother ReadTableFromFile()
        {
            Mother mother = new Mother();

            string file = AppDomain.CurrentDomain.BaseDirectory + "macierz.txt";

            List<string> list = new List<string>();

            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                    list.Add(line);
            }

            mother.iloscNiewiadomych = Convert.ToInt32(list[0]);
            mother.macierz = new double[mother.iloscNiewiadomych, mother.iloscNiewiadomych];
            mother.wyniki = new double[mother.iloscNiewiadomych];

            for (int i = 0; i < mother.iloscNiewiadomych; i++)
            {
                string[] number = list[i + 1].Split(' ');

                if(number.Count() != mother.iloscNiewiadomych + 1)
                    continue;

                for (int j = 0; j < mother.iloscNiewiadomych; j++)
                    mother.macierz[i,j] = Convert.ToDouble(number[j]);

                mother.wyniki[i] = Convert.ToDouble(number[mother.iloscNiewiadomych]);
            }

            return mother;
        }

        static void WriteMother(Mother mother)
        {
            for (int i = 0; i < mother.iloscNiewiadomych; i++)
            {
                for (int j = 0; j < mother.iloscNiewiadomych; j++)
                {
                    Console.Write(mother.macierz[i, j]);
                    Console.Write("\t");
                }

                Console.WriteLine(mother.wyniki[i]);
                Console.Write("\n");
            }
        }

        public static double[] GaussElimination(Mother mother)
        {
            Console.WriteLine("GaussElimination");

            double[] x = new double[mother.iloscNiewiadomych];

            double[,] tmpA = new double[mother.iloscNiewiadomych, mother.iloscNiewiadomych + 1];

            for (int i = 0; i < mother.iloscNiewiadomych; i++)
            {
                for (int j = 0; j < mother.iloscNiewiadomych; j++)
                {
                    tmpA[i, j] = mother.macierz[i, j];
                }
                tmpA[i, mother.iloscNiewiadomych] = mother.wyniki[i];
            }

            double tmp = 0;

            WriteTableDouble(tmpA, mother.iloscNiewiadomych);

            for (int k = 0; k < mother.iloscNiewiadomych - 1; k++)
            {
                for (int i = k + 1; i < mother.iloscNiewiadomych; i++)
                {
                    tmp = tmpA[i, k] / tmpA[k, k];
                    for (int j = k; j < mother.iloscNiewiadomych + 1; j++)
                    {
                        tmpA[i, j] -= tmp * tmpA[k, j];
                    }

                    WriteTableDouble(tmpA, mother.iloscNiewiadomych);
                }
            }

            WriteTableDouble(tmpA, mother.iloscNiewiadomych);

            for (int k = mother.iloscNiewiadomych - 1; k >= 0; k--)
            {
                tmp = 0;
                for (int j = k + 1; j < mother.iloscNiewiadomych; j++)
                {
                    tmp += tmpA[k, j] * x[j];
                }
                x[k] = (tmpA[k, mother.iloscNiewiadomych] - tmp) / tmpA[k, k];
            }

            return x;
        }

        public static double[] GaussJordanElimination(Mother mother)
        {
            Console.WriteLine("GaussElimination");

            double[] x = new double[mother.iloscNiewiadomych];
            double[,] tmpA = new double[mother.iloscNiewiadomych, mother.iloscNiewiadomych + 1];
            double tmp = 0;

            for (int i = 0; i < mother.iloscNiewiadomych; i++)
            {
                for (int j = 0; j < mother.iloscNiewiadomych; j++)
                {
                    tmpA[i, j] = mother.macierz[i, j];
                }
                tmpA[i, mother.iloscNiewiadomych] = mother.wyniki[i];
            }

            WriteTableDouble(tmpA, mother.iloscNiewiadomych);

            for (int k = 0; k < mother.iloscNiewiadomych; k++)
            {
                tmp = tmpA[k, k];
                for (int i = 0; i < mother.iloscNiewiadomych + 1; i++)
                {
                    tmpA[k, i] = tmpA[k, i] / tmp;
                }

                for (int i = 0; i < mother.iloscNiewiadomych; i++)
                {
                    if (i != k)
                    {
                        tmp = tmpA[i, k] / tmpA[k, k];
                        for (int j = k; j < mother.iloscNiewiadomych + 1; j++)
                        {
                            tmpA[i, j] -= tmp * tmpA[k, j];
                        }
                    }
                }
            }

            WriteTableDouble(tmpA, mother.iloscNiewiadomych);

            for (int i = 0; i < mother.iloscNiewiadomych; i++)
            {
                x[i] = tmpA[i, mother.iloscNiewiadomych];
            }

            return x;
        }

        static void WriteTableDouble(double[,] table, int iloscNiewiadomych)
        {
            for (int i = 0; i < iloscNiewiadomych; i++)
            {
                for (int j = 0; j <= iloscNiewiadomych; j++)
                {
                    Console.Write($"{table[i,j]}");
                    Console.Write("\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public class Mother
    {
        public double[,] macierz;
        public double[] wyniki;
        public int iloscNiewiadomych;
    }
}
