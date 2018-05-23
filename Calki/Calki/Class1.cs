using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calki
{
    class Class1
    {

    }
}



#include <iostream>
using namespace std;
double f(double x)
{
//return x*x*x+2*x;
return x * x;
}
double prostokat(double min, double max, int n)
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
double trapezow(double min, double max, int n)
{
double suma, h;
int i;
//cout << setprecision(3) << fixed;               
cout << endl;
suma = 0;
h = (max - min) / n;
//for (i = 1; i < n; i++) { suma += f(min + i * h); }
//suma = (suma + (f(min) + f(max)) / 2) * h;
for (int i = 0; i < n; i++)
{
double p = h / 2 * (f(min + i * h) + f(min + (i + 1) * h));
suma += p;
}
return suma;
}
/*double simpsona(double min, double max, int n)
{
    double h = (max - min) / n;
    double sum = 0;
    for (int i = 0; i < n; i++)
    {
        double pole = (h / 6)*(f(min) + f(max) + 4 * f((max - min) / 2));
        sum += pole;
    }
    return sum;
}*/
double simpsona(double min, double max, int n)
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
int main()
{
cout << prostokat(0, 4, 4) << endl;
cout << prostokat(0, 4, 4) << endl;
cout << trapezow(0, 4, 4) << endl;
cout << trapezow(0, 4, 4) << endl;
cout << simpsona(0, 4, 4) << endl;
cout << simpsona(0, 4, 4) << endl;
system("pause");
//return 0;
}
