using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.Business
{
    public class A
    {

        public int X;
        public int Y;

        public int Suma()
        {
            return X + Y;
        }

    }

    public class B : A
    {

        public int Multiplicar()
        {
            return X * Y;
        }
    }

    public class Main
    {
        public Main()
        {
            Console.Write(" Hola probando la clase A, SUMA ");

            A mihijodeA = new A();
            Console.Write(" Ingrese el valor de X: ");
            mihijodeA.X = Console.Read();
            Console.Write(" Ingrese el valor de Y:");
            mihijodeA.Y = Console.Read();
          
            int resultado = mihijodeA.Suma();


            Console.Write(" El resultado es:" + resultado.ToString());



            B hijodeB = new B();

          

        }
    }
}
