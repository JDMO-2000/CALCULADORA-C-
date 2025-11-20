using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BadCalcVeryBad
{
    // Cambio: antes se usaba ArrayList sin tipo. 
    // Ahora usamos List<string> que es más sencillo y más seguro.
    public class Globals
    {
        public static List<string> History = new List<string>();
        public static int Counter = 0;
        public static string LastLine = "";
    }

    // Calculadora básica con operaciones simples.
    // Cambio: quitamos cosas raras como Random y object any.
    public class ShoddyCalc
    {
        // Método que hace la operación según el operador.
        public double DoIt(double a, double b, string op)
        {
            switch (op)
            {
                case "+":
                    return a + b; // antes hacía +0-0 sin sentido
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    if (b == 0)
                    {
                        // Cambio: controlamos explícitamente la división por cero.
                        throw new DivideByZeroException("No se puede dividir por cero.");
                    }
                    return a / b;
                case "^":
                    // Cambio: usamos Math.Pow en vez de ciclo manual.
                    return Math.Pow(a, b);
                case "%":
                    return a % b;
                default:
                    throw new InvalidOperationException("Operación no válida.");
            }
        }

        // Raíz cuadrada con Math.Sqrt.
        public double Sqrt(double value)
        {
            if (value < 0)
            {
                // En el código original devolvía negativa la raíz del valor absoluto.
                // Dejamos el mismo comportamiento pero de forma clara.
                return -Math.Sqrt(Math.Abs(value));
            }

            return Math.Sqrt(value);
        }
    }

    class Program
    {
        // Instancias globales simples.
        public static ShoddyCalc calc = new ShoddyCalc();

        static void Main(string[] args)
        {
            bool salir = false;

            while (!salir)
            {
                MostrarMenu();
                Console.Write("Opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "0":
                        salir = true;
                        break;
                    case "9":
                        MostrarHistorial();
                        break;
                    case "7":
                        EjecutarRaizCuadrada();
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                        EjecutarOperacionBinaria(opcion);
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                Console.WriteLine();
            }

            GuardarHistorialEnArchivo();
        }

        // Cambio: mostramos un menú más claro y sin opciones extrañas.
        static void MostrarMenu()
        {
            Console.WriteLine("BAD CALC - versión corregida");
            Console.WriteLine("1) Suma");
            Console.WriteLine("2) Resta");
            Console.WriteLine("3) Multiplicación");
            Console.WriteLine("4) División");
            Console.WriteLine("5) Potencia");
            Console.WriteLine("6) Módulo");
            Console.WriteLine("7) Raíz cuadrada");
            Console.WriteLine("9) Ver historial");
            Console.WriteLine("0) Salir");
        }

        // Operaciones que usan dos números (a y b)
        static void EjecutarOperacionBinaria(string opcion)
        {
            double a, b;
            if (!LeerNumero("a", out a) || !LeerNumero("b", out b))
            {
                Console.WriteLine("No se pudo leer uno de los números.");
                return;
            }

            string op = ObtenerOperador(opcion);

            try
            {
                double resultado = calc.DoIt(a, b, op);
                RegistrarYMostrarResultado(a, b, op, resultado);
            }
            catch (Exception ex)
            {
                // Cambio: en lugar de capturas vacías, mostramos el error.
                Console.WriteLine("Error al realizar la operación: " + ex.Message);
            }
        }

        // Operación de raíz cuadrada (solo usa a)
        static void EjecutarRaizCuadrada()
        {
            double a;
            if (!LeerNumero("a", out a))
            {
                Console.WriteLine("No se pudo leer el número.");
                return;
            }

            try
            {
                double resultado = calc.Sqrt(a);
                // Para raíz, podemos guardar b como "-" en el historial.
                RegistrarYMostrarResultado(a, double.NaN, "sqrt", resultado);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al calcular la raíz: " + ex.Message);
            }
        }

        // Lectura de un número double desde consola.
        // Cambio: usamos double.TryParse con CultureInfo.InvariantCulture
        // y permitimos coma o punto.
        static bool LeerNumero(string nombre, out double valor)
        {
            Console.Write(nombre + ": ");
            string texto = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(texto))
            {
                valor = 0;
                return false;
            }

            string normalizado = texto.Replace(',', '.');

            return double.TryParse(
                normalizado,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out valor);
        }

        // Devuelve el operador según la opción del menú.
        static string ObtenerOperador(string opcion)
        {
            switch (opcion)
            {
                case "1": return "+";
                case "2": return "-";
                case "3": return "*";
                case "4": return "/";
                case "5": return "^";
                case "6": return "%";
                default: return "";
            }
        }

        // Registra el resultado en memoria y en pantalla.
        static void RegistrarYMostrarResultado(double a, double b, string op, double resultado)
        {
            string bTexto = double.IsNaN(b)
                ? "-"
                : b.ToString("0.###############", CultureInfo.InvariantCulture);

            string linea = string.Format(
                CultureInfo.InvariantCulture,
                "{0}|{1}|{2}|{3}",
                a.ToString("0.###############", CultureInfo.InvariantCulture),
                bTexto,
                op,
                resultado.ToString("0.###############", CultureInfo.InvariantCulture)
            );

            Globals.History.Add(linea);
            Globals.LastLine = linea;
            Globals.Counter++;

            Console.WriteLine("= " + resultado.ToString("0.###############", CultureInfo.InvariantCulture));
        }

        // Muestra el historial en pantalla.
        static void MostrarHistorial()
        {
            if (Globals.History.Count == 0)
            {
                Console.WriteLine("No hay operaciones en el historial.");
                return;
            }

            Console.WriteLine("=== HISTORIAL ===");
            foreach (var item in Globals.History)
            {
                Console.WriteLine(item);
            }
        }

        // Guarda el historial en un archivo de texto.
        static void GuardarHistorialEnArchivo()
        {
            try
            {
                string nombreArchivo = "history.txt";
                File.WriteAllLines(nombreArchivo, Globals.History);
                Console.WriteLine("Historial guardado en el archivo " + nombreArchivo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se pudo guardar el historial: " + ex.Message);
            }
        }
    }
}
