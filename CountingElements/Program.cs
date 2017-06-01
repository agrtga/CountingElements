using System;
using System.Text.RegularExpressions;

namespace CountingElements
{
    class Program
    {
        static void Main(string[] args)
        {
            string formula = GetFormula(args);
            var molecule = Molecule.Parse(formula);

            DisplayCounts(molecule);
        }

        static void DisplayCounts(Molecule molecule)
        {
            var elements = molecule.CountElements();

            foreach (var element in elements) {
                Console.WriteLine("{0}: {1}", element.Key, element.Value);
            }
        }

        static string GetFormula(string[] args)
        {
            if (args.Length != 1 || !CharactersValid(args[0])) {
                Console.WriteLine("Usage: CountingElements.exe formula\r\n\r\nformula = can contain letters, numbers, and parenthesis");
                Environment.Exit(0);
            }

            return args[0];
        }

        static bool CharactersValid(string formula)
            => Regex.IsMatch(formula, @"^[A-Za-z0-9\(\)]*$");
    }
}
