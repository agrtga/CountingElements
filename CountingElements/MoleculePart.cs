using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CountingElements
{
    public sealed class MoleculePart
    {
        const string PartPattern = @"^([A-Z]{1}[a-z]?[0-9]*)$";

        public string Element { get; set; }
        public ushort Count { get; set; } = 1;

        public static MoleculePart Parse(string formula)
        {
            ArgCheck.IsNullOrEmpty(formula, nameof(formula));
            var part = new MoleculePart();

            if (IsFormulaValid(formula)) {
                var elementChars = formula.TakeWhile(c => !char.IsNumber(c)).ToArray();
                var countString = formula.Substring(elementChars.Length);

                part.Element = new string(elementChars);

                if (countString.Length > 0) {
                    part.Count = ushort.Parse(countString);
                }

                return part;
            }
            else {
                throw new FormatException("The format for the element part is invalid; examples: C, Ch, C2");
            }
        }

        public override string ToString()
            => Element + (Count > 1 ? Count.ToString() : string.Empty);

        static bool IsFormulaValid(string formula)
            => Regex.IsMatch(formula, PartPattern);
    }
}
