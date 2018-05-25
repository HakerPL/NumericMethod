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
            Console.WriteLine();

            Mother motherGauss = ReadTableFromFile();
            Mother motherGauss2 = ReadTableFromFile();

            Mother motherGaussJordan = ReadTableFromFile();
            Mother motherGaussJordan2 = ReadTableFromFile();

            double[] wynikGauss = Gauss(motherGauss);
            double[] wynikGauss2 = GaussElimination(motherGauss2);


            Console.WriteLine($"wynikGauss");
            int i = 1;
            foreach (double x in wynikGauss)
            {
                Console.WriteLine($"{i} = {x}");
                i++;
            }

            Console.WriteLine();
            Console.WriteLine($"wynikGauss2");
            i = 1;
            foreach (double x in wynikGauss2)
            {
                Console.WriteLine($"{i} = {x}");
                i++;
            }

            Console.WriteLine();
            Console.WriteLine();

            double[] wynikGausJordan = GaussJordann(motherGaussJordan);
            double[] wynikGausJordan2 = GaussJordanElimination(motherGaussJordan2);

            Console.WriteLine();
            Console.WriteLine($"wynikGausJordan");
            i = 1;
            foreach (double x in wynikGausJordan)
            {
                Console.WriteLine($"{i} = {x}");
                i++;
            }

            Console.WriteLine();
            Console.WriteLine($"wynikGausJordan2");
            i = 1;
            foreach (double x in wynikGausJordan2)
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

            mother.IloscNiewiadomych = Convert.ToInt32(list[0]);
            mother.Macierz = new double[mother.IloscNiewiadomych, mother.IloscNiewiadomych + 1];

            for (int i = 0; i < mother.IloscNiewiadomych; i++)
            {
                string[] number = list[i + 1].Split(' ');

                if(number.Count() != mother.IloscNiewiadomych + 1)
                    continue;

                for (int j = 0; j < mother.IloscNiewiadomych + 1; j++)
                    mother.Macierz[i,j] = Convert.ToDouble(number[j]);
            }

            return mother;
        }

        static double[] Gauss(Mother mother)
        {
            double[] wynik = new double[mother.IloscNiewiadomych];

            double temp = 0;

            //przelatujemy po wierszach ktore sa dla nas glowne (na ich podstawie zerujemy reszte)
            for (int k = 0; k < mother.IloscNiewiadomych - 1; k++)
            {
                // przelatujemy po nastepnych wierszach (ktore chcemy wyzerowac
                for (int i = k + 1; i < mother.IloscNiewiadomych; i++)
                {
                    // obliczamy wartosc ktora musimy odjac zeby wyzerowac dana kolumne w wierszu j
                    temp = mother.Macierz[i, k] / mother.Macierz[k, k];

                    // obliczamy reszte zmiennych na podstawie tempa i wierszka ktory jest dla nas glowny
                    for (int j = k; j < mother.IloscNiewiadomych + 1; j++)
                        mother.Macierz[i, j] -= temp * mother.Macierz[k, j];
                }
            }

            // przelatujemy po wierszach ale od tylu 
            for (int k = mother.IloscNiewiadomych - 1; k >= 0; k--)
            {
                temp = 0;

                // jesli jest jakis wiersz "pod nami" to obliczamy ta niewiadoma
                for (int j = k + 1; j < mother.IloscNiewiadomych; j++)
                    temp += mother.Macierz[k, j] * wynik[j];

                //obliczenie niewiadomej , ostatni element wiersza - temp (inne niewiadome) dzielone przez obliczana niewiadoma
                wynik[k] = (mother.Macierz[k, mother.IloscNiewiadomych] - temp) / mother.Macierz[k, k];
            }

            return wynik;
        }

        public static double[] GaussElimination(Mother mother)
        {
            double[] x = new double[mother.IloscNiewiadomych];

            double tmp = 0;

            //WriteTableDouble(mother.macierz, mother.iloscNiewiadomych);

            for (int k = 0; k < mother.IloscNiewiadomych - 1; k++)
            {
                for (int i = k + 1; i < mother.IloscNiewiadomych; i++)
                {
                    tmp = mother.Macierz[i, k] / mother.Macierz[k, k];
                    for (int j = k; j < mother.IloscNiewiadomych + 1; j++)
                    {
                        mother.Macierz[i, j] -= tmp * mother.Macierz[k, j];
                    }

                    //WriteTableDouble(mother.macierz, mother.iloscNiewiadomych);
                }
            }

            //WriteTableDouble(mother.macierz, mother.iloscNiewiadomych);

            for (int k = mother.IloscNiewiadomych - 1; k >= 0; k--)
            {
                tmp = 0;
                for (int j = k + 1; j < mother.IloscNiewiadomych; j++)
                {
                    tmp += mother.Macierz[k, j] * x[j];
                }
                x[k] = (mother.Macierz[k, mother.IloscNiewiadomych] - tmp) / mother.Macierz[k, k];
            }

            return x;
        }

        static double[] GaussJordann(Mother mother)
        {
            double[] wynik = new double[mother.IloscNiewiadomych];

            double temp = 0;

            for (int k = 0; k < mother.IloscNiewiadomych; k++)
            {
                temp = mother.Macierz[k, k];

                for (int i = 0; i < mother.IloscNiewiadomych + 1; i++)
                    mother.Macierz[k, i] /= temp;

                for (int i = 0; i < mother.IloscNiewiadomych; i++)
                {
                    if (i == k)
                        continue;

                    temp = mother.Macierz[i, k] / mother.Macierz[k, k];

                    for (int j = k; j < mother.IloscNiewiadomych + 1; j++)
                        mother.Macierz[i, j] -= temp * mother.Macierz[k, j];
                }
            }

            for (int i = 0; i < mother.IloscNiewiadomych; i++)
                wynik[i] = mother.Macierz[i, mother.IloscNiewiadomych];

            return wynik;
        }

        public static double[] GaussJordanElimination(Mother mother)
        {
            Console.WriteLine("GaussElimination");

            double[] x = new double[mother.IloscNiewiadomych];
            double tmp = 0;

            //WriteTableDouble(mother.macierz, mother.iloscNiewiadomych);

            for (int k = 0; k < mother.IloscNiewiadomych; k++)
            {
                tmp = mother.Macierz[k, k];
                for (int i = 0; i < mother.IloscNiewiadomych + 1; i++)
                {
                    mother.Macierz[k, i] = mother.Macierz[k, i] / tmp;
                }

                for (int i = 0; i < mother.IloscNiewiadomych; i++)
                {
                    if (i != k)
                    {
                        tmp = mother.Macierz[i, k] / mother.Macierz[k, k];
                        for (int j = k; j < mother.IloscNiewiadomych + 1; j++)
                        {
                            mother.Macierz[i, j] -= tmp * mother.Macierz[k, j];
                        }
                    }
                }
            }

            //WriteTableDouble(mother.macierz, mother.iloscNiewiadomych);

            for (int i = 0; i < mother.IloscNiewiadomych; i++)
            {
                x[i] = mother.Macierz[i, mother.IloscNiewiadomych];
            }

            return x;
        }

        static void WriteMother(Mother mother)
        {
            for (int i = 0; i < mother.IloscNiewiadomych; i++)
            {
                for (int j = 0; j < mother.IloscNiewiadomych + 1; j++)
                {
                    Console.Write(mother.Macierz[i, j]);
                    Console.Write("\t");
                }

                Console.Write("\n");
            }
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
        public double[,] Macierz;
        public int IloscNiewiadomych;
    }
}
