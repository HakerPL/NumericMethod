using System;
using System.Collections.Generic;
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

            wynik = calculate(0, 10, 1000);
            Console.WriteLine($"Simson wynik = {wynik}");
            

            Console.ReadKey();
        }

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

        private static double calculate(double xp, double xk, double n)
        {
            double dx, calka, s, x;

            dx = (xk - xp) / n;

            calka = 0;
            s = 0;
            for (int i = 1; i < n; i++)
            {
                x = xp + i * dx;
                s += f(x - dx / 2);
                calka += f(x);
            }
            s += f(xk - dx / 2);
            calka = (dx / 6) * (f(xp) + f(xk) + 2 * calka + 4 * s);

            return calka;
        }
    }
}
